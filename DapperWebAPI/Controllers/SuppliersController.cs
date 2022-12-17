using DapperWebAPI.Core.Entities;
using DapperWebAPI.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DapperWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SuppliersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Supplier>>> Get
            () => Ok(await _unitOfWork.Suppliers.GetAllWithProducts());

        [HttpGet("{id}")]
        public async Task<ActionResult<Supplier?>> Get(int id)
        {
            var result = await _unitOfWork.Suppliers.GetById(id);
            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Supplier entity)
        {
            var result = await _unitOfWork.Suppliers.Add(entity);
            if (!result)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPut()]
        public async Task<IActionResult> Put([FromBody] Supplier entity)
        {
            var result = await _unitOfWork.Suppliers.Update(entity);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _unitOfWork.Suppliers.Delete(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
