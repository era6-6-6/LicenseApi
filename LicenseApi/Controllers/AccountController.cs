using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace LicenseApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        [HttpPost(Name = "PostData")]
        public bool AccountData(string json)
        {
            try
            {
                var j1 = JToken.Parse(json);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            
        }

    }
}
