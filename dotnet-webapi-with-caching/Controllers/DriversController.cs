using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_webapi_with_caching.Data;
using dotnet_webapi_with_caching.Models;
using dotnet_webapi_with_caching.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace dotnet_webapi_with_caching.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriversController : Controller
    {
        // fields
        private readonly ICachingService _cacheService;
        private readonly ApiDbContext _context;

        public DriversController(ICachingService cacheService, ApiDbContext context)
        {
            _cacheService = cacheService;
            _context = context;
        }

        // GET: api/values
        [HttpGet("drivers")]
        public async Task<IActionResult> Get()
        {
            // try to get the drivers from the cache if they exist 
            var cacheDrivers = _cacheService.GetData<IEnumerable<Driver>>("drivers");
            if (cacheDrivers != null && cacheDrivers.Count() > 0)
            {
                return Ok(cacheDrivers);
            }
            // otherwise get the data from the database
            var drivers = await _context.Drivers.ToListAsync();
            // add the database list of drivers to the cache directory
            var expiryTime = DateTimeOffset.Now.AddMinutes(2);
            _cacheService.SetData<IEnumerable<Driver>>("drivers", drivers, expiryTime);
            // return the drivers 
            return Ok(drivers);
        }

        // GET api/values/5
        [HttpGet("drivers/{id}")]
        public async Task<IActionResult> GetDriver(int id)
        {
            // try to get the drivers from the cache if they exist 
            var cacheDrivers = _cacheService.GetData<IEnumerable<Driver>>("drivers");
            if (cacheDrivers != null && cacheDrivers.Count() > 0)
            {
                var cacheDriver = cacheDrivers.FirstOrDefault(x => x.Id == id);
                if (cacheDriver != null)
                {
                    return Ok(cacheDriver);
                }
            }
            else
            {
                var dbDriver = _context.Drivers.FirstOrDefault(x => x.Id == id);
                if (dbDriver != null)
                {
                    return Ok(dbDriver);
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Driver driver)
        {
            await _context.Drivers.AddAsync(driver);
            await _context.SaveChangesAsync();
            return Ok(driver);
        }

    }
}

