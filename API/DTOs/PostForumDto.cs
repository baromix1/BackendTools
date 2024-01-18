using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using API.Entities;

namespace API.DTOs
{
    public class PostForumDto
    {
        public required string Username { get; set; }
        public required string Tytul { get; set; }
        public string? Opis { get; set; }
        public required int IdOsiedla { get; set; }
        public required string DataDodania { get; set; }
        public List<KomentarzDoWyswietleniaDto>? listaKomentarzy { get; set; }

    }
}