using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UzytkownikRepository
    {
        private readonly DataContext _context;

        public UzytkownikRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<userDto> GetUzytkownikByUsernameAsync(string username)
        {
#pragma warning disable CS8603 // Possible null reference return.
            var user = await _context.uzytkownicy.SingleOrDefaultAsync(p => p.username == username);
#pragma warning restore CS8603 // Possible null reference return.
            return new userDto
            {
                idUzytkownika = user.Id,
                username = user.username
            };
        }
        public async Task<userDto> GetUzytkownikByIddAsync(int idUzytkownika)
        {


            var user = _context.uzytkownicy.Find(idUzytkownika);

            return new userDto
            {
                idUzytkownika = user.Id,
                username = user.username
            };

        }

        public async Task<IReadOnlyList<userDto>> GetUzytkownicyAsync(string idWspolnoty)
        {

            var asocjacja = await _context.uzytkownicyWspolnotyAsocjace
            .Where(p => p.idWspolnoty == Int16.Parse(idWspolnoty))
            .ToListAsync();

            List<Uzytkownik> users = new List<Uzytkownik>();

            foreach (var a in asocjacja)
            {
                var temp = await _context.uzytkownicy.SingleOrDefaultAsync(p => p.Id == a.idUzytkownika);
                users.Add(temp);
            }
            List<userDto> lista = new List<userDto>();

            foreach (var u in users)
            {
                userDto temp = new()
                {
                    idUzytkownika = u.Id,
                    username = u.username
                };
                lista.Add(temp);
            }
            return lista;
        }

        public async Task<LoggedUserDto> GetUzytkownikByUsernameAndPasswordAsync(string username, string password)
        {


            var uzytkownik = await _context.uzytkownicy.SingleOrDefaultAsync(p => p.username == username && p.password == password);


            if (uzytkownik == null)
            {
                return null;
            }
            var asocjacja = await _context.uzytkownicyWspolnotyAsocjace
            .Where(p => p.idUzytkownika == uzytkownik.Id)
            .ToListAsync();

            List<Wspolnota> lista = new List<Wspolnota>();
            foreach (var a in asocjacja)
            {
                var temp = await _context.wspolnoty.SingleOrDefaultAsync(p => p.Id == a.idWspolnoty);
                if (temp != null)
                {
                    lista.Add(temp);
                }
            }
            return new LoggedUserDto
            {
                id = uzytkownik.Id,
                username = uzytkownik.username,
                typ = uzytkownik.typ,
                listaWspolnot = lista
            };

        }
        public async Task<bool> UserExists(string username)
        {
            return await _context.uzytkownicy.AnyAsync(x => x.username.ToLower() == username.ToLower());
        }

        public async Task<int> AddUserToDb(Uzytkownik uzytkownik)
        {
            _context.uzytkownicy.Add(uzytkownik);

            return await _context.SaveChangesAsync();
        }

        public async Task<bool> AddUserToWspolnotaDbAsyn(int _idUzytkownika, int _idWspolnoty)
        {
            if (!await _context.uzytkownicy.AnyAsync(x => x.Id == _idUzytkownika)) return false;

            UzytkownikWspolnotaAsocjacja temp = new UzytkownikWspolnotaAsocjacja
            {
                idUzytkownika = _idUzytkownika,
                idWspolnoty = _idWspolnoty
            };

            await _context.uzytkownicyWspolnotyAsocjace.AddAsync(temp);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetUserIdByUsernameAsync(string username)
        {
            var user = await _context.uzytkownicy.SingleOrDefaultAsync(p => p.username == username);
            return user?.Id ?? 0;
        }
    }
}