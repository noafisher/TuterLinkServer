using System.ComponentModel.DataAnnotations.Schema;

namespace TutorLinkServer.DTO
{
    public class LessonDTO
    {
        public int LessonId { get; set; }
        public int TeacherId { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public DateTime TimeOfLesson { get; set; }
        public StudentDTO? Student { get; set; }

        public LessonDTO(Models.Lesson l)
        {

            this.LessonId = l.LessonId;
            this.TeacherId = l.TeacherId;
            this.StudentId = l.StudentId;
            this.SubjectId = l.SubjectId;
            this.TimeOfLesson = l.TimeOfLesson;
            if (l.Student != null)
                this.Student = new StudentDTO(l.Student);

        }

        public LessonDTO() { }

        public Models.Lesson GetModels()
        {

            Models.Lesson lesson = new Models.Lesson()
            {
                LessonId= this.LessonId,
                TeacherId = this.TeacherId,
                StudentId = this.StudentId,
                SubjectId = this.SubjectId,
                TimeOfLesson = this.TimeOfLesson,

            };

            return lesson;
        }

    }
}
