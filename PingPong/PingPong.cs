using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace PingPong
{
    public partial class PingPong : Form
    {
        List<host> hosts;
        List<web> webs;
        List<Label> labels;
        List<Label> mslabels;
        List<ProgressBar> progressbars;
        Label transparencyLabel;
        Label condensedLabel;
        Label ontop;
        Label viewLog;
        logManager logger;
        int timeout;
        bool condensed;
        bool transparency;
        bool isontop;
        int x;
        int y;



        public PingPong()
        {
            InitializeComponent();
            this.LostFocus += new EventHandler(FLostFocus);
            this.GotFocus += new EventHandler(FGotFocus);
            logger = new logManager();

            hosts = new List<host>();
            webs = new List<web>();
            labels = new List<Label>();
            mslabels = new List<Label>();
            progressbars = new List<ProgressBar>();


            x = 10;
            y = 10;
            timeout = 500;
            condensed = true;
            transparency = true;
            isontop = true;



            loadIps();
            loadWebs();

            if (hosts.Count() > 0 )
            {
                for (int i = 0; i <= hosts.IndexOf(hosts.Last()); i++)
                {
                    startPing(hosts[i]);
                }
            }

            if (webs.Count() > 0)
            {
                for (int i = 0; i <= webs.IndexOf(webs.Last()); i++)
                {
                    startWebPing(webs[i]);
                }
            }
            endpaint();


        }

        void FLostFocus(object sender, EventArgs e)
        {
            if(transparency==true)
             this.Opacity = .30;
        }
        void FGotFocus(object sender, EventArgs e)
        {
            if (transparency == true)
                this.Opacity = .98;
        }
        private void loadWebs()
        {
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(@"webs.ini");
            while ((line = file.ReadLine()) != null)
            {
                String[] substrings = line.Split(';');

                webs.Add(new web(substrings[0], substrings[1], substrings[2], substrings[3], substrings[4], substrings[5]));
                labels.Add(AddLabel(substrings[1], substrings[0]));
                mslabels.Add(AddMSLabel(substrings[1]));
                progressbars.Add(AddPB(substrings[1]));
                newline();
            }
            file.Close();
        }

        private void loadIps()
        {
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(@"ips.ini");
            while ((line = file.ReadLine()) != null)
            {
                String[] substrings = line.Split(';');

                hosts.Add(new host(substrings[0], substrings[1], substrings[2], substrings[3], substrings[4]));
                labels.Add(AddLabel(substrings[1], substrings[0]));
                mslabels.Add(AddMSLabel(substrings[1]));
                progressbars.Add(AddPB(substrings[1]));
                newline();
            }
            file.Close();
        }
        private void endpaint()
        {
            transparencyLabel = new Label();
            condensedLabel = new Label();
            ontop = new Label();
            viewLog = new Label();

            int fontSize = 7;

            Size forCenter = this.Size;
            transparencyLabel.Location = new Point((forCenter.Width / 3) - 88, y);
            transparencyLabel.Size = new System.Drawing.Size(92, 20);
            transparencyLabel.Text = String.Format("Transparency ON");
            transparencyLabel.Font = new Font("Serif", fontSize);
            transparencyLabel.ForeColor = Color.Green;

            condensedLabel.Location = new Point((forCenter.Width / 3) * 2 - 89, y);
            condensedLabel.Size = new System.Drawing.Size(84, 20);
            condensedLabel.Text = String.Format("Condensed ON");
            condensedLabel.Font = new Font("Serif", fontSize);
            condensedLabel.ForeColor = Color.Green;

            ontop.Location = new Point((forCenter.Width / 3) * 3 - 98, y);
            ontop.Size = new System.Drawing.Size(60, 20);
            ontop.Text = String.Format("Ontop ON");
            ontop.Font = new Font("Serif", fontSize);
            ontop.ForeColor = Color.Green;

            viewLog.Location = new Point((forCenter.Width / 3) * 3 - 42, y);
            viewLog.Size = new System.Drawing.Size(26, 20);
            viewLog.Text = String.Format("Log");
            viewLog.Font = new Font("Serif", fontSize);
            viewLog.ForeColor = Color.RoyalBlue;

            Controls.Add(transparencyLabel);
            Controls.Add(condensedLabel);
            Controls.Add(ontop);
            Controls.Add(viewLog);


            transparencyLabel.Click += new EventHandler(remarkTransparency);
            condensedLabel.Click += new EventHandler(resizeForm);
            ontop.Click += new EventHandler(checkOntop);
            viewLog.Click += new EventHandler(openLog);
        }

        private void openLog(object sender, EventArgs e)
        {
            Process.Start("timeouts.log");
        }

        private void checkOntop(object sender, EventArgs e)
        {
            if(isontop == true)
            {
                ontop.Text = String.Format("Ontop OFF");
                ontop.ForeColor = Color.Gray;
                isontop = false;
                this.TopMost = false;
            } else
            {
                ontop.Text = String.Format("Ontop ON");
                ontop.ForeColor = Color.Green;
                isontop = true;
                this.TopMost = true;
            }
        }

        private void remarkTransparency(object sender, EventArgs e)
        {
            if (transparency == true)
            {
                transparencyLabel.Text = String.Format("Transparency OFF");
                transparencyLabel.ForeColor = Color.Gray;
                transparency = false;
                this.Opacity = .98;
            } else
            {
                transparencyLabel.Text = String.Format("Transparency ON");
                transparencyLabel.ForeColor = Color.Green;
                transparency = true;
            }
        }

        private void resizeForm(object sender, EventArgs e)
        {
            if (condensed == true)
            {
                condensedLabel.Text = String.Format("Condensed OFF");
                condensedLabel.ForeColor = Color.Gray;
                condensed = false;
            }
            else
            {
                condensedLabel.Text = String.Format("Condensed ON");
                condensedLabel.ForeColor = Color.Green;
                condensed = true;
            }

            for (int i = 0; i <= labels.IndexOf(labels.Last()); i++)
            {
                if (condensed == true)
                {
                    labels[i].Size = new System.Drawing.Size(98, 20);
                }
                else
                {
                    labels[i].Size = new System.Drawing.Size(260, 20);
                }
            }
            for (int i = 0; i <= progressbars.IndexOf(progressbars.Last()); i++)
            {
                if (condensed == true)
                    progressbars[i].Location = new System.Drawing.Point(progressbars[i].Location.X - 198, progressbars[i].Location.Y);
                else
                    progressbars[i].Location = new System.Drawing.Point(progressbars[i].Location.X + 198, progressbars[i].Location.Y);
            }
            for (int i = 0; i <= mslabels.IndexOf(mslabels.Last()); i++)
            {
                if (condensed == true)
                    mslabels[i].Location = new System.Drawing.Point(mslabels[i].Location.X -198, mslabels[i].Location.Y);
                else
                    mslabels[i].Location = new System.Drawing.Point(mslabels[i].Location.X + 198, mslabels[i].Location.Y);
            }
        }

        private void newline()
        {
            y += 20;
        }


        private Label AddLabel(string ip, string description)
        {
            Label dynamicLabel = new Label();
            dynamicLabel.Location = new System.Drawing.Point(x,y);
            dynamicLabel.Text = String.Format("[{0}]   {1}{2}", ip, repairtab(ip),description);
            dynamicLabel.Name = ip+"lb";
            if (condensed == true)
            {
                dynamicLabel.Size = new System.Drawing.Size(98, 20);
            }
            else
            {
                dynamicLabel.Size = new System.Drawing.Size(260, 20);
            }
            Controls.Add(dynamicLabel);

            return dynamicLabel;
        }

        private object repairtab(string ip)
        {
            int diff = 0;
            string correction = "";
            if((diff = 15-ip.Length) > 0)
            {
                switch (diff)
                {
                    case 1:
                        correction += "  ";
                        break;
                    case 2:
                        correction += "    ";
                        break;
                    case 3:
                        correction += "      ";
                        break;
                }
            }
            //Console.WriteLine("["+correction+"]" + " " +diff+" "+ip);
            return correction+" ";
        }

        private Label AddMSLabel(string ip)
        {
            Label dynamicLabel = new Label();
            if (condensed == true)
                dynamicLabel.Location = new System.Drawing.Point(x + 100+89, y);
            else
                dynamicLabel.Location = new System.Drawing.Point(x + 262 + 89, y);
            dynamicLabel.Text = "????";
            dynamicLabel.Name = ip+"ms";
            dynamicLabel.Size = new System.Drawing.Size(52, 20);
            Controls.Add(dynamicLabel);

            return dynamicLabel;
        }
        private ProgressBar AddPB(string ip)
        {
            ProgressBar dynamicPB = new ProgressBar();
            if (condensed == true)
                dynamicPB.Location = new System.Drawing.Point(x+100, y-2);
            else
                dynamicPB.Location = new System.Drawing.Point(x + 262, y - 2);
            dynamicPB.Text = ip;
            dynamicPB.Name = ip+"pb";
            dynamicPB.Size = new System.Drawing.Size(88, 16);
            dynamicPB.Maximum = timeout;
            dynamicPB.Minimum = 0;
            dynamicPB.Style = ProgressBarStyle.Continuous;
            Controls.Add(dynamicPB);

            return dynamicPB;
        }

        private void startPing(host pinguedHost)
        {

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                runThread(pinguedHost);
            }).Start();

        }
        private void startWebPing(web pinguedWeb)
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                runWebThread(pinguedWeb);
            }).Start();

        }
        private void runThread(host pinguedHost)
        {
            while (true)
            {
                processPingResult(pinguedHost.LocalPing());
                Thread.Sleep(1000);
            }
        }
        private void runWebThread(web pinguedHost)
        {
            while (true)
            {
                processWebResult(pinguedHost.LocalPing());
                Thread.Sleep(5000);
            }
        }

        private void processPingResult(string[] result)
        {
            ProgressBar updatepb = progressbars.Find(x => x.Name.Equals(result[1] + "pb"));
            Label updatems = mslabels.Find(x => x.Name.Equals(result[1] + "ms"));
            Label updatelb = labels.Find(x => x.Name.Equals(result[1] + "lb"));
            host updatehHost = hosts.Find(x => x.ip.Equals(result[1]));
            try
            {
                if (result[0].Equals("1") == true)
                {
                    int mstemp = Int32.Parse(result[2]);
                    int mspaint = 0;
                    if (mstemp <= 0) { mstemp = 1; }
                    if (mstemp <= 9) { mspaint = 9; }
                    else if (mstemp >= timeout) { mspaint = timeout; }
                    else { mspaint = mstemp; }

                    updatepb.Invoke(new Action(() => updatepb.Value = mspaint));
                    updatems.Invoke(new Action(() => updatems.Text = mstemp + " ms"));
                    if (mspaint < (timeout / 2) - 50) { updatepb.Invoke(new Action(() => updatepb.ForeColor = Color.Green)); }
                    else if (mspaint > (timeout / 2) + 150) { updatepb.Invoke(new Action(() => updatepb.ForeColor = Color.Green)); }
                    if (updatehHost.getFailures() < 5 && updatehHost.alerted == true)
                    {
                        updatehHost.clearFailures();
                        //Console.WriteLine("Host " + updatehHost.getIp() + " RECUPERADO");

                        updatems.Invoke(new Action(() => updatems.ForeColor = Color.Black));
                        updatelb.Invoke(new Action(() => updatelb.ForeColor = Color.Black));

                        mailer alertmailer = new mailer(updatehHost.getFMail(), updatehHost.getTMail(), updatehHost.getMXserver());
                        if (alertmailer.sendRecoverAlert(updatehHost.getIp()))
                        {
                            //Console.WriteLine("Recover sended: "+updatehHost.getIp());
                            logger.registerSendRecoveryhost(updatehHost.getIp());
                        }

                    }
                    else if (updatehHost.getFailures() > 0)
                    {
                        updatehHost.decreaseFailures();
                    }
                    else if (updatehHost.getFailures() == 0)
                    {
                        updatems.Invoke(new Action(() => updatems.ForeColor = Color.Black));
                        updatelb.Invoke(new Action(() => updatelb.ForeColor = Color.Black));
                    }
                }
                else if (result[0].Equals("-1") == true)
                {
                    if (updatehHost.getFailures() > 150)
                    {
                        if (updatehHost.alerted == false)
                        {
                            updatehHost.alerted = true;
                            //MessageBox.Show("Host " + updatehHost.getIp() + " ha caido!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            //Console.WriteLine("Host " + updatehHost.getIp() + " ha caido!");

                            mailer alertmailer = new mailer(updatehHost.getFMail(), updatehHost.getTMail(), updatehHost.getMXserver(),updatehHost.getName());
                            if (alertmailer.sendDownAlert(updatehHost.getIp()))
                            {
                                //Console.WriteLine("Alert sended: " + updatehHost.getIp());
                                logger.registerSendAlertHost(updatehHost.getIp());
                            }
                        }
                    }
                    else
                    {
                        updatehHost.registerFailure();
                    }
                    if (updatehHost.getFailures() > 10)
                    {
                        updatepb.Invoke(new Action(() => updatepb.Value = timeout));
                        updatepb.Invoke(new Action(() => updatepb.ForeColor = Color.Red));
                        updatems.Invoke(new Action(() => updatems.Text = "Down!"));
                        updatems.Invoke(new Action(() => updatems.ForeColor = Color.Red));
                        updatelb.Invoke(new Action(() => updatelb.ForeColor = Color.Red));
                        logger.registerOfflineHost(updatehHost.getIp());
                    }
                }
            }catch(Exception e)
            {
                //todo
            }
            
        }
        private void processWebResult(string[] result)
        {
            ProgressBar updatepb = progressbars.Find(x => x.Name.Equals(result[1] + "pb"));
            Label updatems = mslabels.Find(x => x.Name.Equals(result[1] + "ms"));
            Label updatelb = labels.Find(x => x.Name.Equals(result[1] + "lb"));
            web updatehHost = webs.Find(x => x.ip.Equals(result[1]));

            if (result[0].Equals("1") == true)
            {

                updatepb.Invoke(new Action(() => updatepb.Value = timeout));
                updatepb.Invoke(new Action(() => updatepb.ForeColor = Color.Green));
                updatems.Invoke(new Action(() => updatems.Text = "ONLINE"));
                updatems.Invoke(new Action(() => updatems.ForeColor = Color.Green));

                if (updatehHost.getFailures() < 5 && updatehHost.alerted == true)
                {
                    updatehHost.clearFailures();
                    //Console.WriteLine("Host " + updatehHost.getIp() + " RECUPERADO");

                    updatems.Invoke(new Action(() => updatems.ForeColor = Color.Green));
                    updatelb.Invoke(new Action(() => updatelb.ForeColor = Color.Black));

                    mailer alertmailer = new mailer(updatehHost.getFMail(), updatehHost.getTMail(), updatehHost.getMXserver());
                    if (alertmailer.sendRecoverAlert(updatehHost.getIp()))
                    {
                        Console.WriteLine("Recover sended: "+updatehHost.getIp());
                        logger.registerSendRecoverWeb(updatehHost.getIp());
                    }

                }
                else if (updatehHost.getFailures() > 0)
                {
                    updatehHost.decreaseFailures();
                }
                else if (updatehHost.getFailures() == 0)
                {
                    updatems.Invoke(new Action(() => updatems.ForeColor = Color.Green));
                    updatelb.Invoke(new Action(() => updatelb.ForeColor = Color.Black));
                }
            }
            else if (result[0].Equals("-1") == true || result[0].Equals("-2") == true)
            {
                if (updatehHost.getFailures() > 25)
                {
                    if (updatehHost.alerted == false)
                    {
                        updatehHost.alerted = true;
                        mailer alertmailer = new mailer(updatehHost.getFMail(), updatehHost.getTMail(), updatehHost.getMXserver());
                        if (alertmailer.sendDownAlert(updatehHost.getIp()))
                        {
                            Console.WriteLine("Alert sended: " + updatehHost.getIp());
                            logger.registerSendAlertWeb(updatehHost.getIp());
                        }
                    }
                }
                else
                {
                    updatehHost.registerFailure();
                }
                if (updatehHost.getFailures() > 10)
                {
                    if (result[0].Equals("-1") == true)
                    {
                        updatepb.Invoke(new Action(() => updatepb.Value = timeout));
                        updatepb.Invoke(new Action(() => updatepb.ForeColor = Color.Red));
                        updatems.Invoke(new Action(() => updatems.Text = "OFFLINE"));
                        updatems.Invoke(new Action(() => updatems.ForeColor = Color.Red));
                        updatelb.Invoke(new Action(() => updatelb.ForeColor = Color.Red));
                        logger.registerOfflineWeb(updatehHost.getIp());
                    }
                    else if (result[0].Equals("-2") == true)
                    {
                        updatepb.Invoke(new Action(() => updatepb.Value = timeout));
                        updatepb.Invoke(new Action(() => updatepb.ForeColor = Color.DarkMagenta));
                        updatems.Invoke(new Action(() => updatems.Text = "W/ERROR"));
                        updatems.Invoke(new Action(() => updatems.ForeColor = Color.DarkMagenta));
                        updatelb.Invoke(new Action(() => updatelb.ForeColor = Color.DarkMagenta));
                        logger.registerFailWeb(updatehHost.getIp());
                    }
                }
            }

        }
    }
}
