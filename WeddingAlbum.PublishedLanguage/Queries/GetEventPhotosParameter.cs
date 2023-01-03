﻿using System.Collections.Generic;
using WeddingAlbum.Common.CQRS;
using WeddingAlbum.PublishedLanguage.Dtos;

namespace WeddingAlbum.PublishedLanguage.Queries
{
    public class GetEventPhotosParameter : IQuery<List<ShortPhotoDTO>>
    {
        public int EventId { get; set; }
    }
}
