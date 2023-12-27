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
        private readonly UzytkownikWspolnotaAsocjacjaRepository _uzytkownikWspolnotaAsocjacja;

        public UzytkownikController(UzytkownikRepository uzytkownicy, UzytkownikWspolnotaAsocjacjaRepository uzytkownikWspolnotaAsocjacja)
        {
            _uzytkownicy = uzytkownicy;
            _uzytkownikWspolnotaAsocjacja = uzytkownikWspolnotaAsocjacja;
        }

        [HttpPost("users")]
        public async Task<ActionResult<IEnumerable<Uzytkownik>>> GetUzytkownicy(idWspolontyDto idWspolontyDto)
        {
            return Ok(await _uzytkownicy.GetUzytkownicyAsync(idWspolontyDto.idWspolnoty));
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<Uzytkownik>> GetUzytkownik(string username)
        {
            var uzytkownik = await _uzytkownicy.GetUzytkownikByUsernameAsync(username);

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

            await _uzytkownicy.AddUserToDb(user);

            var idUz = _uzytkownicy.GetUzytkownikByUsernameAsync(registerDto.username).Id;

            await _uzytkownicy.AddUserToWspolnotaDb(idUz, registerDto.idWspolnoty);
            return true;
        }
    }
}