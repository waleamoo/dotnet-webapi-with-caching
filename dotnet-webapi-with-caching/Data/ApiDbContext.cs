using System;
using dotnet_webapi_with_caching.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet_webapi_with_caching.Data
{

	public class ApiDbContext : DbContext
	{
        public DbSet<Driver> Drivers { get; set; }

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
		{
		
		}
	}
}

