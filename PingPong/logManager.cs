using System;
using System.Globalization;

namespace PingPong
{

    class logManager
    {

        public void registerTimeout(string ip)
        {
            saveLine("["+getTime()+"]"+" Timeout detected in "+ip);
        }

        private void saveLine(string s)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"timeouts.log", true))
            {
                file.WriteLine(s);
            }
        }

        private string getTime()
        {
            DateTime localDate = DateTime.Now;
            var culture = new CultureInfo("es-ES");
            return localDate.ToString(culture);

        }

        public void registerOfflineWeb(string ip)
        {
            saveLine("[" + getTime() + "]" + " OFFLINE WEB : " + ip);
        }

        public void registerFailWeb(string ip)
        {
            saveLine("[" + getTime() + "]" + " WEB WITH ERRORS : " + ip);
        }

        public void registerSendAlertWeb(string ip)
        {
            saveLine("[" + getTime() + "]" +" ALERT SENDED : " + ip);
        }

        public void registerSendRecoverWeb(string ip)
        {
            saveLine("[" + getTime() + "]" + " RECOVERY SENDED : " + ip);
        }

        public void registerOfflineHost(string ip)
        {
            saveLine("[" + getTime() + "]" + " OFFLINE HOST : " + ip);
        }

        public void registerSendAlertHost(string ip)
        {
            saveLine("[" + getTime() + "]" + " ALERT SENDED : " + ip);
        }

        public void registerSendRecoveryhost(string ip)
        {
            saveLine("[" + getTime() + "]" + " RECOVERY SENDED : " + ip);
        }
    }
}