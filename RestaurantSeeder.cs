using RestaurantAPI.Entities;

namespace RestaurantAPI
{
    public class RestaurantSeeder
    {
        private readonly RestaurantDbContext _dbContext;
        public RestaurantSeeder(RestaurantDbContext dbContex)
        {
            _dbContext = dbContex;
        }
        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Restaurants.Any())
                {
                    var restaurants = GetRestaurants();
                    _dbContext.Restaurants.AddRange(restaurants);
                    _dbContext.SaveChanges();
                }
            }
        }
        private IEnumerable<Restaurant> GetRestaurants()
        {
            var restaurants = new List<Restaurant>
            {
                new Restaurant
                {
                    Name = "Pizza Place",
                    Description = "Best pizza in town",
                    Category = "Italian",
                    HasDelivery = true,
                    ContactEmail = "pizza@example.com",
                    ContactNumber = "000-000-000",
                    Dishes = new List<Dish>
                    {
                        new Dish
                        {
                            Name = "Margherita Pizza",
                            Description = "Classic pizza with fresh tomatoes, mozzarella, and basil",
                            Price = 8.99m

                        },
                        new Dish
                        {
                            Name = "Pepperoni Pizza",
                            Description = "Spicy pepperoni with mozzarella cheese",
                            Price = 9.99m

                        }
                    },
                    Address = new Address
                    {
                        City = "New York",
                        Street = "123 Pizza",
                        PostalCode = "10001"
                    }
                }
            };
            return restaurants;

        }
    }
}
