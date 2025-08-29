using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
    public class CreatedMultipleRestaurantsRequirement : IAuthorizationRequirement
    {
        public int MinimumRestaurantsCreated { get; set; }
        public CreatedMultipleRestaurantsRequirement(int minimumRestaurantCreated)
        {
            MinimumRestaurantsCreated = minimumRestaurantCreated;
        }
    }
}
