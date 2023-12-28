using API.Controllers;
using API.DTOs;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class HistoriaTransakcjiRepository : BaseApiController
    {
        private readonly DataContext _context;

        public HistoriaTransakcjiRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Oferta>> GetZakonczoneOfertyAsync(int idUzytkownika)
        {
            var historia = await _context.historieTransakcji.FirstOrDefaultAsync(x => x.Id == idUzytkownika);

            return await _context.oferty.Where(x => x.Id == historia.idOferty && x.CzyZakonczona == true).ToListAsync();
        }

        public async Task<bool> AddToHistoryAsync(int _idUzytkownika, int _idOferty)
        {
            var zakonczonaOferta = await _context.oferty.FirstOrDefaultAsync(x => x.Id == _idOferty && x.CzyZakonczona == false);

            if (zakonczonaOferta == null) return false;

            zakonczonaOferta.CzyZakonczona = true;

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