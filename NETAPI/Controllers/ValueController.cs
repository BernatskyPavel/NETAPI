using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NETAPI.Models;

namespace NETAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValueController : ControllerBase
    {
        private readonly IMemoryCache _cache;

        public ValueController(IMemoryCache cache)
        {
            _cache = cache;
        }

        // GET: api/Value
        [HttpGet]
        public Cache Get()
        {
            return _cache.Get<Cache>(Properties.Resources.CacheString);
        }

    }
}
