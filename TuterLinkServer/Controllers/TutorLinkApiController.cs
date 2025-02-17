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
                dtoTeacher.ProfileImagePath = GetProfileImageVirtualPath(dtoTeacher.TeacherId, true);
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
               dtoStudent.ProfileImagePath = GetProfileImageVirtualPath(dtoStudent.StudentId,false);
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
                dtoTeacher.ProfileImagePath = GetProfileImageVirtualPath(dtoTeacher.TeacherId, false);

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
                dtoStudent.ProfileImagePath = GetProfileImageVirtualPath(dtoStudent.StudentId, false);
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
                List<Teacher> listTeachers = context.Teachers.ToList(); ;
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
                List<Subject> listSubjects = context.Subjects.ToList(); ;
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

        #region profile images
        [HttpPost("UploadProfileImage")]
        public async Task<IActionResult> UploadProfileImageAsync(IFormFile file)
        {
            //Check if who is logged in
            string? userEmail = HttpContext.Session.GetString("loggedInUser");
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized("User is not logged in");
            }

            //Get model user class from DB with matching email. 
            Object user = null;
            int id=0;
            string folder="";
            Models.Teacher? t = context.Teachers.Where(tt => tt.Email == userEmail).FirstOrDefault();
            if (t != null)
            {
                user = t;
                id = t.TeacherId;
                folder = "Teacher";
            }
            else
            {
                Models.Student? s = context.Students.Where(ss=>ss.Email == userEmail).FirstOrDefault();
                if (s != null)
                {
                    user = s; 
                    id = s.StudentId;
                    folder = "Student";
                }
            }
            //Clear the tracking of all objects to avoid double tracking
            context.ChangeTracker.Clear();

            if (user == null)
            {
                return Unauthorized("User is not found in the database");
            }


            //Read all files sent
            long imagesSize = 0;

            if (file.Length > 0)
            {
                //Check the file extention!
                string[] allowedExtentions = { ".png", ".jpg" };
                string extention = "";
                if (file.FileName.LastIndexOf(".") > 0)
                {
                    extention = file.FileName.Substring(file.FileName.LastIndexOf(".")).ToLower();
                }
                if (!allowedExtentions.Where(e => e == extention).Any())
                {
                    //Extention is not supported
                    return BadRequest("File sent with non supported extention");
                }

                //Build path in the web root (better to a specific folder under the web root
                string filePath = $"{this.webHostEnvironment.WebRootPath}\\profileImages\\{folder}\\{id}{extention}";

                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);

                    if (IsImage(stream))
                    {
                        imagesSize += stream.Length;
                    }
                    else
                    {
                        //Delete the file if it is not supported!
                        System.IO.File.Delete(filePath);
                    }

                }

            }

            if (user is Teacher)
            {
                DTO.TeacherDTO dtoTeacher = new DTO.TeacherDTO((Teacher)user);
                dtoTeacher.ProfileImagePath = GetProfileImageVirtualPath(id, true);
                return Ok(dtoTeacher);
            }
            else
            {
                DTO.StudentDTO dtoStudent= new DTO.StudentDTO((Student)user);
                dtoStudent.ProfileImagePath = GetProfileImageVirtualPath(id, false);
                return Ok(dtoStudent);
            }
            
        }

        //this function gets a file stream and check if it is an image
        private static bool IsImage(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);

            List<string> jpg = new List<string> { "FF", "D8" };
            List<string> bmp = new List<string> { "42", "4D" };
            List<string> gif = new List<string> { "47", "49", "46" };
            List<string> png = new List<string> { "89", "50", "4E", "47", "0D", "0A", "1A", "0A" };
            List<List<string>> imgTypes = new List<List<string>> { jpg, png };

            List<string> bytesIterated = new List<string>();

            for (int i = 0; i < 8; i++)
            {
                string bit = stream.ReadByte().ToString("X2");
                bytesIterated.Add(bit);

                bool isImage = imgTypes.Any(img => !img.Except(bytesIterated).Any());
                if (isImage)
                {
                    return true;
                }
            }

            return false;
        }

        //this function check which profile image exist and return the virtual path of it.
        //if it does not exist it returns the default profile image virtual path
        private string GetProfileImageVirtualPath(int userId, bool isTeacher)
        {
            string userType = "Student";
            if (isTeacher) 
            {
                userType = "Teacher";
            }

            string virtualPath = $"/profileImages/{userType}/{userId}";
            string path = $"{this.webHostEnvironment.WebRootPath}\\profileImages\\{userType}\\{userId}.png";
            if (System.IO.File.Exists(path))
            {
                virtualPath += ".png";
            }
            else
            {
                path = $"{this.webHostEnvironment.WebRootPath}\\profileImages\\{userType}\\{userId}.jpg";
                if (System.IO.File.Exists(path))
                {
                    virtualPath += ".jpg";
                }
                else
                {
                    virtualPath = $"/profileImages/default.png";
                }
            }

            return virtualPath;
        }
        #endregion

        #region Report 
        [HttpPost("ReportUser")]
        public IActionResult ReportUser([FromBody] DTO.ReportDTO reportDTO)
        {
            try
            {
                HttpContext.Session.Clear(); //Logout any previous login attempt

                //Get model user class from DB with matching email. 
                Models.Report reportModel = reportDTO.GetModels();


                context.Reports.Add(reportModel);
                context.SaveChanges();

                //Review was added!
                DTO.ReportDTO dtoReport = new DTO.ReportDTO(reportModel);
                //dtoUser.ProfileImagePath = GetProfileImageVirtualPath(dtoUser.Id);
                return Ok(dtoReport);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }


        //get all students
        [HttpGet("GetAllStudents")]
        public IActionResult GetAllStudents()
        {
            try
            {
                List<Student> listStudents = context.Students.ToList(); ;
                List<StudentDTO> l = new List<StudentDTO>();
                foreach (Student s in listStudents)
                {
                    l.Add(new StudentDTO(s));
                }
                return Ok(l);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        #endregion
    }




}
