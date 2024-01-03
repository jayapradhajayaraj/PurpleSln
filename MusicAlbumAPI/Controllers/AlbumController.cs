using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicAlbumAPI.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MusicAlbumAPI.DBContext;

namespace MusicAlbumAPI.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/AlbumController")]
    public class AlbumController : ControllerBase
    {
        private readonly MusicDbContext _dbContext;

        public AlbumController(MusicDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Album>>> GetAlbums()
        {
            try
            {
                var albums = await _dbContext.Albums.ToListAsync();
                return Ok(albums);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error occurred while fetching the album: {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Album>> GetAlbumById(int id)
        {
            try
            {
                var album = await _dbContext.Albums.FindAsync(id);
                if (album == null)
                {
                    return NotFound(); 
                }
                return Ok(album);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Album>> CreateAlbum(Album album)
        {
            try
            {
                _dbContext.Albums.Add(album);
                await _dbContext.SaveChangesAsync();
                return CreatedAtAction(nameof(GetAlbumById), new { id = album.Id }, album);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error occurred while creating the album: {ex.Message}");
            }
        }


        [AllowAnonymous]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAlbum(int id, Album album)
        {
            if (id != album.Id)
            {
                return BadRequest();
            }
            _dbContext.Entry(album).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_dbContext.Albums.Any(a => a.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }


        [AllowAnonymous]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAlbum(int id)
        {
            try
            {
                var album = await _dbContext.Albums.FindAsync(id);
                if (album == null)
                {
                    return NotFound();
                }
                _dbContext.Albums.Remove(album);
                await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error occurred while deleting the album: {ex.Message}");

            }
        }


        [AllowAnonymous]
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Album>>> SearchAlbums([FromQuery] string name, string artist, string genre)
        {
            try
            {
                var query = _dbContext.Albums.AsQueryable();
                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(a => a.Name.Contains(name));
                }
                if (!string.IsNullOrEmpty(artist))
                {
                    query = query.Where(a => a.Artist.Contains(artist));
                }
                if (!string.IsNullOrEmpty(genre))
                {
                    query = query.Where(a => a.Genre.Contains(genre));
                }
                var result = await query.ToListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error occurred while searching the album: {ex.Message}");
            }
        }
    }
}


