using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using API.Entities;

namespace API.DTOs
{
    public class AddPostForumDto
    {
        public required int IdAutora { get; set; }
        public required string Tytul { get; set; }
        public string? Opis { get; set; }
        public required DateTime DataDodania { get; set; }
        public required int IdOsiedla { get; set; }

    }
}