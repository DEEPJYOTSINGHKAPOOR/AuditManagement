using Authorization_Service.Models;
using Authorization_Service.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization_Service.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    // [ApiExplorerSettings(GroupName ="AuditAuthorizationOpenApiSpec")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository; 
        }


      

        [HttpPost]
        [Authorize]
        [Route("~/api/Users/CheckWhetherAuthorizedOrNot")]
        public IActionResult CheckWhetherAuthorizedOrNot()
        {

            return Ok();
        }


        /// <summary>
        /// will return token
        /// </summary>
        /// <param name="model">Authentication Model-contains username and password</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticationModel model)
        {
            if(model == null)
            {
                return BadRequest();
            }
            var user = _userRepository.Authenticate(model.Username, model.Password);
            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [Route("~/api/Users/register")]
        public IActionResult Register([FromBody] AuthenticationModel model)
        {
            bool ifUserNameUnique = _userRepository.isUserUnique(model.Username);
            if (!ifUserNameUnique)
            {
                return BadRequest(new { message = "Username already exists" });
            }
            var user = _userRepository.Register(model.Username, model.Password);

            if (user == null)
            {
                return BadRequest(new { message = "Error while registering" });
            }

            return Ok();
        }
    }
}
