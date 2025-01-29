namespace TutorLinkServer.DTO
{
    public class ReportDTO
    {
        public int ReportId { get; set; }
        public int TeacherId { get; set; }
        public int StudentId { get; set; }
        public bool ReportedByStudent { get; set; }
        public string? ReportText { get; set; }

        public ReportDTO(Models.Report r)
        {

            this.ReportId = r.ReportId;
            this.TeacherId = r.TeacherId;
            this.StudentId = r.StudentId;
            this.ReportedByStudent = r.ReportedByStudent;
            this.ReportId = r.ReportId;
           
        }

        public ReportDTO() { }

        public Models.Report GetModels()
        {

            Models.Report UserReport = new Models.Report()
            {
                ReportId = this.ReportId,
                TeacherId = this.TeacherId,
                StudentId = this.StudentId,
                ReportedByStudent = this.ReportedByStudent,
                ReportText = this.ReportText,
               
            };

            return UserReport;
        }
    }
}
