namespace OrleansBlog.Models
{
	public class Post
	{
		public long Id { get; set; }
		public string Title { get; set; } = string.Empty; // Ensure non-null
		public required string Content { get; set; } = string.Empty; // Ensure non-null
		public DateTime Created { get; set; }
		public DateTime? Updated { get; set; }
		public required string AuthorId { get; set; } = string.Empty; // Ensure non-null
		public required string[] Tags { get; set; } = []; // Ensure non-null
	}
}