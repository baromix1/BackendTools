namespace API.Entities
{
    public class Oferta : BaseEntity
    {
        public required string Typ { get; set; }
        public required int IdUzytkownika { get; set; }
        public byte[]? Zdjecie { get; set; }
        public required string Tytul { get; set; }
        public string? Opis { get; set; }
        public required int IdOsiedla { get; set; }
        public required string DataDodaniaOferty { get; set; }
        public string? DataOdKiedy { get; set; }
        public string? DataDoKiedy { get; set; }
        public float? Cena { get; set; }
        public required string CzyZakonczona { get; set; }
    }
}