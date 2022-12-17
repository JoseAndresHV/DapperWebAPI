using DapperWebAPI.Core.Entities;
using DapperWebAPI.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DapperWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            return Ok(await _unitOfWork.Products.GetAllDetailed());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product?>> Get(int id)
        {
            var result = await _unitOfWork.Products.GetById(id);
            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }


        [HttpGet("detailed/{id}")]
        public async Task<ActionResult<Product?>> GetDetailed(int id)
        {
            var result = await _unitOfWork.Products.GetByIdDetailed(id);
            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product entity)
        {
            var result = await _unitOfWork.Products.Add(entity);
            if (!result)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Product entity)
        {
            var result = await _unitOfWork.Products.Update(entity);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _unitOfWork.Products.Delete(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
