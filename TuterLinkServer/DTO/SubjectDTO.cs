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

    public class TeacherSubjectDTO
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int MinClass { get; set; }

        public int MaxClass { get; set; }
        public TeacherSubjectDTO() { }

        public TeacherSubjectDTO(Models.TeachersSubject ts)
        {
            this.SubjectId = ts.SubjectId;
            this.SubjectName = ts.Subject.SubjectName;
            this.MinClass = ts.MinClass;
            this.MaxClass = ts.MaxClass;
            this.Id = ts.Id;

        }

        public Models.TeachersSubject GetModels()
        {
            Models.TeachersSubject subject = new Models.TeachersSubject()
            {
                SubjectId = this.SubjectId,
                MinClass = this.MinClass,
                MaxClass = this.MaxClass,
                Id = this.Id
            };

            return subject;
        }

    }
}
