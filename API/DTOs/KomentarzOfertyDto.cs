using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class KomentarzOfertyDto
    {
        public required DateTime Data { get; set; }
        public required int IdUzytkownika { get; set; }
        public required int IdOferty { get; set; }
        public required string Tresc { get; set; }
    }
}