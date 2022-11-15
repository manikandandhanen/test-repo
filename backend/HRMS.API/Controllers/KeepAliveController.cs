
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KeepAliveController : ApiController
    {
        public KeepAliveController()
        {
        }       
   
        [HttpGet("{input}")]
        public string ReturnInput(string input)
        {
            return input;
        }
    }
}