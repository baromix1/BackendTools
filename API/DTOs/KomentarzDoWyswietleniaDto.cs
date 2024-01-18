using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class KomentarzDoWyswietleniaDto
    {
        public required string Data { get; set; }
        public required string Tresc { get; set; }
        public required string username { get; set; }
        public required int idKomentarza { get; set; }
    }
}