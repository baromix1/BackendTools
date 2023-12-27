using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using Microsoft.EntityFrameworkCore;

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
            OfertaDto temp = new OfertaDto
            {
                Id = o.Id,
                IdOsiedla = o.IdOsiedla,
                IdUzytkownika = o.IdOsiedla,
                Typ = o.Typ,
                Cena = o.Cena,
                Zdjecie = o.Zdjecie,
                DataDodaniaOferty = o.DataDodaniaOferty,
                DataDoKiedy = o.DataDoKiedy,
                DataOdKiedy = o.DataOdKiedy,
                Tytul = o.Tytul,
                Opis = o.Opis,
                CzyZakonczona = o.CzyZakonczona,
                Username = name
            };
            return temp;

        }

        public async Task<IReadOnlyList<OfertaDto>> GetOfertyAsync(int idWspolnoty)
        {
            var oferty = await _context.oferty.Where(p => p.IdOsiedla == idWspolnoty).ToListAsync();

            List<OfertaDto> lista = new List<OfertaDto>();

            foreach (var o in oferty)
            {
                string name = _context.uzytkownicy.Find(o.IdUzytkownika).username;
                OfertaDto temp = new OfertaDto
                {
                    Id = o.Id,
                    IdOsiedla = o.IdOsiedla,
                    IdUzytkownika = o.IdOsiedla,
                    Typ = o.Typ,
                    Cena = o.Cena,
                    Zdjecie = o.Zdjecie,
                    DataDodaniaOferty = o.DataDodaniaOferty,
                    DataDoKiedy = o.DataDoKiedy,
                    DataOdKiedy = o.DataOdKiedy,
                    Tytul = o.Tytul,
                    Opis = o.Opis,
                    CzyZakonczona = o.CzyZakonczona,
                    Username = name
                };
                lista.Add(temp);

            }
            return lista;
        }
        public async Task<IReadOnlyList<Oferta>> GetOfertyByWspolnotaAndUzytkownikAsync(int idWspolnoty, int idUzytkownika)
        {
            return await _context.oferty.Where(p => p.IdOsiedla == idWspolnoty && p.IdUzytkownika == idUzytkownika).ToListAsync();
        }
    }
}