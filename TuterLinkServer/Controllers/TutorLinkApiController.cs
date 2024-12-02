using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TutorLinkServer.Models;

namespace TutorLinkServer.Controllers
{
    [Route("api")]
    [ApiController]
    public class TutorLinkApiController : ControllerBase
    {
        //a variable to hold a reference to the db context!
        private NoaDBcontext context;
        //a variable that hold a reference to web hosting interface (that provide information like the folder on which the server runs etc...)
        private IWebHostEnvironment webHostEnvironment;
        //Use dependency injection to get the db context and web host into the constructor
        public TutorLinkApiController(NoaDBcontext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.webHostEnvironment = env;
        }

        //[HttpPost("register")]
        //public IActionResult Register([FromBody] DTO.UserDTO userDto)
        //{
        //    try
        //    {
        //        HttpContext.Session.Clear(); //Logout any previous login attempt

        //        //Get model user class from DB with matching email. 
        //        Models.User modelsUser = new User()
        //        {
        //            FirstName = userDto.FirstName,
        //            LastName = userDto.LastName,
        //            Email = userDto.Email,
        //            Pass = userDto.Pass,
        //            TypeId = userDto.TypeId
        //        };

        //        context.Users.Add(modelsUser);
        //        context.SaveChanges();

        //        //User was added!
        //        DTO.UserDTO dtoUser = new DTO.UserDTO(modelsUser);
        //        //dtoUser.ProfileImagePath = GetProfileImageVirtualPath(dtoUser.Id);
        //        return Ok(dtoUser);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //        //
        //    }

        //}

        //[HttpPost("login")]
        //public IActionResult Login([FromBody] DTO.LoginInfoDTO loginDto)
        //{
        //    try
        //    {
        //        HttpContext.Session.Clear(); //Logout any previous login attempt

        //        //Get model user class from DB with matching email. 
        //        Models.User? modelsUser = context.Users.Where(u => u.Email == loginDto.Email).FirstOrDefault();

        //        //Check if user exist for this email and if password match, if not return Access Denied (Error 403) 
        //        if (modelsUser == null || modelsUser.Pass != loginDto.Password)
        //        {
        //            return Unauthorized();
        //        }

        //        //Login suceed! now mark login in session memory!
        //        HttpContext.Session.SetString("loggedInUser", modelsUser.Email);

        //        DTO.UserDTO dtoUser = new DTO.UserDTO(modelsUser);
        //        return Ok(dtoUser);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }

        //}
    }

    

}
