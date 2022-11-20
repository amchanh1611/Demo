namespace Demo.DTO
{
    public class MessageRequest : MessageBase
    {
       public DateTime DateSend { get; set; } = DateTime.Now;
    }
}
