using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data.Repositories;
using API.DTOs;
using API.Entities;
using API.Helpers;
using Core.Specification;
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

        [HttpPost("all/{idWspolnoty}")]
        public async Task<ActionResult<Pagination<OfertaDto>>> GetOferty(int idWspolnoty,OfertySpecParams ofertyParams)
        {
            var spec= new ProductsWithTypersAndBrandSpecification(ofertyParams);
            var countSpec=new ProductWithFiltersForCountSpecification(ofertyParams);
            var totalItems=await _oferty.CountAsync(countSpec);
            var oferty=await _oferty.GetOfertyAsync(idWspolnoty,spec);
            return Ok(new Pagination<OfertaDto>(ofertyParams.PageIndex,ofertyParams.PageSize,totalItems-1,oferty));
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