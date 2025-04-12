using AutoMapper;
using Ecom.Api.helper;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Api.Controllers
{

    public class BasketsController : BaseController
    {
        public BasketsController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("get-basket-item/{id}")]
        public async Task<IActionResult> get(string id)
        {
            var result = await _work.CustomerBasketRepository.GetBasketAsync(id);
        
            if(result is null)
            {
                return Ok(new CustomerBasket());
            }

            return Ok(result);

        }

        [HttpPost("update-basket")]
        public async Task<IActionResult> add(CustomerBasket basket)
        {
            var updatedBasket = await _work.CustomerBasketRepository.UpdateBasketAsync(basket);
            return Ok(updatedBasket);
        }

        [HttpDelete("delete-basket-item/{id}")]
        public async Task<IActionResult> delete(string id)
        {
           var result= await _work.CustomerBasketRepository.DeleteBasketAsync(id);
            return result ? Ok(new ResponsApi(200, "Deleted succeffuly")):
                BadRequest(new ResponsApi(400, "Deleted Failed"));
        }


    }
}
