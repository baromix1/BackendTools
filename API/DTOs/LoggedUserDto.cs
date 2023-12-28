using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.DTOs
{
    public class LoggedUserDto
    {
        public required string username { get; set; }
        public int id { get; set; }
        public required string typ { get; set; }
        public required List<Wspolnota> listaWspolnot { get; set; }
    }
}