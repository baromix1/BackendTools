using API.Controllers;
using API.DTOs;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class HistoriaTransakcjiRepository
    {
        private readonly DataContext _context;
        private readonly OfertaRepository _oferta;

        public HistoriaTransakcjiRepository(DataContext context, OfertaRepository oferta)
        {
            _oferta = oferta;
            _context = context;
        }

        public async Task<IReadOnlyList<OfertaDto>> GetZakonczoneOfertyAsync(int _idUzytkownika)
        {
            var historia = await _context.historieTransakcji.Where(x => x.idUzytkownika == _idUzytkownika).ToListAsync();
            var listaOfertaDto = new List<OfertaDto>();
            foreach (var i in historia)
            {
                listaOfertaDto.Add(await _oferta.GetOfertaByIdAsync(i.idOferty));
            }
            return listaOfertaDto;
        }

        public async Task<bool> AddToHistoryAsync(int _idUzytkownika, int _idOferty)
        {
            var zakonczonaOferta = await _context.oferty.FirstOrDefaultAsync(x => x.Id == _idOferty );

            if (zakonczonaOferta == null) return false;

            zakonczonaOferta.CzyZakonczona = "true";

            var historia = new HistoriaTransakcji
            {
                idUzytkownika = _idUzytkownika,
                idOferty = _idOferty
            };

            await _context.historieTransakcji.AddAsync(historia);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}