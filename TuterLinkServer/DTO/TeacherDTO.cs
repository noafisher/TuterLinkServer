using TutorLinkServer.Models;

namespace TutorLinkServer.DTO
{
    public class TeacherDTO
    {
        public int TeacherId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Pass { get; set; }
        public string UserAddress { get; set; }
        public double MaxDistance { get; set; }
        public bool GoToStudent { get; set; }
        public bool TeachAtHome { get; set; }
        public int Vetek {  get; set; }
        public int PricePerHour { get; set; }
        public string ProfileImagePath { get; set; }
        public List<TeacherSubjectDTO>? TeacherSubjects { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsAdmin { get; set; }
        public TeacherDTO() { } 

        public TeacherDTO(Models.Teacher teacher)
        {
            this.TeacherId = teacher.TeacherId;
            this.Email = teacher.Email; 
            this.FirstName = teacher.FirstName;
            this.LastName = teacher.LastName;
            this.Pass = teacher.Pass;
            this.UserAddress = teacher.UserAddress;
            this.MaxDistance = teacher.MaxDistance;
            this.Vetek = teacher.Vetek;
            this.PricePerHour = teacher.PricePerHour;
            if (teacher.TeachersSubjects != null)
            {
                this.TeacherSubjects = new List<TeacherSubjectDTO>();
                foreach(Models.TeachersSubject s in teacher.TeachersSubjects)
                {
                    this.TeacherSubjects.Add(new TeacherSubjectDTO(s));
                }
            }
            this.IsBlocked = teacher.IsBlocked;
            this.IsAdmin = teacher.IsAdmin.Value;

        }

        public Models.Teacher GetModels()
        {
            Models.Teacher modelsTeacher = new Models.Teacher()
            {
                TeacherId = this.TeacherId,
                Email = this.Email,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Pass = this.Pass,
                UserAddress = this.UserAddress,
                MaxDistance = this.MaxDistance,
                Vetek = this.Vetek,
                PricePerHour = this.PricePerHour,
                IsBlocked = this.IsBlocked,
                IsAdmin = this.IsAdmin,
                
            };
            if (this.TeacherSubjects != null)
            {
                modelsTeacher.TeachersSubjects = new List<TeachersSubject>();
                foreach (TeacherSubjectDTO s in this.TeacherSubjects)
                {
                    modelsTeacher.TeachersSubjects.Add(s.GetModels());
                }

            }

            return modelsTeacher;
        }


    }
}
