using API.DTOs;
using API.Entities;
using Core.Specification;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace API.Data.Repositories
{
    public class OfertaRepository
    {
        private readonly DataContext _context;

        public OfertaRepository(DataContext context)
        {
            _context = context;
        }


        public async Task<OfertaDto> GetOfertaByIdAsync(int id)
        {

#pragma warning disable CS8603 // Possible null reference return.
            var o = await _context.oferty.SingleOrDefaultAsync(p => p.Id == id);
            string name = _context.uzytkownicy.Find(o.IdUzytkownika).username;
#pragma warning restore CS8603 // Possible null reference return.
            List<KomentarzDoWyswietleniaDto> listaKomentarzy = new List<KomentarzDoWyswietleniaDto>();
            listaKomentarzy = (List<KomentarzDoWyswietleniaDto>)GetKomentarzeOfertyAsync(o.Id);
            OfertaDto temp = new OfertaDto
            {
                Id = o.Id,
                IdOsiedla = o.IdOsiedla,
                IdUzytkownika = o.IdUzytkownika,
                Typ = o.Typ,
                Cena = o.Cena,
                Zdjecie = o.Zdjecie,
                DataDodaniaOferty = o.DataDodaniaOferty,
                DataDoKiedy = o.DataDoKiedy,
                DataOdKiedy = o.DataOdKiedy,
                Tytul = o.Tytul,
                Opis = o.Opis,
                CzyZakonczona = o.CzyZakonczona,
                Username = name,
                listaKomentarzy = listaKomentarzy
            };
            return temp;

        }

        public async Task<IReadOnlyList<OfertaDto>> GetOfertyAsync(int idWspolnoty, ISpecification<Oferta> spec)
        {
            var oferty = await ApplySpecification(spec).Where(p => p.CzyZakonczona == "false" && p.IdOsiedla == idWspolnoty).ToListAsync();

            List<OfertaDto> lista = new List<OfertaDto>();

            foreach (var o in oferty)
            {
                Console.WriteLine("oferta");
                List<KomentarzDoWyswietleniaDto> listaKomentarzy = new List<KomentarzDoWyswietleniaDto>();
                listaKomentarzy = (List<KomentarzDoWyswietleniaDto>)GetKomentarzeOfertyAsync(o.Id);
                string name = _context.uzytkownicy.Find(o.IdUzytkownika).username;
                OfertaDto temp = new OfertaDto
                {
                    Id = o.Id,
                    IdOsiedla = o.IdOsiedla,
                    IdUzytkownika = o.IdUzytkownika,
                    Typ = o.Typ,
                    Cena = o.Cena,
                    Zdjecie = o.Zdjecie,
                    DataDodaniaOferty = o.DataDodaniaOferty,
                    DataDoKiedy = o.DataDoKiedy,
                    DataOdKiedy = o.DataOdKiedy,
                    Tytul = o.Tytul,
                    Opis = o.Opis,
                    CzyZakonczona = o.CzyZakonczona,
                    Username = name,
                    listaKomentarzy = listaKomentarzy
                };
                lista.Add(temp);

            }
            return lista;
        }
        public async Task<IReadOnlyList<Oferta>> GetOfertyByWspolnotaAndUzytkownikAsync(int idWspolnoty, int idUzytkownika)
        {
            return await _context.oferty.Where(p => p.IdOsiedla == idWspolnoty && p.IdUzytkownika == idUzytkownika).ToListAsync();
        }

        private IQueryable<Oferta> ApplySpecification(ISpecification<Oferta> spec)
        {
            return SpecificationEvalator<Oferta>.GetQuery(_context.oferty.AsQueryable(), spec);
        }

        public async Task<int> CountAsync(ISpecification<Oferta> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }
        public IReadOnlyList<KomentarzDoWyswietleniaDto> GetKomentarzeOfertyAsync(int idOferty)
        {
            var komentarzeOferty = _context.komentarzeOferty.Where(p => p.IdOferty == idOferty).ToList();
            List<KomentarzDoWyswietleniaDto> listaKomentarzy = new List<KomentarzDoWyswietleniaDto>();
            foreach (var k in komentarzeOferty)
            {
                var user = _context.uzytkownicy.Find(k.IdUzytkownika);
                KomentarzDoWyswietleniaDto temp = new KomentarzDoWyswietleniaDto
                {
                    idKomentarza = k.Id,
                    username = user.username,
                    Data = k.Data,
                    Tresc = k.Tresc
                };
                listaKomentarzy.Add(temp);

            }
            return listaKomentarzy;
        }
        public async Task<int> AddKomentarzToOferta(KomentarzOfertyDto komentarzOfertyDto)
        {
            KomentarzOferty nowyKomentarz = new KomentarzOferty
            {
                IdOferty = komentarzOfertyDto.IdOferty,
                IdUzytkownika = komentarzOfertyDto.IdUzytkownika,
                Tresc = komentarzOfertyDto.Tresc,
                Data = komentarzOfertyDto.Data
            };
            await _context.komentarzeOferty.AddAsync(nowyKomentarz);

            return await _context.SaveChangesAsync();
        }
        public async Task<int> AddOferta(AddOfertaDto oferta)
        {
            using (var memoryStream = new MemoryStream())
            {
                oferta.imageFile?.CopyTo(memoryStream);
                byte[] imageBytes = memoryStream.ToArray();

                Oferta nowaOferta = new Oferta
                {
                    IdOsiedla = oferta.IdOsiedla,
                    IdUzytkownika = oferta.IdUzytkownika,
                    Typ = oferta.Typ,
                    Cena = oferta.Cena,
                    Zdjecie = imageBytes,
                    DataDodaniaOferty = oferta.DataDodaniaOferty,
                    DataDoKiedy = oferta.DataDoKiedy,
                    DataOdKiedy = oferta.DataOdKiedy,
                    Tytul = oferta.Tytul,
                    Opis = oferta.Opis,
                    CzyZakonczona = oferta.CzyZakonczona,
                };

                await _context.oferty.AddAsync(nowaOferta);
                return await _context.SaveChangesAsync();
            }
        }
        public async Task<int> ChangeToPending(int idOferty)
        {
            var oferta = _context.oferty.FirstOrDefault(p => p.Id == idOferty);
            oferta.CzyZakonczona = "pending";
            return await _context.SaveChangesAsync();
        }
        public async Task<int> ChangeToFalse(int idOferty)
        {
            var oferta = _context.oferty.FirstOrDefault(p => p.Id == idOferty);
            oferta.CzyZakonczona = "false";
            return await _context.SaveChangesAsync();
        }
    }
}