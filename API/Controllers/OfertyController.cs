using API.Data;
using API.Data.Repositories;
using API.DTOs;
using API.Entities;
using API.Helpers;
using Core.Specification;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class OfertyController : BaseApiController
    {
        private readonly OfertaRepository _oferty;
        private readonly DataContext _context;

        public OfertyController(OfertaRepository oferty, DataContext context)
        {
            _context = context;
            _oferty = oferty;
        }

        [HttpPost("all/{idWspolnoty}")]
        public async Task<ActionResult<Pagination<OfertaDto>>> GetOferty(int idWspolnoty, OfertySpecParams ofertyParams)
        {
            var spec = new ProductsWithTypersAndBrandSpecification(ofertyParams);
            var countSpec = new ProductWithFiltersForCountSpecification(ofertyParams);
            var totalItems = await _oferty.CountAsync(countSpec);
            var oferty = await _oferty.GetOfertyAsync(idWspolnoty, spec);
            return Ok(new Pagination<OfertaDto>(ofertyParams.PageIndex, ofertyParams.PageSize, totalItems, oferty));
        }
        
        [HttpGet("{idOferty}")]
        public async Task<ActionResult<OfertaDto>> GetOferta(int idOferty)
        {
            return Ok(await _oferty.GetOfertaByIdAsync(idOferty));
        }

        [HttpPost("all/user")]
        public async Task<ActionResult<IEnumerable<Oferta>>> GetOfertyByUserAndWspolnota(WspolnotaUzytkownikDto wspolnotaUzytkownikDto)
        {
            return Ok(await _oferty.GetOfertyByWspolnotaAndUzytkownikAsync(wspolnotaUzytkownikDto.idWspolnoty, wspolnotaUzytkownikDto.idUzytkownika));
        }
        [HttpPut("change-oferta-to-pending")]
        public async Task<ActionResult<IEnumerable<Oferta>>> ChangeToPending(int idOferty)
        {
             var i = await _oferty.ChangeToPending(idOferty);
            if (i > 0)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
          [HttpPut("change-oferta-to-false")]
        public async Task<ActionResult<IEnumerable<Oferta>>> ChangeToFalse(int idOferty)
        {
             var i = await _oferty.ChangeToFalse(idOferty);
            if (i > 0)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("add/komentarz")]
        public async Task<ActionResult<IEnumerable<Oferta>>> AddKomentarzToOferta(KomentarzOfertyDto komentarzOfertyDto)
        {
            var i = await _oferty.AddKomentarzToOferta(komentarzOfertyDto);
            if (i > 0)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpPost("add/oferta")]
        public async Task<ActionResult<IEnumerable<Oferta>>> AddOferta([FromForm] AddOfertaDto oferta)
        {
            var i = await _oferty.AddOferta(oferta);
            if (i > 0)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpDelete("usun-oferte/{id}")]

        public async Task<ActionResult> DeleteOferta(int id)
        {
            var oferta = await _context.oferty.FirstOrDefaultAsync(x => x.Id == id);

            if (oferta == null) return BadRequest("Taka oferta nie istnieje");

            _context.oferty.Remove(oferta);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
