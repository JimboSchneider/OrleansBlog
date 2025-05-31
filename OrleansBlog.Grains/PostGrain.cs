using OrleansBlog.Abstractions;
using OrleansBlog.Abstractions.Models;

namespace OrleansBlog.Grains
{
	public class PostGrain : Grain, IPostGrain
	{
		private Post? _post;

		public Task<Post?> GetPost()
		{
			return Task.FromResult(_post);
		}

		public Task CreatePost(Post post)
		{
			if (_post != null && _post.Id > 0)
			{
				throw new InvalidOperationException("Post already exists");
			}

			post.Id = (int)this.GetPrimaryKeyLong();
			post.Created = DateTime.UtcNow;
			_post = post;
			return Task.CompletedTask;
		}

		public Task UpdatePost(Post post)
		{
			if (_post == null || _post.Id == 0)
			{
				throw new InvalidOperationException("Post does not exist");
			}

			post.Id = _post.Id;
			post.Created = _post.Created;
			post.Updated = DateTime.UtcNow;
			_post = post;
			return Task.CompletedTask;
		}
	}
}
