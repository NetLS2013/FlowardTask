namespace MediatorService.Interfaces.IServices
{
    public interface IRabbitMQService
    {
        bool SendEmail(string emailBody);
    }
}
