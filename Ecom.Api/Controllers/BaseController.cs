using AutoMapper;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {

        public IUnitOfWork _work { get; }
        protected readonly IMapper mapper;
        public BaseController(IUnitOfWork work, IMapper mapper)
        {
            _work = work;
            this.mapper = mapper;
        }

    }
}
