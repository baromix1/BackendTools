

namespace API.Entities
{
    public class KomentarzOferty : BaseEntity
    {
        public required DateTime Data { get; set; }
        public required int IdUzytkownika { get; set; }
        public required int IdOferty { get; set; }
        public required string Tresc { get; set; }
    }
}