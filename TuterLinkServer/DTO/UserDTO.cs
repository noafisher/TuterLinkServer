namespace TutorLinkServer.DTO
{
    public class UserDTO
    {
        public string Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Pass { get; set; }
        public int? TypeId { get; set; }

        public UserDTO() {}

        public UserDTO(Models.User modelUser) 
        {
            this.Email = modelUser.Email;
            this.FirstName = modelUser.FirstName;
            this.LastName = modelUser.LastName;
            this.Pass = modelUser.Pass;
            this.TypeId = modelUser.TypeId;
        }



    }
}
