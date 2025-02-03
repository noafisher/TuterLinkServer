using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TutorLinkServer.DTO
{
    public class ChatMessageDTO
    {
        public int MessageId { get; set; }

        public int TeacherId { get; set; }

        public int StudentId { get; set; }

        public bool IsTeacherSender { get; set; }

        public string? MessageText { get; set; }

        public DateTime TextTime { get; set; }

        public ChatMessageDTO(Models.ChatMessage chatMessage)
        {
            MessageId = chatMessage.MessageId;
            TeacherId = chatMessage.TeacherId;
            StudentId = chatMessage.StudentId;
            MessageText = chatMessage.MessageText;
            TextTime = chatMessage.TextTime;
            IsTeacherSender = chatMessage.IsTeacherSender;
        }

        public Models.ChatMessage GetModel()
        {
            return new Models.ChatMessage()
            {
                MessageId = MessageId,
                TeacherId = TeacherId,
                StudentId = StudentId,
                MessageText = MessageText,
                TextTime = TextTime,
                IsTeacherSender = IsTeacherSender
            };
        }
    }
}
