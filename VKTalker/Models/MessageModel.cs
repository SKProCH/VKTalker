namespace VKTalker.Models
{
    public class MessageModel
    {
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string Message{ get; set; }
        public string Image { get; set; }
        public long? ChatId { get; set; }
        public string Date { get; set; }
    }
}