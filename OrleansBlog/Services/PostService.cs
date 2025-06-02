using OrleansBlog.Abstractions;
using OrleansBlog.Models;

namespace OrleansBlog.Services
{
    public class PostService : IPostService
    {
        private readonly IClusterClient _clusterClient;
        private static long _nextPostId = 1;
        private static readonly ConcurrentQueue<long> _postIds = new();

        public PostService(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        public async Task<Post?> GetPostAsync(long id)
        {
            var grain = _clusterClient.GetGrain<IPostGrain>(id);
            var orleansPost = await grain.GetPost();
            
            if (orleansPost == null || orleansPost.Id == 0)
                return null;

            return MapToLocalPost(orleansPost);
        }

        public async Task<long> CreatePostAsync(Post post)
        {
            var postId = Interlocked.Increment(ref _nextPostId);
            var grain = _clusterClient.GetGrain<IPostGrain>(postId);
            
            var orleansPost = MapToOrleansPost(post);
            orleansPost.Id = (int)postId;
            
            await grain.CreatePost(orleansPost);
            _postIds.Add(postId);
            
            return postId;
        }

        public async Task UpdatePostAsync(Post post)
        {
            var grain = _clusterClient.GetGrain<IPostGrain>(post.Id);
            var orleansPost = MapToOrleansPost(post);
            await grain.UpdatePost(orleansPost);
        }

        public async Task<List<Post>> GetRecentPostsAsync(int count = 10)
        {
            var posts = new List<Post>();
            var recentIds = _postIds.OrderByDescending(id => id).Take(count);

            foreach (var id in recentIds)
            {
                var post = await GetPostAsync(id);
                if (post != null)
                {
                    posts.Add(post);
                }
            }

            return posts.OrderByDescending(p => p.Created).ToList();
        }

        private Post MapToLocalPost(OrleansBlog.Abstractions.Models.Post orleansPost)
        {
            return new Post
            {
                Id = orleansPost.Id,
                Title = orleansPost.Title,
                Content = orleansPost.Content,
                Created = orleansPost.Created,
                Updated = orleansPost.Updated,
                AuthorId = orleansPost.AuthorId,
                Tags = orleansPost.Tags
            };
        }

        private OrleansBlog.Abstractions.Models.Post MapToOrleansPost(Post localPost)
        {
            return new OrleansBlog.Abstractions.Models.Post
            {
                Id = (int)localPost.Id,
                Title = localPost.Title,
                Content = localPost.Content,
                Created = localPost.Created,
                Updated = localPost.Updated,
                AuthorId = localPost.AuthorId,
                Tags = localPost.Tags
            };
        }
    }
}