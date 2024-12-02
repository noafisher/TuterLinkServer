namespace TutorLinkServer.DTO
{
    public class TeacherDTO
    {
        public int TeacherId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Pass { get; set; }
        public int CurrentClass { get; set; }
        public string UserAddress { get; set; }
        public double MaxDistance { get; set; }
        public bool GoToStudent { get; set; }
        public bool TeachAtHome { get; set; }
        public int Vetek {  get; set; }
        public int PricePerHour { get; set; }


    }
}
