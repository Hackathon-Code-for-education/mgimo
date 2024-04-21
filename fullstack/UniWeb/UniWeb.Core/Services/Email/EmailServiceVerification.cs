using System.Net;
using System.Net.Mail;

namespace UniWeb.Core.Services.Email;

public class EmailServiceVerification
{
    #region Private Fields

    private readonly MailAddress _mailFrom = new MailAddress("siquelljase@yandex.ru", "Russin Universities");

    private const string SUBJECT = "Подтвердите адрес своей студенческой почты";

    private readonly SmtpClient _client;
    
    #endregion

    #region Constructor

    public EmailServiceVerification()
    {
        _client = new SmtpClient("smtp.yandex.ru", 587);
        _client.EnableSsl = true;
        _client.Credentials = new NetworkCredential("siquelljase@yandex.ru", "JLgHe5Vs"); 
    }

    #endregion

    #region Public Methods

    public bool SendVerificationEmail(string email, string verificationUrl)
    {
        try
        {
            var mail = new MailMessage();
            mail.From = _mailFrom;
            mail.To.Add(new MailAddress(email)); // Адрес получателя
            mail.Subject = SUBJECT;
            mail.Body = "Спасибо за регистрацию в нашем сервисе Университеты России! Мы в скором времени подтвердим вашу учетную запись! Преейдите по ссылке ниже, чтобы поддвердить вашу почту: \n\n " +
                        verificationUrl + "\n\n" +
                        "Ссылка работает только один раз, второй раз проходить верификацию нельзя";
            
            _client.Send(mail);

            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    #endregion
}