namespace API.Entities
{
    public class KomentarzForum : BaseEntity
    {
        public required int IdUzytkownika { get; set; }
        public required int IdPosta { get; set; }
        public string Data { get; set; }
        public required string Tresc {get; set;}
    }
}