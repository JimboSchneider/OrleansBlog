using OrleansBlog.Models;

namespace OrleansBlog.Services
{
    public interface IPostService
    {
        Task<Post?> GetPostAsync(long id);
        Task<long> CreatePostAsync(Post post);
        Task UpdatePostAsync(Post post);
        Task<List<Post>> GetRecentPostsAsync(int count = 10);
    }
}