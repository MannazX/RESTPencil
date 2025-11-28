using Microsoft.AspNetCore.Mvc;
using PencilLibrary;

namespace RESTPencil.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PencilController : Controller
	{
		private IPencilRepository _repo;

		public PencilController(IPencilRepository repo)
		{
			_repo = repo;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public ActionResult<IEnumerable<Pencil>> Get(
			[FromQuery] string? type, [FromQuery] string? brand, 
			[FromQuery] double? thickness, [FromQuery] double? length, 
			[FromQuery] double? price, string? sortBy)
		{
			IEnumerable<Pencil> result =_repo.Get(type, brand, thickness, length, price, sortBy);
			if (result.Count() == 0)
			{
				return NoContent();
			}
			else
			{
				return Ok(result);
			}
		}

		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<Pencil> Get(int id)
		{
			Pencil? result = _repo.GetById(id);
			if (result == null)
			{
				return NotFound("Ingen pencil med id " + id);
			}
			else
			{
				return Ok(result);
			}
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult<Pencil> Post([FromBody] Pencil value)
		{
			try
			{
				Pencil result = _repo.Add(value);
				string uri = Url.RouteUrl(RouteData.Values) + "/" + result.PencilId;
				return Created(uri, result);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult<Pencil> Put(int id, [FromBody] Pencil value)
		{
			try
			{
				Pencil result = _repo.Update(id, value);
				if (result == null)
				{
					return NotFound("Ingen aktiehandel med id " + id);
				}
				else
				{
					return Ok(result);
				}
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<Pencil> Delete(int id)
		{
			Pencil? result = _repo.Delete(id);
			if (result == null)
			{
				return NotFound("Ingen aktiehandel med id " + id);
			}
			else
			{
				return Ok(result);
			}
		}
	}
}
