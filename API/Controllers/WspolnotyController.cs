
using API.Data;
using API.DTOs;
using API.Entities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace API.Controllers
{

    public class WspolnotyController : BaseApiController
    {
        private readonly WspolnotaRepository _wspolnoty;
        private readonly DataContext _context;

        public WspolnotyController(WspolnotaRepository wspolnoty, DataContext context)
        {
            _wspolnoty = wspolnoty;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Wspolnota>>> GetWspolnoty()
        {
            return Ok(await _wspolnoty.GetWspolnotyAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Wspolnota>> GetWspolnota(int id)
        {
            var wspolnota = await _wspolnoty.GetWspolnotaByIdAsync(id);

            if (wspolnota == null)
            {
                return NotFound();
            }

            return Ok(wspolnota);
        }

        [HttpPost("dodaj-wspolnote")]
        public async Task<ActionResult> AddWspolnotaToDb(WspolnotaDto wspolnota)
        {
            if (await _context.wspolnoty.AnyAsync(x => x.nazwa == wspolnota.nazwa)) return BadRequest("Wspolnota ta istnieje");

            var nowaWspolnota = new Wspolnota
            {
                nazwa = wspolnota.nazwa,
                miasto = wspolnota.miasto
            };

            await _context.wspolnoty.AddAsync(nowaWspolnota);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("usun-wspolnote/{id}")]
        public async Task<ActionResult> DeleteWspolnota(int id)
        {
            var wspolnota = await _context.wspolnoty.FirstOrDefaultAsync(x => x.Id == id);

            if (wspolnota == null) return BadRequest("Taka wspolnota nie istnieje");

            _context.wspolnoty.Remove(wspolnota);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}