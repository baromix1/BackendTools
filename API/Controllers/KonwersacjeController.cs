using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data.Repositories;
using API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class KonwersacjeController : BaseApiController
    {
        private readonly KonwersacjaRepository _konwersacjaRepository;

        public KonwersacjeController(KonwersacjaRepository konwersacjaRepository)
        {
            _konwersacjaRepository = konwersacjaRepository;
        }

        [HttpPost]
        public async Task<ActionResult> GetWiadomosci(KonwersacjaDto konwersacja)
        {
            var listaWiadomosci = await _konwersacjaRepository.GetKonwersacjaAsync(konwersacja.idWysylajacego, konwersacja.idOdbierajacego, konwersacja.idWspolnoty);

            if (listaWiadomosci == null) return BadRequest("Nie znaleziono takiej konwersacji");

            return Ok(listaWiadomosci);
        }

        [HttpPost("wyslij-wiadomosc")]
        public async Task<ActionResult> SendWiadomosc(WiadomoscDto wiadomosc)
        {
            if (await _konwersacjaRepository.AddWiadomosc(wiadomosc.idWysylajacego, wiadomosc.idOdbierajacego, wiadomosc.idWspolnoty, wiadomosc.trescWiadomosci))
            {
                return Ok();
            }
            return BadRequest("Nie udalo sie dodac do bazy danych, najprawdopobodniej jeden z podanych uzytkownikow nie istnieje");
        }
    }
}