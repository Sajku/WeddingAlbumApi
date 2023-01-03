﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeddingAlbum.PublishedLanguage.Dtos
{
    public class PhotoCommentDTO
    {
        public int CommentId { get; set; }
        public string AuthorName { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }
        public int LikesCount { get; set; }
    }
}
