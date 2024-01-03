using Microsoft.EntityFrameworkCore;
using MusicAlbumAPI.Model;
using System.Collections.Generic;


namespace MusicAlbumAPI.DBContext
{
    public class MusicDbContext : DbContext
    {

        public DbSet<Album> Albums { get; set; }

        public MusicDbContext(DbContextOptions<MusicDbContext> options)
            : base(options)
        {
        }


    }

}

