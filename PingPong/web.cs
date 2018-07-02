using System;
using System.Net;
using System.IO;

namespace PingPong
{
    class web
    {
        public string ip;
        string[] returnStatement;
        int failures;
        public bool alerted;
        string fmail;
        string tmail;
        string mxserver;
        string name;
        string[] searchwords;

        public web(string n, string i, string f, string t, string s,string sw)
        {
            ip = i;
            returnStatement = new string[6];
            searchwords = new string[3];
            failures = 0;
            alerted = false;
            fmail = f;
            tmail = t;
            mxserver = s;
            name = n;

            searchwords = sw.Split('-');

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
            failures += 5;
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
        public string[] LocalPing()
        {

            // Ping's the local machine.
            try {
                /*
                WebRequest request = WebRequest.Create(ip);
                request.Timeout = 40;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Console.WriteLine(response.StatusCode);
                if (response == null || response.StatusCode != HttpStatusCode.OK)
                {
                    returnStatement[0] = "-1";
                    returnStatement[1] = ip;

                    return returnStatement;
                }
                else
                {
                    returnStatement[0] = "1";
                    returnStatement[1] = ip;

                    return returnStatement;
                }
                */
                CookieContainer cookies = new CookieContainer();
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(ip);
                webRequest.CookieContainer = cookies;

                HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
                StreamReader responseReader = new StreamReader(response.GetResponseStream());

                string sResponseHTML = responseReader.ReadToEnd();

                if (response == null || response.StatusCode != HttpStatusCode.OK)
                {
                    returnStatement[0] = "-1";
                    returnStatement[1] = ip;

                    return returnStatement;
                }
                else
                {
                    bool swordFound = false;
                    for(int i = 0;i< searchwords.Length; i++)
                    {
                        if (sResponseHTML.Contains(searchwords[i]))
                        {
                            swordFound = true;
                        }
                    }
                    if (swordFound)
                    {
                        returnStatement[0] = "-2";
                        returnStatement[1] = ip;

                        return returnStatement;
                    }
                    else
                    {
                        returnStatement[0] = "1";
                        returnStatement[1] = ip;

                        return returnStatement;
                    }
                }

            }
            catch (Exception e)
            {
                returnStatement[0] = "-1";
                returnStatement[1] = ip;

                return returnStatement;
            }

        }
    }
}