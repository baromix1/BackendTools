using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class RegisterDto
    {
        public required string username { get; set; }
        public required string password { get; set; }
        public required string typ { get; set; }
        public int idWspolnoty { get; set; }
    }
}