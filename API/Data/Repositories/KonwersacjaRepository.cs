using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class KonwersacjaRepository
    {
        private readonly DataContext _context;


        public KonwersacjaRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Wiadomosc>> GetKonwersacjaAsync(int idWysylajacego, int idOdbierajacego, int iddWspolnoty)
        {
            var konwersacja = await _context.konwersacje.SingleOrDefaultAsync(x => (x.idUzytkownik1 == idWysylajacego
            && x.idUzytkownik2 == idOdbierajacego
            && x.idWspolnoty == iddWspolnoty) || (x.idUzytkownik2 == idWysylajacego
            && x.idUzytkownik1 == idOdbierajacego
            && x.idWspolnoty == iddWspolnoty));

            if (konwersacja == null) return null;

            return await _context.wiadomosci.Where(x => x.idKonwersacji == konwersacja.Id).ToListAsync();
        }

        public async Task<bool> AddWiadomosc(int _idWysylajacego, int _idOdbierajacego, int _idWspolnoty, string tresc)
        {
            if (!await _context.uzytkownicy.AnyAsync(x => x.Id == _idWysylajacego || x.Id == _idOdbierajacego))
            {
                return false;
            }

            var konwersacja = await _context.konwersacje.SingleOrDefaultAsync(x => (x.idUzytkownik1 == _idWysylajacego
            && x.idUzytkownik2 == _idOdbierajacego
            && x.idWspolnoty == _idWspolnoty) || (x.idUzytkownik2 == _idWysylajacego
            && x.idUzytkownik1 == _idOdbierajacego
            && x.idWspolnoty == _idWspolnoty));

            if (konwersacja == null)
            {
                Konwersacja nowaKonwersacja = new Konwersacja
                {
                    idWspolnoty = _idWspolnoty,
                    idUzytkownik1 = _idWysylajacego,
                    idUzytkownik2 = _idOdbierajacego
                };

                await _context.konwersacje.AddAsync(nowaKonwersacja);
                await _context.SaveChangesAsync();

                var konwersacjaa = await _context.konwersacje.SingleOrDefaultAsync(x => (x.idUzytkownik1 == _idWysylajacego
            && x.idUzytkownik2 == _idOdbierajacego
            && x.idWspolnoty == _idWspolnoty) || (x.idUzytkownik2 == _idWysylajacego
            && x.idUzytkownik1 == _idOdbierajacego
            && x.idWspolnoty == _idWspolnoty));

                Wiadomosc wiadomosc = await createWiadomosc(konwersacjaa.Id, _idWysylajacego, tresc);

                await _context.wiadomosci.AddAsync(wiadomosc);
                await _context.SaveChangesAsync();

                return true;
            }
            else
            {
                Wiadomosc wiadomosc = await createWiadomosc(konwersacja.Id, _idWysylajacego, tresc);

                await _context.wiadomosci.AddAsync(wiadomosc);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<Wiadomosc> createWiadomosc(int idK, int idWys, string tresc)
        {
            return new Wiadomosc
            {
                idKonwersacji = idK,
                idWysylajacego = idWys,
                trescWiadomosci = tresc
            };
        }
    }
}