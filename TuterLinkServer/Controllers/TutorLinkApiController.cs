﻿using Microsoft.AspNetCore.Http;
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
    }

    

}
