﻿namespace WeddingAlbum.PublishedLanguage.Dtos
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Salt { get; set; }
        public string Hash { get; set; }
    }
}
