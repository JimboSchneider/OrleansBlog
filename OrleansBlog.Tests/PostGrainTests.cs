using Orleans.TestingHost;
using OrleansBlog.Abstractions;
using OrleansBlog.Abstractions.Models;

namespace OrleansBlog.Tests;

public class PostGrainTests : IAsyncLifetime
{
    private TestCluster? _cluster;
    private IClusterClient? _client;

    public async Task InitializeAsync()
    {
        var builder = new TestClusterBuilder();
        builder.AddSiloBuilderConfigurator<TestSiloConfigurations>();
        _cluster = builder.Build();
        await _cluster.DeployAsync();
        _client = _cluster.Client;
    }

    public async Task DisposeAsync()
    {
        if (_cluster != null)
        {
            await _cluster.StopAllSilosAsync();
            await _cluster.DisposeAsync();
        }
    }

    [Fact]
    public async Task GetPost_WhenNewGrain_ReturnsNullPost()
    {
        // Arrange
        var grain = _client?.GetGrain<IPostGrain>(1);

        // Act
        if (grain != null)
        {
            var post = await grain.GetPost();

            // Assert
            Assert.Null(post);
        }
    }

    [Fact]
    public async Task CreatePost_WhenNewGrain_StoresPost()
    {
        // Arrange
        var grain = _client?.GetGrain<IPostGrain>(2);
        var newPost = new Post
        {
            Title = "Test Post",
            Content = "Test Content",
            AuthorId = "user123",
            Tags = ["test", "unit-test"]
        };

        // Act
        if (grain != null)
        {
            await grain.CreatePost(newPost);
            var retrievedPost = await grain.GetPost();

            // Assert
            Assert.NotNull(retrievedPost);
            Assert.Equal("Test Post", retrievedPost.Title);
            Assert.Equal("Test Content", retrievedPost.Content);
            Assert.Equal("user123", retrievedPost.AuthorId);
            Assert.Equal(2, retrievedPost.Id);
            Assert.NotEqual(DateTime.MinValue, retrievedPost.Created);
            Assert.Null(retrievedPost.Updated);
            Assert.Equal(2, retrievedPost.Tags.Length);
        }
    }

    [Fact]
    public async Task CreatePost_WhenPostAlreadyExists_ThrowsException()
    {
        // Arrange
        var grain = _client?.GetGrain<IPostGrain>(3);
        var post = new Post
        {
            Title = "First Post",
            Content = "First Content",
            AuthorId = "user123"
        };

        // Act
        if (grain != null)
        {
            await grain.CreatePost(post);

            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await grain.CreatePost(post));
        }
    }

    [Fact]
    public async Task UpdatePost_WhenPostExists_UpdatesPost()
    {
        // Arrange
        var grain = _client?.GetGrain<IPostGrain>(4);
        var originalPost = new Post
        {
            Title = "Original Title",
            Content = "Original Content",
            AuthorId = "user123"
        };
        if (grain != null)
        {
            await grain.CreatePost(originalPost);

            // Get the created post to access its created date
            var createdPost = await grain.GetPost();
            var originalCreatedDate = createdPost?.Created;

            // Act
            var updatedPost = new Post
            {
                Title = "Updated Title",
                Content = "Updated Content",
                AuthorId = "user123",
                Tags = ["updated"]
            };
            await grain.UpdatePost(updatedPost);

            // Assert
            var retrievedPost = await grain.GetPost();
            Assert.Equal("Updated Title", retrievedPost?.Title);
            Assert.Equal("Updated Content", retrievedPost?.Content);
            Assert.NotNull(retrievedPost?.Updated);
            Assert.Equal(4, retrievedPost.Id); // ID should remain the same
            Assert.Equal(originalCreatedDate, retrievedPost.Created); // Created date should remain the same
            Assert.True(retrievedPost.Updated > retrievedPost.Created); // Updated should be after created
        }
    }

    [Fact]
    public async Task UpdatePost_WhenPostDoesNotExist_ThrowsException()
    {
        // Arrange
        var grain = _client?.GetGrain<IPostGrain>(5);
        var post = new Post
        {
            Title = "Title",
            Content = "Content",
            AuthorId = "user123"
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            if (grain != null) await grain.UpdatePost(post);
        });
    }

    private class TestSiloConfigurations : ISiloConfigurator
    {
        public void Configure(ISiloBuilder siloBuilder)
        {
            siloBuilder.AddMemoryGrainStorage("PostStore");
        }
    }
}