using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Konwersacja : BaseEntity
    {
        public required int idUzytkownik1 { get; set; }
        public required int idUzytkownik2 { get; set; }
        public required int idWspolnoty { get; set; }
    }
}