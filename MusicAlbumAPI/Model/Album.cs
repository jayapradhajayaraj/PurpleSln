using System.ComponentModel.DataAnnotations;

namespace MusicAlbumAPI.Model
{
    public class Album
    {

            [Required]  
            public int Id { get; set; }

            [Required]
            public string Name { get; set; }

            [Required]
            public string Artist { get; set; }

            [Required]
            public string Genre { get; set; }
            public DateTime ReleaseDate { get; set; }
            public int Rating { get; set; }
    }

   
}
