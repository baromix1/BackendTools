using API.Data;
using API.Data.Repositories;
using API.DTOs;
using API.Entities;
using API.Helpers;
using Core.Specification;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class ForumController : BaseApiController
    {
        private readonly PostForumRepository _posty;
        private readonly DataContext _context;

        public ForumController(PostForumRepository posty, DataContext context)
        {
            _context = context;
            _posty = posty;
        }

        [HttpGet("{idPosta}")]
        public async Task<ActionResult<PostForumDto>> GetPost(int idPosta)
        {
            return Ok(await _posty.GetPostByIdAsync(idPosta));
        }

        [HttpPost("all/{idWspolnoty}")]
        public async Task<ActionResult<Pagination<PostForumDto>>> GetPosty(int idWspolnoty, OfertySpecParams postyParams)
        {
            var countSpec = new PostSpecification(postyParams);
            var totalItems = await _posty.CountAsync(countSpec);
            var posty = await _posty.GetPostyAsync(idWspolnoty);
            return Ok(new Pagination<PostForumDto>(postyParams.PageIndex, postyParams.PageSize, totalItems, posty));
        }

        [HttpPost("add/post")]
        public async Task<ActionResult<IEnumerable<PostForum>>> AddPost([FromForm] AddPostForumDto post)
        {
            var i = await _posty.AddPost(post);
            if (i > 0)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("add/post/komentarz")]
        public async Task<ActionResult<IEnumerable<PostForum>>> AddKomentarzToPost([FromForm] KomentarzForumDto komentarzForumDto)
        {
            var i = await _posty.AddKomentarzToPost(komentarzForumDto);
            if (i > 0)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpDelete("usun-post/{idPosta}")]

        public async Task<ActionResult> DeletePost(int idPosta)
        {
            var post = await _context.postyForum.FirstOrDefaultAsync(x => x.Id == idPosta);

            if (post == null) return BadRequest("Taki post nie istnieje");

            _context.postyForum.Remove(post);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
