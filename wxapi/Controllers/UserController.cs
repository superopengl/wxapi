
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using wxapi.Services;

namespace wxapi.Controllers
{
    [Route("api/answers/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
		private readonly IConfigService configService;

		public UserController(IConfigService configService)
		{
			this.configService = configService;
		}
		// GET api/values
		[HttpGet]
        public ActionResult<object> Get()
        {
            return new { name = configService.GetName(), token = configService.GetToken() };
        }
    }
}
