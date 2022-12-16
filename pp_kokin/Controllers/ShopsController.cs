using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pp_kokin.ModelBinders;

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

        [HttpPost]
        public IActionResult CreateShop([FromBody] ShopForCreationDto shop)
        {
            if (shop == null)
            {
                _logger.LogError("ShopForCreationDto object sent from client is null.");
            return BadRequest("ShopForCreationDto object is null");
            }
            var shopEntity = _mapper.Map<Shop>(shop);
            _repository.Shop.CreateShop(shopEntity);
            _repository.Save();
            var shopToReturn = _mapper.Map<ShopDto>(shopEntity);
            return CreatedAtRoute("ShopById", new { id = shopToReturn.Id },
            shopToReturn);
        }

        [HttpGet("collection/({ids})", Name = "ShopCollection")]
        public IActionResult GetShopCollection([ModelBinder(BinderType =
typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }
            var shopEntities = _repository.Shop.GetByIds(ids, trackChanges: false);
 if (ids.Count() != shopEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }
            var shopsToReturn =
           _mapper.Map<IEnumerable<ShopDto>>(shopEntities);
            return Ok(shopsToReturn);
        }

        [HttpPost("collection")]
        public IActionResult CreateShopCollection([FromBody]
IEnumerable<ShopForCreationDto> shopCollection)
        {
            if (shopCollection == null)
            {
                _logger.LogError("Shop collection sent from client is null.");
                return BadRequest("Shop collection is null");
            }
            var shopEntities = _mapper.Map<IEnumerable<Shop>>(shopCollection);
            foreach (var shop in shopEntities)
            {
                _repository.Shop.CreateShop(shop);
            }
            _repository.Save();
            var shopCollectionToReturn =
            _mapper.Map<IEnumerable<ShopDto>>(shopEntities);
            var ids = string.Join(",", shopCollectionToReturn.Select(c => c.Id));
            return CreatedAtRoute("ShopCollection", new { ids },
            shopCollectionToReturn);
        }

    }
}
