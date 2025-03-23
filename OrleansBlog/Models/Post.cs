namespace OrleansBlog.Models
{
	public class Post
	{
		public long Id { get; set; }

		public string Title { get; set; }

		public string Content { get; set; }

		public DateTime Created { get; set; }

		public DateTime? Updated { get; set; }

		public string AuthorId { get; set; }

		public string[] Tags { get; set; }
	}
}
