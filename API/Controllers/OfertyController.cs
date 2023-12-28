using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data.Repositories;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class OfertyController : BaseApiController
    {
        private readonly OfertaRepository _oferty;

        public OfertyController(OfertaRepository oferty)
        {
            _oferty = oferty;
        }

        [HttpGet("all/{idWspolnoty}")]
        public async Task<ActionResult<IEnumerable<Oferta>>> GetOferty(int idWspolnoty)
        {
            return Ok(await _oferty.GetOfertyAsync(idWspolnoty));
        }
        [HttpGet("{idOferty}")]
        public async Task<ActionResult<OfertaDto>> GetOferta(int idOferty)
        {
            return Ok(await _oferty.GetOfertaByIdAsync(idOferty));
        }
        [HttpPost("all/user")]
        public async Task<ActionResult<IEnumerable<Oferta>>> GetOfertyByUserAndWspolnota(WspolnotaUzytkownikDto wspolnotaUzytkownikDto)
        {
            return Ok(await _oferty.GetOfertyByWspolnotaAndUzytkownikAsync(wspolnotaUzytkownikDto.idWspolnoty,wspolnotaUzytkownikDto.idUzytkownika));
        }

  
    }
}