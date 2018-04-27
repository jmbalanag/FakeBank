using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeBank.WebApp.Controllers
{
    public class BaseController : Controller
    {
        public string UserName => HttpContext.User.Identity.Name;

        public Guid UserId => Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value);
    }
}
