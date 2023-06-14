using System;
namespace dotnet_webapi_with_caching.Services
{
	public interface ICachingService
	{
		T GetData<T>(string key);
		bool SetData<T>(string key, T value, DateTimeOffset expirationTime);
		object RemoveData(string key);
	}
}

