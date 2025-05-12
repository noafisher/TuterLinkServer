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
                dtoStudent.ProfileImagePath = GetProfileImageVirtualPath(dtoStudent.StudentId, false);
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
                Models.Teacher? modelsTeacher = context.Teachers.Include(t => t.TeachersSubjects)
                                                                .ThenInclude(ts => ts.Subject).Where(u => u.Email == loginDto.Email).FirstOrDefault();

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


        //get all teachers
        [HttpGet("GetAllTeachers")]
        public IActionResult GetAllTeachers()
        {
            try
            {
                List<Teacher> listTeachers = context.Teachers.Include(t=>t.TeachersSubjects).ThenInclude(s=>s.Subject).ToList(); ;
                List<TeacherDTO> l = new List<TeacherDTO>();
                foreach (Teacher t in listTeachers)
                {
                    TeacherDTO teacher = new TeacherDTO(t);
                    teacher.ProfileImagePath = GetProfileImageVirtualPath(teacher.TeacherId, true);
                    l.Add(teacher);
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

        //get all subjects
        [HttpGet("GetTeacherSubjects")]
        public IActionResult GetTeacherSubjects()
        {
            try
            {
                //Check if who is logged in
                string? userEmail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User is not logged in");
                }

                //Get model user class from DB with matching email. 
                Models.Teacher? t = context.Teachers.Include(t => t.TeachersSubjects)
                                                    .ThenInclude(ts => ts.Subject).Where(tt => tt.Email == userEmail).FirstOrDefault();
                List<SubjectDTO> l = new List<SubjectDTO>();

                if (t != null)
                {
                    foreach (TeachersSubject s in t.TeachersSubjects)
                    {
                        l.Add(new SubjectDTO(s.Subject));
                    }
                }
                return Ok(l);



            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // post a review 
        [HttpPost("RateTeacher")]
        public IActionResult RateTeacher([FromBody] DTO.ReviewDTO reviewDTO)
        {
            try
            {

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
            int id = 0;
            string folder = "";
            Models.Teacher? t = context.Teachers.Include(t=>t.TeachersSubjects).ThenInclude(s=>s.Subject).Where(tt => tt.Email == userEmail).FirstOrDefault();
            if (t != null)
            {
                user = t;
                id = t.TeacherId;
                folder = "Teacher";
            }
            else
            {
                Models.Student? s = context.Students.Where(ss => ss.Email == userEmail).FirstOrDefault();
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
                DTO.StudentDTO dtoStudent = new DTO.StudentDTO((Student)user);
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

        [HttpPost("ReportUser")]
        public IActionResult ReportUser([FromBody] DTO.ReportDTO reportDTO)
        {
            try
            {
                
                //Get model user class from DB with matching email. 
                Models.Report reportModel = reportDTO.GetModels();
                


                context.Reports.Update(reportModel);
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
                    StudentDTO student = new StudentDTO(s);
                    student.ProfileImagePath = GetProfileImageVirtualPath(student.StudentId, false);
                    l.Add(student);
                }
                return Ok(l);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        //get all lessons from a specific date
        [HttpPost("GetAllLessons")]
        public IActionResult GetAllLessons([FromBody] DateOnly dateOnly)
        {
            try
            {
                List<Lesson> listLessons = context.Lessons.Include(l => l.Student).ToList(); ;
                List<LessonDTO> l = new List<LessonDTO>();
                foreach (Lesson lesson in listLessons)
                {
                    if (lesson.TimeOfLesson.Day == dateOnly.Day && lesson.TimeOfLesson.Month == dateOnly.Month && lesson.TimeOfLesson.Year == dateOnly.Year)
                        l.Add(new LessonDTO(lesson));
                }
                return Ok(l);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("AddLesson")]
        public IActionResult AddLesson([FromBody] DTO.LessonDTO lessonDTO)
        {
            try
            {
                
                //Get model user class from DB with matching email. 
                Models.Lesson lessonModel = lessonDTO.GetModels();


                context.Lessons.Add(lessonModel);
                context.SaveChanges();

                //lesson was added!
                DTO.LessonDTO dtoLesson = new DTO.LessonDTO(lessonModel);
                //dtoUser.ProfileImagePath = GetProfileImageVirtualPath(dtoUser.Id);
                return Ok(dtoLesson);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpGet("FindStudents")]
        public IActionResult FindStudents([FromQuery] string search)
        {
            try
            {
                List<Student> list = context.Students.Where(s => s.FirstName.Contains(search) ||
                                                                 s.LastName.Contains(search)).ToList();

                //Create DTO USer list
                List<DTO.StudentDTO> result = new List<StudentDTO>();
                if (list != null)
                {
                    foreach (Student s in list)
                    {
                        StudentDTO studentDTO = new StudentDTO(s);
                        studentDTO.ProfileImagePath = GetProfileImageVirtualPath(studentDTO.StudentId, false);
                        result.Add(studentDTO);
                    }
                        
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpGet("GetReportsNotProcessed")]
        public IActionResult GetReportsNotProcessed()
        {
            try
            {
                List<Report> listReports = context.Reports.Include(r=>r.Teacher).Include(r=>r.Student).ToList(); ;
                List<ReportDTO> l = new List<ReportDTO>();
                foreach (Report r in listReports)
                {
                    if (r.Processed == false)
                    {
                        ReportDTO report = new ReportDTO(r);
                        report.Student.ProfileImagePath = GetProfileImageVirtualPath(r.StudentId, false);
                        report.Teacher.ProfileImagePath = GetProfileImageVirtualPath(r.TeacherId, true);

                        l.Add(report);
                    }
                        
                }
                return Ok(l);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("BlockStudent")]
        public IActionResult BlockStudent([FromQuery] int id)
        {
            try
            {
                //Check if who is logged in
                string? userEmail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User is not logged in");
                }

                if (!IsAdmin(userEmail))
                {
                    return Unauthorized();
                }

                Student? student = context.Students.Where(s => s.StudentId == id).FirstOrDefault();

                if ( student != null)
                {
                    student.IsBlocked = true;
                    context.Update(student);
                    context.SaveChanges();
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ProcessReport")]
        public IActionResult ProcessReport([FromQuery] int id)
        {
            try
            {
                //Check if who is logged in
                string? userEmail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User is not logged in");
                }

                if (!IsAdmin(userEmail))
                {
                    return Unauthorized();
                }

                Report? report = context.Reports.Where(r => r.ReportId == id).FirstOrDefault();

                if (report != null)
                {
                    report.Processed = true;
                    context.Update(report);
                    context.SaveChanges();
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("BlockTeacher")]
        public IActionResult BlockTeacher([FromQuery] int id)
        {
            try
            {
                //Check if who is logged in
                string? userEmail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User is not logged in");
                }

                if (!IsAdmin(userEmail))
                {
                    return Unauthorized();
                }

                Teacher? teacher = context.Teachers.Where(t=>t.TeacherId==id).FirstOrDefault();

                if (teacher != null)
                {
                    teacher.IsBlocked = true;
                    context.Update(teacher);
                    context.SaveChanges();
                    return Ok();
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private bool IsAdmin(string email)
        {
            Student? student = context.Students.Where(s => s.Email == email).FirstOrDefault();
            if (student != null)
            {
                return student.IsAdmin.Value;
            }

            Teacher? teacher = context.Teachers.Where(t=>t.Email == email).FirstOrDefault();
            if (teacher != null)
            {
                return teacher.IsAdmin.Value;
            }

            return false;
        }

    

     [HttpPost("UpdateStudent")]
        public IActionResult UpdateStudent([FromBody] DTO.StudentDTO studentDto)
        {
            try
            {
                //Check if who is logged in
                string? studentEmail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(studentEmail))
                {
                    return Unauthorized("User is not logged in");
                }

                //Get model user class from DB with matching email. 
                Models.Student? student = context.Students.Where(s => s.Email == studentEmail).FirstOrDefault();

                //Check if the logged in user is admin

                bool isAdmin = IsAdmin(studentEmail);
                //Clear the tracking of all objects to avoid double tracking
                context.ChangeTracker.Clear();

                //Check if the user that is logged in is the same user of the task
                //this situation is ok only if the user is a manager
                if (student == null || (isAdmin == false && studentDto.StudentId != student.StudentId))
                {
                    return Unauthorized("Non Manager User is trying to update a different user");
                }

                Models.Student appStudent = studentDto.GetModels();

                context.Entry(appStudent).State = EntityState.Modified;

                context.SaveChanges();

                //Task was updated!
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


    

    [HttpPost("UpdateTeacher")]
        public IActionResult UpdateTeacher([FromBody] DTO.TeacherDTO teacherDto)
        {
            try
            {
                //Check if who is logged in
                string? teacherEmail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(teacherEmail))
                {
                    return Unauthorized("User is not logged in");
                }

                //Get model user class from DB with matching email. 
                Models.Teacher? teacher = context.Teachers.Where(s => s.Email == teacherEmail).FirstOrDefault();

                //Check if the logged in user is admin
                bool isAdmin = IsAdmin(teacherEmail);

                //Clear the tracking of all objects to avoid double tracking
                context.ChangeTracker.Clear();

                //Check if the user that is logged in is the same user of the task
                //this situation is ok only if the user is a manager
                if (teacher == null || (isAdmin == false && teacherDto.TeacherId != teacher.TeacherId))
                {
                    return Unauthorized("Non Manager User is trying to update a different user");
                }

                Models.Teacher appTeacher = teacherDto.GetModels();

                context.Entry(appTeacher).State = EntityState.Modified;

                context.SaveChanges();

                //Task was updated!
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


    }

}
