using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Authorization;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _dbContex;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public RestaurantService(RestaurantDbContext dbContex, IMapper mapper, ILogger<RestaurantService> logger
            , IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            _dbContex = dbContex;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }
        public RestaurantDto GetById(int id)
        {
            var restaurant = _dbContex
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == id);
            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");
            var result = _mapper.Map<RestaurantDto>(restaurant);
            return result;
        }
        public IEnumerable<RestaurantDto> GetAll(string searchPhrase)
        {
            var restaurants = _dbContex
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .Where(r => searchPhrase == null || (r.Name.ToLower().Contains(searchPhrase.ToLower())
                || r.Description.ToLower().Contains(searchPhrase.ToLower())))
                .ToList();
            var restaurantsDtos = _mapper.Map<List<RestaurantDto>>(restaurants);
            return restaurantsDtos;
        }
        public int Create(CreateRestaurantDto dto)
        {
            var restaurant = _mapper.Map<Restaurant>(dto);
            restaurant.CreatedById = _userContextService.GetUserId;
            _dbContex.Restaurants.Add(restaurant);
            _dbContex.SaveChanges();
            return restaurant.Id;
        }
        public void Delete(int id)
        {
            _logger.LogError($"Restaurant with id: {id} DELETE action invoked");
            var restaurant = _dbContex
                .Restaurants
                .FirstOrDefault(r => r.Id == id);
            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");
            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }
            _dbContex.Restaurants.Remove(restaurant);
            _dbContex.SaveChanges();
        }
        public void Update(int id, UpdateRestaurantDto dto)
        {

            var restaurant = _dbContex
                .Restaurants
                .FirstOrDefault(r => r.Id == id);
            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");
            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            restaurant.Name = dto.Name;
            restaurant.Description = dto.Description;
            restaurant.HasDelivery = dto.HasDelivery;
            _dbContex.SaveChanges();
        }
    }
}
