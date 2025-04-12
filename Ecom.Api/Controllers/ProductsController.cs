using AutoMapper;
using Ecom.Api.helper;
using Ecom.Core.DTOs;
using Ecom.Core.Interfaces;
using Ecom.Core.Sharing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Api.Controllers
{

    public class ProductsController : BaseController
    {
        public ProductsController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }



        [HttpGet("get-all")]
        public async Task<IActionResult> get([FromQuery] ProductParams productParams)
        {
            try
            {
                var product =  await _work.ProductRepository.GetAllAsync(productParams);
                var totalCount=await _work.ProductRepository.CountAsync();
                return Ok(new Pagenation<ProductDTO>(productParams.PageSize,
                    productParams.PageNumber,totalCount,product));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-by-id/{Id}")]
        public async Task<IActionResult> getById (int Id)
        {
            try
            {

                var product= await _work.ProductRepository.GetByIdAsync (Id,p=>p.Category, p =>p.Photos);
                if (product is null) return BadRequest( new ResponsApi( 400, "Not found Product"));

                var result=mapper.Map<ProductDTO>(product);

                return Ok(result);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);            }


        }


        [HttpPost("Add-Product")]
        public async Task<IActionResult> AddProduct(AddProductDto productDto)
        {

            try
            {
                await _work.ProductRepository.AddAsync(productDto);
                return Ok(new ResponsApi(200));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("Update-Product")]
        public async Task<IActionResult> Update(UpdateProductDto productdto)
        {
            try
            {
                 await _work.ProductRepository.UpdateAsync(productdto);
                return Ok(new ResponsApi(200, "Updated Successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete-Product/{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
              var product=  await _work.ProductRepository.GetByIdAsync(Id,p=>p.Photos,p =>p.Category);

                await _work.ProductRepository.DeleteAsync(product);
                return Ok(new ResponsApi(200, "Deleted Successfully"));


            }
            catch (Exception ex)
            {
                return BadRequest(new ResponsApi(400, ex.Message));
            }
        }

    }
}
