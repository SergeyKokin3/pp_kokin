using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace pp_kokin.Controllers
{
    [Route("api/shops")]
    [ApiController]
    public class ShopsController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public ShopsController(IRepositoryManager repository, ILoggerManager
 logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetShops()
        {
            var shops = _repository.Shop.GetAllShops(trackChanges: false);
            var shopsDto = _mapper.Map<IEnumerable<ShopDto>>(shops);
            return Ok(shopsDto);
        }

        [HttpGet("{id}")]
        public IActionResult GetShop(Guid id)
        {
            var shop = _repository.Shop.GetShop(id, trackChanges: false);
            if (shop == null)
            {
                _logger.LogInfo($"Shop with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            else
            {
                var shopDto = _mapper.Map<ShopDto>(shop);
                return Ok(shopDto);
            }
        }
    }
}
