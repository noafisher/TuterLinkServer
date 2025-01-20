using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TutorLinkServer.DTO;
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

        [HttpPost("registerTeacher")]
        public IActionResult RegisterTeacher([FromBody] DTO.TeacherDTO teacherDTO)
        {
            try
            {
                HttpContext.Session.Clear(); //Logout any previous login attempt

                //Get model user class from DB with matching email. 
                Models.Teacher modelsTeacher = teacherDTO.GetModels();
                

                context.Teachers.Add(modelsTeacher);
                context.SaveChanges();

                //User was added!
                DTO.TeacherDTO dtoTeacher = new DTO.TeacherDTO(modelsTeacher);
                //dtoUser.ProfileImagePath = GetProfileImageVirtualPath(dtoUser.Id);
                return Ok(dtoTeacher);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                
            }

        }

        [HttpPost("registerStudent")]
        public IActionResult RegisterStudent([FromBody] DTO.StudentDTO studentDTO)
        {
            try
            {
                HttpContext.Session.Clear(); //Logout any previous login attempt

                //Get model user class from DB with matching email. 
                Models.Student modelsStudent = studentDTO.GetModels();


                context.Students.Add(modelsStudent);
                context.SaveChanges();

                //User was added!
                DTO.StudentDTO dtoStudent = new DTO.StudentDTO(modelsStudent);
                //dtoUser.ProfileImagePath = GetProfileImageVirtualPath(dtoUser.Id);
                return Ok(dtoStudent);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }

        [HttpPost("loginTeacher")]
        public IActionResult LoginTeacher([FromBody] DTO.LoginInfoDTO loginDto)
        {
            try
            {
                HttpContext.Session.Clear(); //Logout any previous login attempt

                //Get model user class from DB with matching email. 
                Models.Teacher? modelsTeacher = context.Teachers.Where(u => u.Email == loginDto.Email).FirstOrDefault();

                //Check if user exist for this email and if password match, if not return Access Denied (Error 403) 
                if (modelsTeacher == null || modelsTeacher.Pass != loginDto.Password)
                {
                    return Unauthorized();
                }

                //Login suceed! now mark login in session memory!
                HttpContext.Session.SetString("loggedInUser", modelsTeacher.Email);

                DTO.TeacherDTO dtoTeacher = new DTO.TeacherDTO(modelsTeacher);
                return Ok(dtoTeacher);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("loginStudent")]
        public IActionResult LoginStudent([FromBody] DTO.LoginInfoDTO loginDto)
        {
            try
            {
                HttpContext.Session.Clear(); //Logout any previous login attempt

                //Get model user class from DB with matching email. 
                Models.Student? modelsStudent = context.Students.Where(u => u.Email == loginDto.Email).FirstOrDefault();

                //Check if user exist for this email and if password match, if not return Access Denied (Error 403) 
                if (modelsStudent == null || modelsStudent.Pass != loginDto.Password)
                {
                    return Unauthorized();
                }

                //Login suceed! now mark login in session memory!
                HttpContext.Session.SetString("loggedInUser", modelsStudent.Email);

                DTO.StudentDTO dtoStudent = new DTO.StudentDTO(modelsStudent);
                return Ok(dtoStudent);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        #region Teachers list page 
        //get all teachers
        [HttpGet("GetAllTeachers")]
        public IActionResult GetAllTeachers()
        {
            try
            {
                List<Teacher> listTeachers = context.GetAllTeachers();
                List<TeacherDTO> l = new List<TeacherDTO>();
                foreach(Teacher t in listTeachers)
                {
                    l.Add(new TeacherDTO(t));
                }
                return Ok(l);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        //get all subjects
        [HttpGet("GetAllSubjects")]
        public IActionResult GetAllSubjects()
        {
            try
            {
                List<Subject> listSubjects = context.GetAllSubjects();
                List<SubjectDTO> l = new List<SubjectDTO>();
                foreach (Subject s in listSubjects)
                {
                    l.Add(new SubjectDTO(s));
                }
                return Ok(l);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        #endregion

        #region Rate Teacher page

        // post a review 
        [HttpPost("RateTeacher")]
        public IActionResult RateTeacher([FromBody] DTO.ReviewDTO reviewDTO)
        {
            try
            {
                HttpContext.Session.Clear(); //Logout any previous login attempt

                //Get model user class from DB with matching email. 
                Models.TeacherReview reviewModel = reviewDTO.GetModels();


                context.TeacherReviews.Add(reviewModel);
                context.SaveChanges();

                //Review was added!
                DTO.ReviewDTO dtoReviow = new DTO.ReviewDTO(reviewModel);
                //dtoUser.ProfileImagePath = GetProfileImageVirtualPath(dtoUser.Id);
                return Ok(dtoReviow);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }


        #endregion
    }




}
