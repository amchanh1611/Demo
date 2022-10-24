using Demo.BUS.IBUS;
using Demo.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserBUS userBUS;
        public UsersController(IUserBUS userBUS)
        {
            this.userBUS = userBUS;
        }
        [HttpPost]
        public ActionResult Create([FromForm] CreateUserRequest request)
        {
            bool result = userBUS.Create(request);
            if (result)
                return Ok();
            return BadRequest("Eror");
        }
        [HttpGet]
        public ActionResult GetList()
        {
            List<ResponseUser> result = userBUS.GetList();
            if (result!= null)
                return Ok(result);
            return BadRequest("Eror");
        }
        [HttpGet("{userId}")]
        public ActionResult Get([FromRoute] int userId)
        {
            ResponseUser result = userBUS.Get(userId);
            if (result != null)
                return Ok(result);
            return BadRequest("Eror");
        }
        [HttpPost("Login")]
        public ActionResult Login([FromForm] LoginRequest request)
        {
            bool result = userBUS.Login(request);
            if (result)
                return Ok();
            return BadRequest("Eror");
        }
        [HttpPut("{userId}")]
        public ActionResult Upadte([FromRoute] int userId,[FromForm] UpdateUserRequest request)
        {
            bool result = userBUS.Update(userId,request);
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
    }
}
