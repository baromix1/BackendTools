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
        public async Task<IReadOnlyList<LoggedUserDto>> GetAllUzytkownicyByAsync(string idwspolnoty)
        {
            // var asocjacja = await _context.uzytkownicyWspolnotyAsocjace.Where(p=>p.idWspolnoty!=Int16.Parse(idwspolnoty)).ToListAsync();
            // List<userDto> users = new List<userDto>();
            // foreach(var a in asocjacja){
            //     var username = _context.uzytkownicy.SingleOrDefault(p=>p.Id==a.idUzytkownika);
            //     var temp = new userDto{
            //         idUzytkownika=a.idUzytkownika,
            //         username=username.username
            //     };
            //     users.Add(temp);
            // }
            // return  users;
            var users = await _context.uzytkownicy.ToListAsync();

            var associatedUsersIds = await _context.uzytkownicyWspolnotyAsocjace
                .Where(p => p.idWspolnoty == Int16.Parse(idwspolnoty))
                .Select(a => a.idUzytkownika)
                .ToListAsync();

            var freeUsers = users
                .Where(u => !associatedUsersIds.Contains(u.Id))
                .ToList();

            var list = new List<LoggedUserDto>();

            foreach (var f in freeUsers)
            {
                var asocjacja = await _context.uzytkownicyWspolnotyAsocjace
                .Where(p => p.idUzytkownika == f.Id)
                .ToListAsync();

                var tempList = new List<Wspolnota>();

                foreach (var a in asocjacja)
                {
                    var tempp = await _context.wspolnoty.SingleOrDefaultAsync(p => p.Id == a.idWspolnoty);
                    if (tempp != null)
                    {
                        tempList.Add(tempp);
                    }
                }

                var temp = new LoggedUserDto
                {
                    username = f.username,
                    typ = f.typ,
                    listaWspolnot = tempList
                };
            }

            return list;

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