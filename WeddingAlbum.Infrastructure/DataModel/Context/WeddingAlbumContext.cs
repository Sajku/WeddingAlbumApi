﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WeddingAlbum.Common.Auth;
using WeddingAlbum.Domain.Albums;
using WeddingAlbum.Domain.Comments;
using WeddingAlbum.Domain.Common;
using WeddingAlbum.Domain.Events;
using WeddingAlbum.Domain.PhotoInAlbums;
using WeddingAlbum.Domain.Photos;
using WeddingAlbum.Domain.UserFavouriteAlbums;
using WeddingAlbum.Domain.UserInEvents;
using WeddingAlbum.Domain.Users;
using WeddingAlbum.Infrastructure.DataModel.Mappings;

namespace WeddingAlbum.Infrastructure.DataModel.Context
{
    public class WeddingAlbumContext : DbContext
    {
        private readonly ICurrentUserService _currentUserService;

        public WeddingAlbumContext() { }

        public WeddingAlbumContext(DbContextOptions<WeddingAlbumContext> options, ICurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<UserInEvent> UserInEvents { get; set; }
        public DbSet<PhotoInAlbum> PhotoInAlbums { get; set; }
        public DbSet<UserFavouriteAlbum> UserFavouriteAlbums { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new PhotoConfiguration());
            builder.ApplyConfiguration(new EventConfiguration());
            builder.ApplyConfiguration(new CommentConfiguration());
            builder.ApplyConfiguration(new AlbumConfiguration());
            builder.ApplyConfiguration(new UserInEventConfiguration());
            builder.ApplyConfiguration(new PhotoInAlbumConfiguration());
            builder.ApplyConfiguration(new UserFavouriteAlbumConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfiguration configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .Build();

                optionsBuilder.UseSqlServer(configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING"));
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.CreatedOn = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedBy = _currentUserService.UserId;
                        entry.Entity.UpdatedOn = DateTime.Now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
