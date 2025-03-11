using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullGonzoDevBlog.Abstractions.Models
{
	[GenerateSerializer, Immutable]
	[Alias("FullGonzoDevBlog.Abstractions.Models.Post")]
	public record Post
    {
		[Id(0)]
		public int Id { get; set; }
		[Id(1)]
		public string Title { get; set; }
		[Id(2)]
		public string Content { get; set; }
		[Id(3)]
		public DateTime Created { get; set; }
		[Id(4)]
		public DateTime? Updated { get; set; }
		[Id(5)]
		public string AuthorId { get; set; }
		[Id(6)]
		public string[] Tags { get; set; }
	}
}
