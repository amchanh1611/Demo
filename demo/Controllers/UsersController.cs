using demo.Models;
using Demo.BUS.IBUS;
using Demo.DTO;
using Demo.Helper.JWT;
using Demo.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace Demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserBUS userBUS;
        private readonly ILoginGoogleBUS loginGoogleBUS;
        private readonly IJwtUtils jwtUtils;

        public UsersController(IUserBUS userBUS, ILoginGoogleBUS loginGoogleBUS,IJwtUtils jwtUtils)
        {
            this.userBUS = userBUS;
            this.loginGoogleBUS = loginGoogleBUS;
            this.jwtUtils = jwtUtils;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateUserRequest request)
        {
            bool result = await userBUS.CreateAsync(request);
            if (result)
                return Ok();
            return BadRequest("Eror");
        }

        [HttpGet]
        public ActionResult GetList()
        {
            List<ResponseUser> result = userBUS.GetList();
            if (result != null)
                return Ok(result);
            return BadRequest("Eror");
        }

        [HttpGet("{userId}")]
        public ActionResult Get([FromRoute] int userId)
        {
            HttpContext context = HttpContext;
            ResponseUser result = userBUS.Get(context, userId);
            if (result != null)
                return Ok(result);
            return BadRequest("Eror");
        }

        [HttpPost("Login")]
        public ActionResult Login([FromBody] LoginRequest request)
        {
            LoginResponse result = userBUS.Login(request);
            if (result != null)
                return Ok(result);
            return BadRequest("Incorrect username/password");
        }

        [HttpPut("{userId}")]
        public ActionResult Upadte([FromRoute] int userId, [FromForm] UpdateUserRequest request)
        {
            bool result = userBUS.Update(userId, request);
            if (result)
                return Ok();
            return BadRequest("Eror");
        }

        [HttpDelete("{userId}")]
        public ActionResult Delete([FromRoute] int userId)
        {
            bool result = userBUS.Delete(userId);
            if (result)
                return Ok();
            return BadRequest("Eror");
        }

        [HttpPut("Profile"), Authorize]
        public ActionResult UpdateProfile([FromForm] UpdateUserRequest request)
        {
            Claim? claim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            int userId = int.Parse(claim!.Value);
            //var currentUser = (User)HttpContext.Items["User"];
            //if(currentUser == null)
            //    return BadRequest("User is invalid");
            //int userId=currentUser.Id;
            bool result = userBUS.Update(userId, request);
            if (result)
                return Ok();
            return BadRequest("Update Fail");
        }
        [HttpPost("LoginGoogle")]
        public async Task<IActionResult> LoginGoogleAsync(ExternalAuthDto externalAuthDto)
        {
            Payload payload = await userBUS.VerifyGoogleToken(externalAuthDto);
            if (payload == null)
                return BadRequest("Invalid External Authentication.");

            GoogleLogin user = loginGoogleBUS.FindByCondition(payload.Subject);
            string token = string.Empty;
            if(user!=null)
            {
                token = jwtUtils.GenerateJwtToken(user.User);
                return Ok(user);
            }
               

            bool createUser = await userBUS.CreateAsync(new CreateUserRequest { FullName = payload.Name, Email = payload.Email, UserName = payload.Email });

            User userNewCreate = userBUS.FindByEmail(payload.Email);
            bool createGoogleLogin = loginGoogleBUS.Create(new GoogleLogin { Key = payload.Subject, UserId = userNewCreate.Id });

            token = jwtUtils.GenerateJwtToken(userNewCreate);
            return Ok(new LoginResponse(token));
        }
    }
}
