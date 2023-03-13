using System.Diagnostics;
using LicenseApi.Managers;
using Microsoft.AspNetCore.Mvc;

namespace LicenseApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LicenseController : ControllerBase
    {
        [HttpGet(Name = "GetLicense")]
        public string License(int id)
        {
            return BuildResponse(LicenseManager.ValidateLicense(id));
        }


        private string BuildResponse(bool request)
        {
            Stopwatch s = new Stopwatch();
            s.Start();
            string response = "";
            string a = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
            int j = 0;
            for (int i = 0; i < 1_000; i++)
            {
                if (i != 0 &&i % 10 == 0 && j <= request.ToString().Length - 1)
                {
                    response += request.ToString().ToUpper().ToCharArray()[j];
                    j++;
                }
                else
                {
                    response += a.ToCharArray()[new Random().Next(0, a.Length - 1)];
                }
            }

            Console.WriteLine(s.ElapsedMilliseconds);
            return response;
        }
    }
}
