using API.Data.Repositories;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class HistoriaTransakcjiController : BaseApiController
    {
        private readonly HistoriaTransakcjiRepository _historiaTransakcji;

        public HistoriaTransakcjiController(HistoriaTransakcjiRepository historiaTransakcji)
        {
            _historiaTransakcji = historiaTransakcji;
        }

        [HttpGet("{idUzytkownika}")]
        public async Task<IReadOnlyList<OfertaDto>> GetHistoria(int idUzytkownika)
        {
            return await _historiaTransakcji.GetZakonczoneOfertyAsync(idUzytkownika);
        }

        [HttpPut("dodaj-do-historii")]
        public async Task<bool> AddToHistory(AddingToHistoryDto addingToHistory)
        {
            if (!await _historiaTransakcji.AddToHistoryAsync(addingToHistory.idUzytkownika, addingToHistory.idOferty)) return false;

            return true;
        }
    }
}