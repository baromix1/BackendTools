using API.Entities;

namespace API.Data.Repositories
{
    public class UzytkownikWspolnotaAsocjacjaRepository
    {

        private readonly DataContext _context;
        public UzytkownikWspolnotaAsocjacjaRepository(DataContext context)
        {
            _context = context;
        }

        // public async Task<bool> AddUserToWspolnotaDb(int _idUzytkownika, int _idWspolnoty)
        // {
        //     UzytkownikWspolnotaAsocjacja temp = new UzytkownikWspolnotaAsocjacja
        //     {
        //         idUzytkownika = _idUzytkownika,
        //         idWspolnoty = _idWspolnoty
        //     };

        //     _context.uzytkownicyWspolnotyAsocjace.Add(temp);
        //     await _context.SaveChangesAsync();
        //     return true;
        // }
    }
}