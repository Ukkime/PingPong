using System;
using System.Net.Mail;

namespace PingPong
{
    class mailer
    {
        string fromMail;
        string server;
        string toMail;
        string name;

        public mailer(string f,string t,string s,string n)
        {
            fromMail = f;
            toMail = t;
            server = s;
            name = n;
        }
        public mailer(string f, string t, string s)
        {
            fromMail = f;
            toMail = t;
            server = s;
            name = "";
        }

        public bool sendMail(string s, string b)
        {
            try
            {

                MailMessage message = new MailMessage();
                message.To.Add(new MailAddress(toMail));
                message.From = new MailAddress(fromMail);
                message.Subject = s;
                message.Body = b;

                SmtpClient mailclient = new SmtpClient(server);
                mailclient.Send(message);
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool sendDownAlert(string ip)
        {
            return sendMail("Host " + name + " " + ip + " is Down!", "I can't communicate with host " + ip + ", maybe has a network problem or host is down!\nWhen host becomes alive again, i will send you a mail!\n\n\n  v1.1.235813");
        }

        public bool sendRecoverAlert(string ip)
        {
            return sendMail("Host " + name + " " + ip + " is Alive!", "Communication with host " + ip + " is restored!\nI will stay alert with this host.\n\n\n  v1.1.235813");
        }

    }
}
