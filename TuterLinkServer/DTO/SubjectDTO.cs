namespace TutorLinkServer.DTO
{
    public class SubjectDTO
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }

        public SubjectDTO() { }

        public SubjectDTO(Models.Subject subject)
        {
            this.SubjectId = subject.SubjectId;
            this.SubjectName = subject.SubjectName;
        }

        public Models.Subject GetModels()
        {
            Models.Subject subject = new Models.Subject();
            {
                SubjectId = this.SubjectId;
                SubjectName = this.SubjectName;
            }

            return subject;
        }

    }
}
