﻿using System;

namespace OrleansBlog.Abstractions.Models
{
    [GenerateSerializer, Immutable]
    [Alias("OrleansBlog.Abstractions.Models.Post")]
    public record Post
    {
        [Id(0)]
        public int Id { get; set; }
		
        [Id(1)]
        public string Title { get; set; } = string.Empty; // Default value

        [Id(2)]
        public string Content { get; set; } = string.Empty; // Default value

        [Id(3)]
        public DateTime Created { get; set; }

        [Id(4)]
        public DateTime? Updated { get; set; }

        [Id(5)]
        public string AuthorId { get; set; } = string.Empty; // Default value

        [Id(6)]
        public string[] Tags { get; set; } = []; // Default value
    }
}