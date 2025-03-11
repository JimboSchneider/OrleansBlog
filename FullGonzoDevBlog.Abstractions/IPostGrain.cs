using FullGonzoDevBlog.Abstractions.Models;

namespace FullGonzoDevBlog.Abstractions
{
	[Alias("FullGonzoDevBlog.Abstractions.IPostGrain")]
	public interface IPostGrain : IGrainWithIntegerKey
    {
		Task<Post> GetPost();
		Task UpdatePost(Post post);
	}
}
