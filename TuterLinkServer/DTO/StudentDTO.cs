namespace TutorLinkServer.DTO
{
    public class StudentDTO
    {
        public int StudentId { get; set; }
        public string Email { get; set; }  
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Pass {  get; set; }   
        public int CurrentClass {  get; set; }
        public string UserAddress { get; set; }
        public string ProfileImagePath { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsAdmin { get; set; }
        public StudentDTO() { }

        public StudentDTO(Models.Student student) 
        {
            
            this.StudentId = student.StudentId;
            this.Email = student.Email;
            this.FirstName = student.FirstName;
            this.LastName = student.LastName;
            this.Pass = student.Pass;
            this.CurrentClass = student.CurrentClass;
            this.UserAddress = student.UserAddress;
            this.IsBlocked = student.IsBlocked;
            this.IsAdmin = student.IsAdmin.Value;
        }

        public Models.Student GetModels()
        {
            Models.Student modelsStudent = new Models.Student()
            {
                StudentId = this.StudentId,
                Email = this.Email,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Pass = this.Pass,
                UserAddress = this.UserAddress,
                IsBlocked = this.IsBlocked,
                IsAdmin= this.IsAdmin,
            };

            return modelsStudent;
        }

    }
}
