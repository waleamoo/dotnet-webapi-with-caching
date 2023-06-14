using System;
using System.Runtime.Caching;

namespace dotnet_webapi_with_caching.Services
{
	public class CachingService : ICachingService
	{
        // create the cache field 
        private ObjectCache _memoryCache = MemoryCache.Default;

		public CachingService()
		{
		}

        public T GetData<T>(string key)
        {
            try
            {
                T item = (T)_memoryCache.Get(key);
                return item;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public object RemoveData(string key)
        {
            var res = true;
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    var result = _memoryCache.Remove(key);
                }
                else
                    res = false;
                return res;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var res = true;
            try
            {
                if (!string.IsNullOrEmpty(key))
                    _memoryCache.Set(key, value, expirationTime);
                else
                    res = false;
                return res;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

