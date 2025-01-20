namespace TutorLinkServer.DTO
{
    public class ReviewDTO
    {
        public int ReviewId { get; set; }
        public int TeacherId { get; set; }
        public int StudentId { get; set; }
        public DateTime TimeOfReview { get; set; }
        public string ReviewText { get; set; }

        public ReviewDTO(Models.TeacherReview ts)
        {

            this.ReviewId = ts.ReviewId;
            this.TeacherId = ts.TeacherId;
            this.StudentId = ts.StudentId;
            this.TimeOfReview = ts.TimeOfReview;
            this.ReviewText = ts.ReviewText;
        }

        public Models.TeacherReview GetModels()
        {

            Models.TeacherReview teacherReviewModel = new Models.TeacherReview()
            {
                ReviewId = this.ReviewId,
                TeacherId = this.TeacherId,
                StudentId = this.StudentId,
                TimeOfReview = this.TimeOfReview,
                ReviewText = this.ReviewText
            };

            return teacherReviewModel;
        }

    }
}
