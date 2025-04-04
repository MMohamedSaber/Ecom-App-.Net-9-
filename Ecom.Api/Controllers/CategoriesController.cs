using AutoMapper;
using Ecom.Api.helper;
using Ecom.Core.DTOs;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Api.Controllers
{

    public class CategoriesController : BaseController
    {
        public CategoriesController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> get()
        {
            try
            {
               var category=await _work.CategoryRepository.GetAllAsync();
                if (category is null) return BadRequest();
                return Ok(category);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> getbyId(int id)
        {
            try
            {
                var category= await _work.CategoryRepository.GetByIdAsync(id);
                if (category is null) return BadRequest(new ResponsApi(400,$"Not Found Category with {id}"));
                return Ok(category);
            }
            catch (Exception ex )
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> add (Categorydto categorydto)
        {
            try
            {
                var category = mapper.Map<Category>(categorydto);

                 await _work.CategoryRepository.AddAsync(category);
                return Ok( new ResponsApi(200,"Item has been added" ));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> update(updateCategorydto categorydto)
        {
            try
            {
                var category = mapper.Map<Category>(categorydto);

                await _work.CategoryRepository.UpdateAsync(category);

                return Ok(new ResponsApi(200, "Updated Successfully"));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete/{Id}")]
        public async Task<IActionResult> delete(int Id)
        {
            try
            {
                await _work.CategoryRepository.DeleteAsync(Id);
                return Ok(new ResponsApi(200, "Deleted Successfully"));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
