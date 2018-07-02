using System;
using System.Net.NetworkInformation;

namespace PingPong
{
    class host
    {
        public string ip;
        string[] returnStatement;
        int failures;
        public bool alerted;
        string fmail;
        string tmail;
        string mxserver;
        string name;

        public host(string n,string i, string f,string t,string s)
        {
            ip = i;
            returnStatement = new string[6];
            failures = 0;
            alerted = false;
            fmail = f;
            tmail = t;
            mxserver = s;
            name = n;
        }
        public void decreaseFailures()
        {
            failures--;
        }
        public int getFailures()
        {
            return failures;
        }
        public string getIp()
        {
            return ip;
        }
        public string getDelay()
        {
            return returnStatement[2];
        }
        public void registerFailure()
        {
            failures+=5;
        }
        public void clearFailures()
        {
            failures = 0;
            alerted = false;
        }
        public string getTMail()
        {
            return tmail;
        }
        public string getFMail()
        {
            return fmail;
        }
        public string getMXserver()
        {
            return mxserver;
        }
        public string getName()
        {
            return name;
        }
        public string[] LocalPing()
        {
            try
            {
                // Ping's the local machine.
                Ping pingSender = new Ping();
                var buffer = new byte[32];
                PingReply reply = pingSender.Send(ip, 1000, buffer, new PingOptions(600, false));

                if (reply.Status == IPStatus.Success)
                {
                    returnStatement[0] = "1";
                    returnStatement[1] = reply.Address.ToString();
                    returnStatement[2] = reply.RoundtripTime + "";
                    returnStatement[3] = reply.Options.Ttl + "";
                    returnStatement[4] = reply.Options.DontFragment + "";
                    returnStatement[5] = reply.Buffer.Length + "";

                    // Console.WriteLine(ip+" "+reply.RoundtripTime + " ms"+" "+ reply.Status);

                    return returnStatement;
                }
                else
                {
                    //Console.WriteLine(ip+" "+reply.Status);
                    returnStatement[0] = "-1";
                    returnStatement[1] = ip;
                    return returnStatement;
                }
            }catch(Exception e)
            {
                returnStatement[0] = "-1";
                returnStatement[1] = ip;
                return returnStatement;
            }

        }
    }
}
