namespace API.Entities
{
    public class PostForum : BaseEntity
    {
        public required int IdAutora { get; set; }
        public required string Tytul { get; set; }
        public string? Opis { get; set; }
        public required string DataDodania { get; set; }
        public required int IdOsiedla { get; set; }
    }
}