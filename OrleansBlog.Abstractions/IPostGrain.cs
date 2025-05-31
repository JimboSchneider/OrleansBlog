using OrleansBlog.Abstractions.Models;

namespace OrleansBlog.Abstractions
{
	[Alias("OrleansBlog.Abstractions.IPostGrain")]
	public interface IPostGrain : IGrainWithIntegerKey
    {
		Task<Post?> GetPost();
		Task CreatePost(Post post);
		Task UpdatePost(Post post);
	}
}
