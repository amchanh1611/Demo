using Demo.BUS.IBUS;
using Demo.DTO;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserBUS userBUS;
        private readonly IValidator<CreateUserRequest> validator;

        public UsersController(IUserBUS userBUS, IValidator<CreateUserRequest> validator)
        {
            this.userBUS = userBUS;
            this.validator = validator;
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

        [HttpPut("Profile")]
        public ActionResult UpdateProfile([FromForm] UpdateUserRequest request)
        {
            Claim? claim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                return BadRequest("User is invalid");
            int userId = int.Parse(claim.Value);
            bool result = userBUS.Update(userId, request);
            if (result)
                return Ok();
            return BadRequest("Update Fail");
        }
    }
}