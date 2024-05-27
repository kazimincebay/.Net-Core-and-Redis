using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemory.Caching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        
        public ValuesController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        [HttpGet("set/{name}")] //swagger parametreyi alacaktır fakat postman yada başka bir yolla denenecekse routing parametresi olarak kalmalıdır.
        public void Set(string name)
        {
            _memoryCache.Set("name", name); // set fonksiyonu ile key value olarak değer atayabiliriz.


        }


        [HttpGet("setdate")] 
        public void SetDate()
        {
            _memoryCache.Set<DateTime>("Date", DateTime.Now, options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(5)
            });
            // absolute time cachein tutulacağı max süredeir
            // sliding time da bu süre içerisinde cache erişim yenilenirse sliding time absolute time dahiilinde tekrardan başlar.
            // iki zaman türü de tanımlanabilir.

        }

        [HttpGet("getdate")]
        public DateTime GetDate()
        {
            return _memoryCache.Get<DateTime>("Date");

        }

        [HttpGet]
        public string Get()
        {
            //_memoryCache.Get("name"); bu şekilde elde edip stringede dönüştürülebilir
            //generic yapılanma ile türü elde edebiliriz
            return _memoryCache.Get<string>("name");
        }

        [HttpGet("trygetvalue")]
        public string TryGetValue()
        {

            // cachlenen veri olmadığından substring gibi veri üzerine etki edecek olan fonksiyonlar hata verecektir
            // bu nedenle trygetvalue fonksiyonu kullanılır

            if (_memoryCache.TryGetValue<string>("name", out string name))
            {
                return name.Substring(3);
            }
            return "";
        }

        [HttpDelete]
        public void Delete()
        {
            _memoryCache.Remove("name"); // remove fonksiyonu ile cachelenmiş veri silinebilir.
        }
    }
}
