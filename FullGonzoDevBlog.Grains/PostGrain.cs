using FullGonzoDevBlog.Abstractions;
using FullGonzoDevBlog.Abstractions.Models;

namespace FullGonzoDevBlog.Grains
{
	public class PostGrain : IPostGrain
	{
		private readonly IGrainFactory _grainFactory;
		private Post _post = new();

		public PostGrain(IGrainFactory grainFactory)
		{
			_grainFactory = grainFactory;
		}

		public async Task<Post> GetPost()
		{
			return await Task.FromResult(_post);
		}

		public Task UpdatePost(Post post)
		{
			_post = post;
			return Task.CompletedTask;
		}
	}
}
