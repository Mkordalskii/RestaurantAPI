using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;
using System.Security.Claims;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    [ApiController] // except if (!ModelState.IsValid) return BadRequest(ModelState);
    [Authorize]
    public class RestaurantController : ControllerBase
    {

        private readonly IRestaurantService restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            this.restaurantService = restaurantService;
        }
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            restaurantService.Delete(id, User);
            return NoContent();
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]

        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var id = restaurantService.Create(dto, userId);
            return Created($"/api/restaurant/{id}", null);
        }
        [HttpGet]
        [Authorize(Policy = "HasNationality")]
        public ActionResult<IEnumerable<RestaurantDto>> GetAll()
        {
            var restaurantDtos = restaurantService.GetAll();
            return Ok(restaurantDtos);
        }
        [HttpGet("{id}")]
        [Authorize(Policy = "Atleast20")]
        public ActionResult<RestaurantDto> Get([FromRoute] int id)
        {

            var restaurant = restaurantService.GetById(id);
            return Ok(restaurant);
        }
        [HttpPut("{id}")]
        [AllowAnonymous]
        public ActionResult Update([FromBody] UpdateRestaurantDto dto, [FromRoute] int id)
        {
            restaurantService.Update(id, dto, User);
            return Ok();
        }
    }
}
