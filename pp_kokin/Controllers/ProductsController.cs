using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace pp_kokin.Controllers
{
    [Route("api/shops/{shopId}/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public ProductsController(IRepositoryManager repository, ILoggerManager
       logger,
        IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsForShop(Guid shopId)
        {
            var shop = await _repository.Shop.GetShopAsync(shopId, trackChanges: false);
            if (shop == null)
            {
                _logger.LogInfo($"Shop with id: {shopId} doesn't exist in the database.");
            return NotFound();
            }
            var productsFromDb = await _repository.Product.GetProductsAsync(shopId,
            trackChanges: false);
            var productsDto = _mapper.Map<IEnumerable<ProductDto>>(productsFromDb);
            return Ok(productsDto);
        }

        [HttpGet("{id}", Name = "GetProductForShop")]
        public async Task<IActionResult> GetProductForShop(Guid shopId, Guid id)
        {
            var shop = await _repository.Shop.GetShopAsync(shopId, trackChanges: false);
            if (shop == null)
            {
                _logger.LogInfo($"Shop with id: {shopId} doesn't exist in the database.");
            return NotFound();
            }
            var productDb = await _repository.Product.GetProductAsync(shopId, id,
           trackChanges:
            false);
            if (productDb == null)
            {
                _logger.LogInfo($"Product with id: {id} doesn't exist in the database.");
            return NotFound();
            }
            var product = _mapper.Map<ProductDto>(productDb);
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductForShop(Guid shopId, [FromBody]
ProductForCreationDto product)
        {
            if (product == null)
            {
                _logger.LogError("ProductForCreationDto object sent from client is null.");
            return BadRequest("ProductForCreationDto object is null");
            }
            var shop = await _repository.Shop.GetShopAsync(shopId, trackChanges: false);
            if (shop == null)
            {
                _logger.LogInfo($"Shop with id: {shopId} doesn't exist in the database.");
            return NotFound();
            }
            var productEntity = _mapper.Map<Product>(product);
            _repository.Product.CreateProductForShop(shopId, productEntity);
            await _repository.SaveAsync();
            var productToReturn = _mapper.Map<ProductDto>(productEntity);
            return CreatedAtRoute("GetProductForShop", new
            {
                shopId,
                id = productToReturn.Id
            }, productToReturn);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductForShop(Guid shopId, Guid id)
        {
            var shop = await _repository.Shop.GetShopAsync(shopId, trackChanges: false);
            if (shop == null)
            {
                _logger.LogInfo($"Shop with id: {shopId} doesn't exist in the database.");
            return NotFound();
            }
            var productForShop = await _repository.Product.GetProductAsync(shopId, id,
            trackChanges: false);
            if (productForShop == null)
            {
                _logger.LogInfo($"Product with id: {id} doesn't exist in the database.");
            return NotFound();
            }
            _repository.Product.DeleteProduct(productForShop);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductForShop(Guid shopId, Guid id, [FromBody]
ProductForUpdateDto product)
        {
            if (product == null)
            {
                _logger.LogError("ProductForUpdateDto object sent from client is null.");
            return BadRequest("ProductForUpdateDto object is null");
            }
            var shop = await _repository.Shop.GetShopAsync(shopId, trackChanges: false);
            if (shop == null)
            {
                _logger.LogInfo($"Shop with id: {shopId} doesn't exist in the database.");
            return NotFound();
            }
            var productEntity = await _repository.Product.GetProductAsync(shopId, id,
           trackChanges:
            true);
            if (productEntity == null)
            {
                _logger.LogInfo($"Product with id: {id} doesn't exist in the database.");
            return NotFound();
            }
            _mapper.Map(product, productEntity);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateProductForShop(Guid shopId, Guid id,
 [FromBody] JsonPatchDocument<ProductForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }
            var shop = await _repository.Shop.GetShopAsync(shopId, trackChanges: false);
            if (shop == null)
            {
                _logger.LogInfo($"Shop with id: {shopId} doesn't exist in the database.");
            return NotFound();
            }
            var productEntity = await _repository.Product.GetProductAsync(shopId, id,
           trackChanges:
            true);
            if (productEntity == null)
            {
                _logger.LogInfo($"Product with id: {id} doesn't exist in the database.");
            return NotFound();
            }
            var productToPatch = _mapper.Map<ProductForUpdateDto>(productEntity);
            patchDoc.ApplyTo(productToPatch);
 _mapper.Map(productToPatch, productEntity);
            await _repository.SaveAsync();
            return NoContent();
        }

    }
}
