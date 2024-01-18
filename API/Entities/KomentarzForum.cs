namespace API.Entities
{
    public class KomentarzForum : BaseEntity
    {
        public required int IdUzytkownika { get; set; }
        public required int IdPosta { get; set; }
        public DateTime Data { get; set; }
        public required string Tresc {get; set;}
    }
}