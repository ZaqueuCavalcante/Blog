namespace Blog.Services
{
    public interface IEmailSender
    {
        void Send(Message message);
    }
}
