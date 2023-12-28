using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Data.Repositories;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UzytkownikController : BaseApiController
    {
        private readonly UzytkownikRepository _uzytkownicy;
        private readonly DataContext _context;


        public UzytkownikController(UzytkownikRepository uzytkownicy, DataContext context)
        {
            _uzytkownicy = uzytkownicy;
            _context = context;

        }

        [HttpPost("users")]
        public async Task<ActionResult<IEnumerable<Uzytkownik>>> GetUzytkownicy(idWspolontyDto idWspolontyDto)
        {
            return Ok(await _uzytkownicy.GetUzytkownicyAsync(idWspolontyDto.idWspolnoty));
        }

        [HttpGet("{idUzytkownika}")]
        public async Task<ActionResult<Uzytkownik>> GetUzytkownik(int idUzytkownika)
        {
            var uzytkownik = await _uzytkownicy.GetUzytkownikByIddAsync(idUzytkownika);

            if (uzytkownik == null)
            {
                return NotFound(); // Zwracaj NotFound, jeśli nie znaleziono wspólnoty o danym ID
            }

            return Ok(uzytkownik);
        }

        [HttpPost("login")]

        public async Task<ActionResult<LoggedUserDto>> GetLoggedUser(LoginDto loginDto)
        {
            return await _uzytkownicy.GetUzytkownikByUsernameAndPasswordAsync(loginDto.Username, loginDto.Password);
        }

        [HttpPost("register")]

        public async Task<bool> Register(RegisterDto registerDto)
        {
            if (await _uzytkownicy.UserExists(registerDto.username)) return false;

            var user = new Uzytkownik
            {
                username = registerDto.username,
                password = registerDto.password,
                typ = registerDto.typ
            };

            _context.uzytkownicy.Add(user);


            await _context.SaveChangesAsync();

            var uz = await _uzytkownicy.GetUzytkownikByUsernameAsync(registerDto.username);
            int idUz = uz.idUzytkownika;

            UzytkownikWspolnotaAsocjacja temp = new UzytkownikWspolnotaAsocjacja
            {
                idUzytkownika = idUz,
                idWspolnoty = registerDto.idWspolnoty
            };

            _context.uzytkownicyWspolnotyAsocjace.Add(temp);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}