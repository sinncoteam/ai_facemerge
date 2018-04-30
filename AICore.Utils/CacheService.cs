using Microsoft.Extensions.Caching.Memory;
using System;

namespace AICore.Utils
{
    public class CacheService
    {
        static MemoryCache cache = new MemoryCache(new MemoryCacheOptions());
        /// <summary>
        /// 获取缓存值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetCache(string key)
        {
            object val = null;
            if (key != null && cache.TryGetValue(key, out val))
            {
                return val;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 添加缓存内容
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireTime">秒</param>
        public static void SetChache(string key, object value, int expireTime)
        {
            if (key != null)
            {
                cache.Set(key, value, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromSeconds(expireTime)
                });
            }
        }
    }
}
