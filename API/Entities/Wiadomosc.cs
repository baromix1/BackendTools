namespace API.Entities
{
    public class Wiadomosc : BaseEntity
    {
        public required int idKonwersacji { get; set; }
        public required int idWysylajacego { get; set; }
        public required string trescWiadomosci { get; set; }
    }
}