namespace API.DTOs
{
    public class KomentarzForumDto
    {
        public required DateTime Data { get; set; }
        public required int IdUzytkownika { get; set; }
        public required int IdPosta { get; set; }
        public required string Tresc { get; set; }
    }
}