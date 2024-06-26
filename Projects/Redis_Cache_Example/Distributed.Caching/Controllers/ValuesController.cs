﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace Distributed.Caching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        readonly IDistributedCache _distributedCache;

        public ValuesController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [HttpGet("Set/{name}/{surname}")]
        public async Task<IActionResult> Set(string name, string surname)
        {
            await _distributedCache.SetStringAsync("name", name); // string olarak set edilebilir.
            await _distributedCache.SetAsync("surname", Encoding.UTF8.GetBytes(surname)); // binary olarak set edebiliriz.
            return Ok();
        }

        [HttpGet("SetWithTime/{name}/{surname}")]
        public async Task<IActionResult> SetWithTime(string name, string surname)
        {
            await _distributedCache.SetStringAsync("name", name, options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(5)
            }); ; // string olarak set edilebilir.
            await _distributedCache.SetAsync("surname", Encoding.UTF8.GetBytes(surname),options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                SlidingExpiration =TimeSpan.FromSeconds(5)
            }); // binary olarak set edebiliriz.
            return Ok();
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var name = await _distributedCache.GetStringAsync("name");
            var surnameBinary = await _distributedCache.GetAsync("surname");
            var surname = Encoding.UTF8.GetString(surnameBinary);
            return Ok(new {name,surname});
        }



    }
}
