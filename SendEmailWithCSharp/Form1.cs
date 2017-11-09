using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;

namespace SendEmailWithCSharp
{
    public partial class Form1 : Form
    {
        NetworkCredential login;
        SmtpClient client;
        MailMessage msg;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnSendEmail_Click(object sender, EventArgs e)
        {
            login = new NetworkCredential(txtUsername.Text, txtPassword.Text);
            //client = new SmtpClient(txtSmtp.Text);
            //client.Port = Convert.ToInt32(txtPort.Text);
            //client.EnableSsl = chkSSL.Checked;

            //SmtpClient client = new SmtpClient("xxx.clientserver.com", 555);//555 is port number
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);//587 is port number
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = login;
            msg = new MailMessage();
            msg.From = new MailAddress(txtUsername.Text);
            msg.To.Add(txtTo.Text);
            //if (!string.IsNullOrEmpty(txtCC.Text)) msg.To.Add(new MailAddress(txtCC.Text));
            msg.Subject = txtSubject.Text;
            msg.Body = txtMessage.Text;
            client.Send(msg);
            MessageBox.Show("Your message has been sent successfully", "Success Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
        }

        private void sendEmail()
        {
            login = new NetworkCredential(txtUsername.Text, txtPassword.Text);
            client = new SmtpClient(txtSmtp.Text);
            client.Port = Convert.ToInt32(txtPort.Text);
            client.EnableSsl = chkSSL.Checked;
            client.Credentials = login;
            //client.Host = "relay-hosting.secureserver.net";
            msg = new MailMessage { From = new MailAddress(txtUsername.Text + txtSmtp.Text.Replace("smtp.", "@"), "SendMailApp", Encoding.UTF8) };
            msg.To.Add(new MailAddress(txtTo.Text));
            if (!string.IsNullOrEmpty(txtCC.Text)) msg.To.Add(new MailAddress(txtCC.Text));
            msg.Subject = txtSubject.Text;
            msg.Body = txtMessage.Text;
            msg.BodyEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.Priority = MailPriority.Normal;
            msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallBack);
            string userState = "Sending....";
            client.SendAsync(msg, userState);
        }

        private static void SendCompletedCallBack(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
                MessageBox.Show(string.Format("{0} send canceled.", e.UserState), "Failure Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (e.Error != null)
                MessageBox.Show(string.Format("{0}\n {1}", e.UserState, e.Error), "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show("Your message has been sent successfully", "Success Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
