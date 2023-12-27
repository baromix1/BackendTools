using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class OfertaRepository
    {
         private readonly DataContext _context;

        public OfertaRepository(DataContext context){
            _context=context;
        }

        public async Task<Oferta> GetOfertaByIdAsync(int id){

#pragma warning disable CS8603 // Possible null reference return.
            return await _context.oferty.SingleOrDefaultAsync(p=>p.Id==id);
#pragma warning restore CS8603 // Possible null reference return.

        }

        public async Task<IReadOnlyList<Oferta>> GetOfertyAsync(int idWspolnoty){
            return await _context.oferty.Where(p=>p.IdOsiedla==idWspolnoty).ToListAsync();
        }
    }
}