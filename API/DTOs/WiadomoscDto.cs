using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class WiadomoscDto
    {
        public int idWysylajacego { get; set; }
        public int idOdbierajacego { get; set; }
        public int idWspolnoty { get; set; }
        public string trescWiadomosci { get; set; }
    }
}