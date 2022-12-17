using DapperWebAPI.Core.Entities;
using DapperWebAPI.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DapperWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> Get()
        {
            return Ok(await _unitOfWork.Categories.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category?>> Get(int id)
        {
            var result = await _unitOfWork.Categories.GetById(id);
            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("products/{id}")]
        public async Task<ActionResult<Category?>> GetWithProducts(int id)
        {
            var result = await _unitOfWork.Categories.GetByIdWithProducts(id);
            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Category entity)
        {
            var result = await _unitOfWork.Categories.Add(entity);
            if (!result)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Category entity)
        {
            var result = await _unitOfWork.Categories.Update(entity);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _unitOfWork.Categories.Delete(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
