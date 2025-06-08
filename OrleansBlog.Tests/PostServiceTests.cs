using Moq;
using Orleans;
using OrleansBlog.Abstractions;
using OrleansBlog.Models;
using OrleansBlog.Services;

namespace OrleansBlog.Tests;

public class PostServiceTests
{
    private readonly Mock<IClusterClient> _mockClusterClient;
    private readonly PostService _postService;

    public PostServiceTests()
    {
        _mockClusterClient = new Mock<IClusterClient>();
        _postService = new PostService(_mockClusterClient.Object);
    }

    [Fact]
    public async Task GetPostAsync_WhenPostExists_ReturnsPost()
    {
        // Arrange
        var postId = 1L;
        var mockGrain = new Mock<IPostGrain>();
        var orleansPost = new OrleansBlog.Abstractions.Models.Post
        {
            Id = 1,
            Title = "Test Post",
            Content = "Test Content",
            AuthorId = "user123",
            Created = DateTime.UtcNow,
            Tags = ["test"]
        };

        mockGrain.Setup(g => g.GetPost()).ReturnsAsync(orleansPost);
        _mockClusterClient.Setup(c => c.GetGrain<IPostGrain>(postId, null))
            .Returns(mockGrain.Object);

        // Act
        var result = await _postService.GetPostAsync(postId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Post", result.Title);
        Assert.Equal("Test Content", result.Content);
        Assert.Equal("user123", result.AuthorId);
    }

    [Fact]
    public async Task GetPostAsync_WhenPostDoesNotExist_ReturnsNull()
    {
        // Arrange
        var postId = 999L;
        var mockGrain = new Mock<IPostGrain>();
        mockGrain.Setup(g => g.GetPost()).ReturnsAsync((OrleansBlog.Abstractions.Models.Post)null!);
        _mockClusterClient.Setup(c => c.GetGrain<IPostGrain>(postId, null))
            .Returns(mockGrain.Object);

        // Act
        var result = await _postService.GetPostAsync(postId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreatePostAsync_CreatesPostAndReturnsId()
    {
        // Arrange
        var mockGrain = new Mock<IPostGrain>();
        var post = new Post
        {
            Title = "New Post",
            Content = "New Content",
            AuthorId = "user456",
            Tags = ["new", "test"]
        };

        mockGrain.Setup(g => g.CreatePost(It.IsAny<OrleansBlog.Abstractions.Models.Post>()))
            .Returns(Task.CompletedTask);
        
        _mockClusterClient.Setup(c => c.GetGrain<IPostGrain>(It.IsAny<long>(), null))
            .Returns(mockGrain.Object);

        // Act
        var postId = await _postService.CreatePostAsync(post);

        // Assert
        Assert.True(postId > 0);
        mockGrain.Verify(g => g.CreatePost(It.Is<OrleansBlog.Abstractions.Models.Post>(p => 
            p.Title == "New Post" && 
            p.Content == "New Content" && 
            p.AuthorId == "user456"
        )), Times.Once);
    }

    [Fact]
    public async Task UpdatePostAsync_CallsGrainUpdate()
    {
        // Arrange
        var mockGrain = new Mock<IPostGrain>();
        var post = new Post
        {
            Id = 5,
            Title = "Updated Post",
            Content = "Updated Content",
            AuthorId = "user789"
        };

        mockGrain.Setup(g => g.UpdatePost(It.IsAny<OrleansBlog.Abstractions.Models.Post>()))
            .Returns(Task.CompletedTask);
        
        _mockClusterClient.Setup(c => c.GetGrain<IPostGrain>(post.Id, null))
            .Returns(mockGrain.Object);

        // Act
        await _postService.UpdatePostAsync(post);

        // Assert
        mockGrain.Verify(g => g.UpdatePost(It.Is<OrleansBlog.Abstractions.Models.Post>(p => 
            p.Title == "Updated Post" && 
            p.Content == "Updated Content"
        )), Times.Once);
    }

    [Fact]
    public async Task GetRecentPostsAsync_ReturnsOrderedPosts()
    {
        // Arrange
        var olderDate = DateTime.UtcNow.AddDays(-2);
        var newerDate = DateTime.UtcNow.AddDays(-1);

        // Create posts
        var post1 = new Post { Title = "Post 1", Content = "Content 1", AuthorId = "user1" };
        var post2 = new Post { Title = "Post 2", Content = "Content 2", AuthorId = "user2" };

        // Setup grains for each post
        var mockGrain1 = new Mock<IPostGrain>();
        var orleansPost1 = new OrleansBlog.Abstractions.Models.Post
        {
            Id = 1,
            Title = "Post 1",
            Content = "Content 1",
            AuthorId = "user1",
            Created = olderDate
        };
        mockGrain1.Setup(g => g.GetPost()).ReturnsAsync(orleansPost1);

        var mockGrain2 = new Mock<IPostGrain>();
        var orleansPost2 = new OrleansBlog.Abstractions.Models.Post
        {
            Id = 2,
            Title = "Post 2",
            Content = "Content 2",
            AuthorId = "user2",
            Created = newerDate
        };
        mockGrain2.Setup(g => g.GetPost()).ReturnsAsync(orleansPost2);

        // Create posts first
        _mockClusterClient.SetupSequence(c => c.GetGrain<IPostGrain>(It.IsAny<long>(), null))
            .Returns(mockGrain1.Object)
            .Returns(mockGrain2.Object)
            .Returns(mockGrain2.Object)  // For GetRecentPosts call
            .Returns(mockGrain1.Object); // For GetRecentPosts call

        await _postService.CreatePostAsync(post1);
        await _postService.CreatePostAsync(post2);

        // Act
        var recentPosts = await _postService.GetRecentPostsAsync(2);

        // Assert
        Assert.Equal(2, recentPosts.Count);
        Assert.Equal("Post 2", recentPosts[0].Title); // Newer post should be first
        Assert.Equal("Post 1", recentPosts[1].Title);
    }
}