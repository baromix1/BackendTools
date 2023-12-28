using API.Data.Repositories;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class HistoriaTransakcjiController
    {
        private readonly HistoriaTransakcjiRepository _historiaTransakcji;

        public HistoriaTransakcjiController(HistoriaTransakcjiRepository historiaTransakcji)
        {
            _historiaTransakcji = historiaTransakcji;
        }

        [HttpGet("{id}")]
        public async Task<IReadOnlyList<Oferta>> GetHistoria(int id)
        {
            return await _historiaTransakcji.GetZakonczoneOfertyAsync(id);
        }

        [HttpPut("dodaj-do-historii")]
        public async Task<bool> AddToHistory(AddingToHistoryDto addingToHistory)
        {
            if (!await _historiaTransakcji.AddToHistoryAsync(addingToHistory.idUzytkownika, addingToHistory.idOferty)) return false;

            return true;
        }
    }
}