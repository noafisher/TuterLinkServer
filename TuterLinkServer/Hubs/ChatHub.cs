using TutorLinkServer.Models;
using Microsoft.AspNetCore.SignalR;
using TutorLinkServer.DTO;
using Microsoft.EntityFrameworkCore;


namespace TutorLinkServer.Hubs

{
    public class ChatHub:Hub   
    {
        private static Dictionary<string, string> connectedStudents = new Dictionary<string, string>();
        private static Dictionary<string, string> connectedTeachers = new Dictionary<string, string>();

        private readonly NoaDBcontext dbContext;
        public ChatHub(NoaDBcontext dbContext)
        {
            this.dbContext = dbContext;
        }


        //This method gets student id (sender), teacherid (reciever) and the message to be sent
        public async Task SendMessageToTeacher(string studentId, string teacherId, string message)
        {
            try
            {
                //Write message to DB
                int sid = int.Parse(studentId);
                int tid = int.Parse(teacherId);
                ChatMessage m = new ChatMessage()
                {
                    StudentId = sid,
                    TeacherId = tid,
                    MessageText = message,
                    IsTeacherSender = false,
                    TextTime = DateTime.Now
                };
                dbContext.ChatMessages.Add(m);
                dbContext.SaveChanges();

                //Read Student from DB
                Student? s = dbContext.Students.Where(ss => ss.StudentId == sid).FirstOrDefault();
                StudentDTO sDto = new StudentDTO(s);
                //Send message to teacher if teacher is connected
                //Find all connections for the user id who need to recieve the message
                List<KeyValuePair<string, string>>? connections = connectedTeachers.Where(x => x.Value == teacherId).ToList();
                //If all is good, loop through the connections and send them all the message
                if (connections != null)
                {
                    foreach (KeyValuePair<string, string> connection in connections)
                    {
                        await Clients.Client(connection.Key).SendAsync("ReceiveMessageFromStudent", sDto,  m);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        //This method gets teacher id (sender), studentid (reciever) and the message to be sent
        public async Task SendMessageToStudent(string teacherId, string studentId, string message)
        {
            try
            {
                //Write message to DB
                int sid = int.Parse(studentId);
                int tid = int.Parse(teacherId);
                ChatMessage m = new ChatMessage()
                {
                    StudentId = sid,
                    TeacherId = tid,
                    MessageText = message,
                    IsTeacherSender = true,
                    TextTime = DateTime.Now
                };
                dbContext.ChatMessages.Add(m);
                dbContext.SaveChanges();

                //Read Student from DB
                Teacher? t = dbContext.Teachers.Where(ss => ss.TeacherId== tid).FirstOrDefault();
                TeacherDTO tDto = new TeacherDTO(t);
                //Send message to teacher if teacher is connected
                //Find all connections for the user id who need to recieve the message
                List<KeyValuePair<string, string>>? connections = connectedStudents.Where(x => x.Value == studentId).ToList();
                //If all is good, loop through the connections and send them all the message
                if (connections != null)
                {
                    foreach (KeyValuePair<string, string> connection in connections)
                    {
                        await Clients.Client(connection.Key).SendAsync("ReceiveMessageFromTeacher", tDto, m);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        public async Task<List<MessagesFromTeacher>> OnStudentConnect(string studentId)
        {
            int sid = int.Parse(studentId);
            connectedStudents.Add(Context.ConnectionId, studentId);
            //Read message history from DB
            List<ChatMessage> messages = dbContext.ChatMessages.Where(mm => mm.StudentId == sid).
                Include(mm => mm.Teacher).
                OrderBy(mm => mm.TeacherId).ToList();
            List<MessagesFromTeacher> output = new List<MessagesFromTeacher>();
            if (messages != null) 
            {
                ChatMessage prev = null;
                MessagesFromTeacher current = null;
                foreach (ChatMessage message in messages) 
                {
                    if (prev == null || prev.TeacherId != message.TeacherId)
                    {
                        current = new MessagesFromTeacher()
                        {
                            Teacher = new TeacherDTO(message.Teacher),
                            Messages = new List<ChatMessageDTO>()
                        };
                        output.Add(current);
                    }
                    current.Messages.Add(new ChatMessageDTO(message));
                    prev = message;
                }

            }
            

            await base.OnConnectedAsync();
            return output;
        }

        public async Task<List<MessagesFromStudent>> OnTeacherConnect(string teacherId)
        {
            int tid = int.Parse(teacherId);
            connectedTeachers.Add(Context.ConnectionId, teacherId);
            //Read message history from DB
            List<ChatMessage> messages = dbContext.ChatMessages.Where(mm => mm.TeacherId == tid).
                Include(mm => mm.Student).
                OrderBy(mm => mm.StudentId).ToList();
            List<MessagesFromStudent> output = new List<MessagesFromStudent>();
            if (messages != null)
            {
                ChatMessage prev = null;
                MessagesFromStudent current = null;
                foreach (ChatMessage message in messages)
                {
                    if (prev == null || prev.StudentId != message.StudentId)
                    {
                        current = new MessagesFromStudent()
                        {
                            Student = new StudentDTO(message.Student),
                            Messages = new List<ChatMessageDTO>()
                        };
                        output.Add(current);
                    }
                    current.Messages.Add(new ChatMessageDTO(message));
                    prev = message;
                }
            }


            await base.OnConnectedAsync();
            return output;
        }

        public async Task OnDisconnect(bool isTeacher)
        {
            if (isTeacher) 
            {
                connectedTeachers.Remove(Context.ConnectionId);
            }
            else
                connectedStudents.Remove(Context.ConnectionId);
            await base.OnDisconnectedAsync(null);
        }
    }
}
    

