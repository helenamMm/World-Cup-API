using MailKit.Net.Smtp;
using MimeKit;
using WorldCupProjectApi.Models;

namespace WorldCupProjectApi.Services;

public class NotificationService
{
    private readonly EmailConfiguration _emailConfig;
    //private List<string> _subscribedEmails;
    private readonly UsuarioService _usuarioService;

    public NotificationService(IConfiguration configuration, UsuarioService usuarioService)
    {
        _emailConfig = new EmailConfiguration
        {
            SmtpServer = "smtp.gmail.com",
            Port = 587,
            Username = "worldcupn450@gmail.com",
            Password = "mtcf bxyj spdo ttqg",
            FromEmail = "worldcupn450@gmail.com",
            FromName = "World Cup Project"
        };
        
        _usuarioService = usuarioService;
        //_subscribedEmails = new List<string>();
    }
    
    private async Task SendBulkEmailAsync(List<string> subscribedEmails, string subject, string body)
    {
        var tasks = subscribedEmails.Select(email => SendEmailAsync(email, subject, body));
        await Task.WhenAll(tasks);
    }

    public async Task SendGoalNotificationAsync(Partido partido, string equipoAnotador, string jugador)
    {
         var subscribedEmails = await _usuarioService.GetUsersToNotifyAsync(
            partido.Id, 
            partido.EquipoA.Id, 
            partido.EquipoB.Id
        );
        Console.WriteLine(subscribedEmails);
        if (!subscribedEmails.Any())
        {
            Console.WriteLine("No hay usuarios");
            return;
        }
        var subject = $"⚽ ¡GOOL! {equipoAnotador} ha anotado";
        var body = $@"
        <h1>⚽ ¡GOOL DEL MUNDIAL! ⚽</h1>
        <p><strong>{equipoAnotador}</strong> ha anotado un gol en el partido:</p>
        <p><strong>{partido.EquipoA?.Nombre} vs {partido.EquipoB?.Nombre}</strong></p>
        <p><strong>Jugador:</strong> {jugador}</p>
        <p><strong>Marcador actual:</strong> {partido.GolesEquipoA} - {partido.GolesEquipoB}</p>
        <p><strong>Estadio:</strong> {partido.Estadio}</p>
        <p><strong>Fase:</strong> {partido.Fase}</p>
        <br/>
        <p>¡Sigue el partido en vivo en nuestra app!</p>";

        await SendBulkEmailAsync(subscribedEmails, subject, body);
    }
    
    public async Task SendMatchUpdateNotificationAsync(Partido partido, string mensaje)
    {
        
        var subscribedEmails  = await _usuarioService.GetUsersToNotifyAsync(
            partido.Id, 
            partido.EquipoA.Id, 
            partido.EquipoB.Id
        );
        if (!subscribedEmails.Any())
        {
            Console.WriteLine("No hay usuarios");
            return;
        }
        var subject = $"📢 Hoy Juegan: {partido.EquipoA?.Nombre} vs {partido.EquipoB?.Nombre}";
        var body = $@"
        <h2>📢 Partido de hoy</h2>
        <p><strong>{partido.EquipoA?.Nombre}  - {partido.EquipoB?.Nombre}</strong></p>
        <p>{mensaje}</p>
        <p><strong>Estadio:</strong> {partido.Estadio}</p>
        <p><strong>Fase:</strong> {partido.Fase}</p>
        <br/>
        <p>¡No te lo pierdas!</p>";

        await SendBulkEmailAsync(subscribedEmails, subject, body);
    }
    
    private async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailConfig.FromName, _emailConfig.FromEmail));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body
            };

            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_emailConfig.Username, _emailConfig.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            Console.WriteLine($"Email enviado a: {toEmail}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error enviando email a {toEmail}: {ex.Message}");
        }
    }
}

