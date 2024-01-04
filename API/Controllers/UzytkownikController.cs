using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Data.Repositories;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [HttpPost("all-users-without-id")]
        public async Task<ActionResult<IEnumerable<Uzytkownik>>> GetUzytkownicyAll(idWspolontyDto idWspolontyDto)
        {
            return Ok(await _uzytkownicy.GetAllUzytkownicyByAsync(idWspolontyDto.idWspolnoty));
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

        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            if (await _uzytkownicy.UserExists(registerDto.username)) return BadRequest("Taki uzytkownik juz istnieje");

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

            return Ok();
        }

        [HttpDelete("usun-uzytkownika/{id}")]

        public async Task<ActionResult> DeleteUser(int id)
        {
            var user = await _context.uzytkownicy.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null) return BadRequest("Taki uzytkownik nie istnieje");

            _context.uzytkownicy.Remove(user);
            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpDelete("usun-uzytkownika-z-wspolnoty")]
        public async Task<ActionResult> DeleteUserFromWspolnota(int idUzytkownika,int idWspolnoty)
        {
            var polaczenie = await _context.uzytkownicyWspolnotyAsocjace.FirstOrDefaultAsync(x => x.idUzytkownika == idUzytkownika && x.idWspolnoty==idWspolnoty);

            if (polaczenie == null) return BadRequest("Uzytkownik ten nie nalezy do tej wspolnoty");

            _context.uzytkownicyWspolnotyAsocjace.Remove(polaczenie);

            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpPut("dodaj-uzytkownika-do-wspolnoty")]
        public async Task<ActionResult> AddUserToWspolnotaDb(WspolnotaUzytkownikDto wspolnotaUzytkownik)
        {
            if (await _uzytkownicy.AddUserToWspolnotaDbAsyn(wspolnotaUzytkownik.idUzytkownika, wspolnotaUzytkownik.idWspolnoty)) return Ok();

            return BadRequest("Taki uzytkownik nie istnieje");
        }
    }
}