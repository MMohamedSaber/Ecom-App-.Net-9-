using AutoMapper;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Api.Controllers
{
    public class BugsController : BaseController
    {
        public BugsController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("not-found")]
        public async Task<IActionResult> GetNotFound()
        {
            var category = await _work.CategoryRepository.GetByIdAsync(100);
            if (category == null)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpGet("server-Error")]
        public async Task<IActionResult> GetServerError()
        {
            var category = await _work.CategoryRepository.GetByIdAsync(100);
            category.Name = "";
            return Ok(category);
        } 
        
        [HttpGet("bad-request/{Id}")]
        public async Task<IActionResult> GetBadRequest(int Id)
        {
            return Ok();
        }

        [HttpGet("bad-request")]
        public async Task<IActionResult> GetBadRequest()
        {
            return BadRequest();
        }


    }
}
