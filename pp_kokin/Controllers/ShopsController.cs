using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
        public async Task<IActionResult> GetShops()
        {
            var shops = _repository.Shop.GetAllShopsAsync(trackChanges: false);
            var shopsDto = _mapper.Map<IEnumerable<ShopDto>>(shops);
            return Ok(shopsDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetShop(Guid id)
        {
            var shop = await _repository.Shop.GetShopAsync(id, trackChanges: false);
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
        public async Task<IActionResult> CreateShop([FromBody] ShopForCreationDto shop)
        {
            if (shop == null)
            {
                _logger.LogError("ShopForCreationDto object sent from client is null.");
            return BadRequest("ShopForCreationDto object is null");
            }
            var shopEntity = _mapper.Map<Shop>(shop);
            _repository.Shop.CreateShop(shopEntity);
            await _repository.SaveAsync();
            var shopToReturn = _mapper.Map<ShopDto>(shopEntity);
            return CreatedAtRoute("ShopById", new { id = shopToReturn.Id },
            shopToReturn);
        }

        [HttpGet("collection/({ids})", Name = "ShopCollection")]
        public async Task<IActionResult> GetShopCollection([ModelBinder(BinderType =
typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }
            var shopEntities = await _repository.Shop.GetByIdsAsync(ids, trackChanges: false);
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
        public async Task<IActionResult> CreateShopCollection([FromBody]
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
            await _repository.SaveAsync();
            var shopCollectionToReturn =
            _mapper.Map<IEnumerable<ShopDto>>(shopEntities);
            var ids = string.Join(",", shopCollectionToReturn.Select(c => c.Id));
            return CreatedAtRoute("ShopCollection", new { ids },
            shopCollectionToReturn);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShop(Guid id)
        {
            var shop = await _repository.Shop.GetShopAsync(id, trackChanges: false);
            if (shop == null)
            {
                _logger.LogInfo($"Shop with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _repository.Shop.DeleteShop(shop);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShop(Guid id, [FromBody] ShopForUpdateDto
shop)
        {
            if (shop == null)
            {
            _logger.LogError("ShopForUpdateDto object sent from client is null.");
                return BadRequest("ShopForUpdateDto object is null");
            }
            var shopEntity = await _repository.Shop.GetShopAsync(id, trackChanges: true);
            if (shopEntity == null)
            {
                _logger.LogInfo($"Shop with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _mapper.Map(shop, shopEntity);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateShop(Guid id,
 [FromBody] JsonPatchDocument<ShopForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }
            var shopEntity = await _repository.Shop.GetShopAsync(id,
           trackChanges:
            true);
            if (shopEntity == null)
            {
                _logger.LogInfo($"Shop with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            var shopToPatch = _mapper.Map<ShopForUpdateDto>(shopEntity);
            patchDoc.ApplyTo(shopToPatch);

            _mapper.Map(shopToPatch, shopEntity);
            await _repository.SaveAsync();
            return NoContent();
        }


    }
}
