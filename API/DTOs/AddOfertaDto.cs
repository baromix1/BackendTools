using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class AddOfertaDto
    {
               public required string Typ { get; set; }
        public required int IdUzytkownika { get; set; }
        public string? Zdjecie { get; set; }
        public required string Tytul { get; set; }
        public string? Opis { get; set; }
        public required int IdOsiedla { get; set; }
        public required DateTime DataDodaniaOferty { get; set; }
        public DateTime? DataOdKiedy { get; set; }
        public DateTime? DataDoKiedy { get; set; }
        public float? Cena { get; set; }
        public required bool CzyZakonczona { get; set; }
    }
}