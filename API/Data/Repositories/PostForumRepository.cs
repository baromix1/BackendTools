using API.DTOs;
using API.Entities;
using Core.Specification;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace API.Data.Repositories
{
    public class PostForumRepository
    {
        private readonly DataContext _context;

        public PostForumRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<PostForumDto> GetPostByIdAsync(int id)
        {

#pragma warning disable CS8603 // Possible null reference return.
            var o = await _context.postyForum.SingleOrDefaultAsync(p => p.Id == id);
            string name = _context.uzytkownicy.Find(o.IdAutora).username;
#pragma warning restore CS8603 // Possible null reference return.
            List<KomentarzDoWyswietleniaDto> listaKomentarzy = new List<KomentarzDoWyswietleniaDto>();
            listaKomentarzy = (List<KomentarzDoWyswietleniaDto>)GetKomentarzePostaAsync(o.Id);
            PostForumDto temp = new PostForumDto
            {
                IdOsiedla = o.IdOsiedla,
                Username = name,
                DataDodania = o.DataDodania,
                Tytul = o.Tytul,
                Opis = o.Opis,
                listaKomentarzy = listaKomentarzy
            };
            return temp;
        }

        public async Task<IReadOnlyList<PostForumDto>> GetPostyAsync(int idWspolnoty)
        {
            var posty = await _context.postyForum.Where(p => p.IdOsiedla == idWspolnoty).ToListAsync();

            List<PostForumDto> lista = new List<PostForumDto>();

            foreach (var p in posty)
            {
                List<KomentarzDoWyswietleniaDto> listaKomentarzy = new List<KomentarzDoWyswietleniaDto>();
                listaKomentarzy = (List<KomentarzDoWyswietleniaDto>)GetKomentarzePostaAsync(p.Id);
                string name = _context.uzytkownicy.Find(p.IdAutora).username;
                PostForumDto temp = new PostForumDto
                {
                    IdOsiedla = p.IdOsiedla,
                    Username = name,
                    DataDodania = p.DataDodania,
                    Tytul = p.Tytul,
                    Opis = p.Opis,
                    listaKomentarzy = listaKomentarzy
                };
                lista.Add(temp);

            }
            return lista;
        }

        public IReadOnlyList<KomentarzDoWyswietleniaDto> GetKomentarzePostaAsync(int idPosta)
        {
            var komentarzePosta = _context.komentarzeForum.Where(p => p.IdPosta == idPosta).ToList();
            List<KomentarzDoWyswietleniaDto> listaKomentarzy = new List<KomentarzDoWyswietleniaDto>();
            foreach (var k in komentarzePosta)
            {
                var user = _context.uzytkownicy.Find(k.IdUzytkownika);
                KomentarzDoWyswietleniaDto temp = new KomentarzDoWyswietleniaDto
                {
                    idKomentarza = k.Id,
                    username = user.username,
                    Data = k.Data,
                    Tresc = k.Tresc
                };
                listaKomentarzy.Add(temp);

            }
            return listaKomentarzy;
        }

        public async Task<int> AddPost(AddPostForumDto post)
        {

            PostForum nowyPost = new PostForum
            {
                IdAutora = post.IdAutora,
                IdOsiedla = post.IdOsiedla,
                Tytul = post.Tytul,
                Opis = post.Opis,
                DataDodania = post.DataDodania
            };

            await _context.postyForum.AddAsync(nowyPost);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddKomentarzToPost(KomentarzForumDto komentarzForumDto)
        {
            KomentarzForum nowyKomentarz = new KomentarzForum
            {
                IdPosta = komentarzForumDto.IdPosta,
                IdUzytkownika = komentarzForumDto.IdUzytkownika,
                Tresc = komentarzForumDto.Tresc,
                Data = komentarzForumDto.Data
            };
            await _context.komentarzeForum.AddAsync(nowyKomentarz);

            return await _context.SaveChangesAsync();
        }

        private IQueryable<PostForum> ApplySpecification(ISpecification<PostForum> spec)
        {
            return SpecificationEvalator<PostForum>.GetQuery(_context.postyForum.AsQueryable(), spec);
        }

        public async Task<int> CountAsync(ISpecification<PostForum> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }
    }
}