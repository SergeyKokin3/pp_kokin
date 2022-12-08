using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Http;
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
        public IActionResult GetProductsForShop(Guid shopId)
        {
            var shop = _repository.Shop.GetShop(shopId, trackChanges: false);
            if (shop == null)
            {
                _logger.LogInfo($"Shop with id: {shopId} doesn't exist in the database.");
            return NotFound();
            }
            var productsFromDb = _repository.Product.GetProducts(shopId,
            trackChanges: false);
            var productsDto = _mapper.Map<IEnumerable<ProductDto>>(productsFromDb);
            return Ok(productsDto);
        }

        [HttpGet("{id}")]
        public IActionResult GetProductForShop(Guid shopId, Guid id)
        {
            var shop = _repository.Shop.GetShop(shopId, trackChanges: false);
            if (shop == null)
            {
                _logger.LogInfo($"Shop with id: {shopId} doesn't exist in the database.");
            return NotFound();
            }
            var productDb = _repository.Product.GetProduct(shopId, id,
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
    }
}
