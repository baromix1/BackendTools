using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UzytkownikController : BaseApiController
    {
        private readonly UzytkownikRepository _uzytkownicy;

        public UzytkownikController(UzytkownikRepository uzytkownicy)
        {
            _uzytkownicy = uzytkownicy;
        }

        [HttpPost("{idWspolnoty}")]
        public async Task<ActionResult<IEnumerable<Uzytkownik>>> GetUzytkownicy(string idWspolnoty)
        {
            return Ok(await _uzytkownicy.GetUzytkownicyAsync(idWspolnoty));
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
    }
}