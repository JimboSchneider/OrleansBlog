using OrleansBlog.Abstractions.Models;

namespace OrleansBlog.Abstractions
{
	[Alias("OrleansBlog.Abstractions.IPostGrain")]
	public interface IPostGrain : IGrainWithIntegerKey
    {
		Task<Post> GetPost();
		Task UpdatePost(Post post);
	}
}
