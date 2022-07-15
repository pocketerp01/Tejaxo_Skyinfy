using HtmlAgilityPack;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using skyinfyMVC.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;


public class Mailer
{
    //Create 15 SMTP connections to the server.
    //We don't want to create some sort of DoS (Denial-of-service) attack 
    //because we don't want to create a new SMTP connection 
    //for every email being sent.

    private const int _clientcount = 15;
    private SmtpClient[] _smtpClients = new SmtpClient[1];
    private CancellationTokenSource _cancelToken;

    public List<MailObjs> data;
    public string UserCode = "", Client_id = "", Unit_id = "", FromAddress = "", FromName = "", MyGuid = "";
    public Mailer(string userCode, string cl_id, string unitid, String Myguid)
    {
        MyGuid = Myguid;
        UserCode = userCode;
        Client_id = cl_id;
        Unit_id = unitid;
        data = new List<MailObjs>();
        setupSMTPClients();
        
    }

    public void Add_Mail(string toAddress, string subject, string body, IList<HttpPostedFile> fileCollection)
    {
        MailObjs mail = new MailObjs();
        mail.toAddress = toAddress;
        mail.body = body;
        mail.subject = subject;
        mail.fileCollection = fileCollection;
        mail.fromAddress = FromAddress;
        mail.CC = FromAddress;
        mail.fromName = FromName;
        data.Add(mail);
    }


    public void Add_Mail(string toAddress, string subject, string body)
    {
        MailObjs mail = new MailObjs();
        mail.toAddress = toAddress;
        mail.body = body;
        mail.subject = subject;
        mail.fromAddress = FromAddress;
        mail.CC = FromAddress;
        mail.fromName = FromName;
        data.Add(mail);
    }
    public void Send_Email_Bulk()
    {
        try
        {
            ParallelOptions po = new ParallelOptions();
            //Create a cancellation token so you can cancel the task.
            _cancelToken = new CancellationTokenSource();
            po.CancellationToken = _cancelToken.Token;
            //Manage the MaxDegreeOfParallelism instead of .NET Managing this. We dont need 500 threads spawning for this.
            po.MaxDegreeOfParallelism = Environment.ProcessorCount * 2;
            try
            {

                Parallel.ForEach(data, po, (MailObjs dataobject) =>
                {
                    try
                    {
                        MailMessage msg = new MailMessage(dataobject.fromAddress, dataobject.toAddress);
                        msg.From = new MailAddress(dataobject.fromAddress, dataobject.fromName);
                        msg.Body = dataobject.body;
                        msg.IsBodyHtml = true;
                        msg.To.Add(dataobject.toAddress);
                        if (!dataobject.CC.Trim().Equals("")) msg.CC.Add(dataobject.CC);
                        msg.Subject = dataobject.subject;
                        msg.Priority = MailPriority.Normal;
                        if (dataobject.fileCollection != null)
                        {
                            for (int i = 0; i < dataobject.fileCollection.Count; i++)
                            {
                                string nana = dataobject.fileCollection[i].FileName.ToString();
                                msg.Attachments.Add(new Attachment(dataobject.fileCollection[i].InputStream, nana));

                            }
                        }
                        SendEmail(msg);
                    }
                    catch (Exception ex)
                    {
                        //Log error
                    }
                });
            }
            catch (OperationCanceledException e)
            {
                //User has cancelled this request.
            }
        }
        finally
        {
            disposeSMTPClients();
        }
    }

    public void CancelEmailRun()
    {
        _cancelToken.Cancel();
    }

    private void SendEmail(MailMessage msg)
    {
        try
        {
            bool _gotlocked = false;
            while (!_gotlocked)
            {
                //Keep looping through all smtp client connections until one becomes available
                for (int i = 0; i <= _clientcount; i++)
                {
                    if (Monitor.TryEnter(_smtpClients[i]))
                    {
                        try
                        {
                            _smtpClients[i].Send(msg);
                        }
                        catch (Exception err)
                        {
                        }
                        finally
                        {
                            Monitor.Exit(_smtpClients[i]);
                        }
                        _gotlocked = true;
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }
                //Do this to make sure CPU doesn't ramp up to 100%
                Thread.Sleep(1);
            }
        }
        finally
        {
            msg.Dispose();
        }
    }

    private void setupSMTPClients()
    {

        sgenFun sgen = new sgenFun(MyGuid);
        DataTable dtmail = new DataTable();

        string mq = "select com_email,com_password,com_smtp,com_port,company_name from company_profile where company_profile_id='" + Client_id + "' and type='CP'";
        dtmail = sgen.getdata(UserCode, mq);



        //for (int i = 0; i <= _clientcount; i++)
        //{
        try
        {
            SmtpClient _client = new SmtpClient();
            if (dtmail.Rows.Count > 0)
            {
                _client.Host = Convert.ToString(dtmail.Rows[0]["com_Smtp"]);
                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                NetworkCred.UserName = Convert.ToString(dtmail.Rows[0]["com_Email"]);
                NetworkCred.Password = EncryptDecrypt.Decrypt(Convert.ToString(dtmail.Rows[0]["com_password"].ToString()));
                _client.UseDefaultCredentials = true;
                _client.Credentials = NetworkCred;
                _client.Port = Convert.ToInt32(dtmail.Rows[0]["com_Port"]);
                _client.EnableSsl = true;
                _smtpClients[0] = _client;
                FromName = dtmail.Rows[0]["company_name"].ToString().Replace(" ", "");
                FromAddress = Convert.ToString(dtmail.Rows[0]["com_Email"]);
            }

        }
        catch (Exception ex)
        {
            //Log Exception
        }
        //}
    }
    private void disposeSMTPClients()
    {
        for (int i = 0; i <= _clientcount; i++)
        {
            try
            {
                _smtpClients[i].Dispose();
            }
            catch (Exception ex)
            {
                //Log Exception
            }
        }
    }


    public static void Kaamkar(string cocd, string Myguid)
    {

        sgenFun sgen = new sgenFun(Myguid);
        /*BackgroundWorker sendingWorker = (BackgroundWorker)sender;*///Capture the BackgroundWorker that fired the event
                                                                      //object arrObjects = (object)args;//Collect the array of objects the we recived from the main thread

        string rec_id = "000001";//Get the numeric value from inside the objects array, don't forget to cast
                                 //StringBuilder sb = new StringBuilder();//Declare a new string builder to store the result.
        string user = "gsthelp001@gmail.com", pass = "srsoffice1";


        var client = new ImapClient();

        var my = @"<table width='100%' border='0' cellspacing='0' cellpadding='1' style='border-collapse: collapse; font-family: Arial,Helvetica,sans-serif;' class=''>
                            
                            <tbody class=''><tr class=''>
                                <td width='10' class='' style='padding: 1px;'></td>
                                <td height='5' class='' style='padding: 1px;'></td>
                                <td width='10' class='' style='padding: 1px;'></td>
                            </tr>
                            <tr class=''>
                                <td width='10' class='' style='padding: 1px;'></td>
                                <td style='padding: 1px; color: #106c9f;font-size: 18px;text-align: center;line-height: 18px;' class=''>
                                	Contact Details of the Customer                                </td>
                                <td width='10' class='' style='padding: 1px;'></td>
                            </tr>
                            
                            <tr class=''>
                                <td width='10' class='' style='padding: 1px;'></td>
                                <td height='5' class='' style='padding: 1px;'></td>
                                <td width='10' class='' style='padding: 1px;'></td>
                            </tr>
                            <tr class=''>
                                <td width='10' class='' style='padding: 1px;'></td>
                                <td style='padding: 1px; font-size: 14px; text-align: center;' class=''> Raaj Sachdeva [Dealer] <br class=''> <a href='mailto:raaj.sachdeva1@gmail.com' target='_blank' class='mailto-link'>raaj.sachdeva1@gmail.com</a> <br class=''> 91-9811692612<font color='green' class=''><b class=''>(Verified)</b></font> <br class=''> <b class=''>Query</b> : pls send detais</td>
                                <td width='10' class='' style='padding: 1px;'></td>
                            </tr>
                            
                            <tr class=''>
                                <td width='10' class='' style='padding: 1px;'></td>
                                <td height='5' class='' style='padding: 1px;'></td>
                                <td width='10' class='' style='padding: 1px;'></td>
                            </tr>
                             
                              
                            <tr class=''>
                                <td width='10' class='' style='padding: 1px;'></td>
                                <td align='center' class='' style='padding: 1px;'>
                                                                        <table width='180px' border='0' style='border-collapse: collapse; min-width: 140px;font-family: Arial,Helvetica,sans-serif;' cellspacing='0' cellpadding='10' class=''>
                                    <tbody class=''><tr class=''>
                                    <td bgcolor='#3399fe' style='padding: 10px; border: 1px #0565cb solid;color: #fff;font-size: 14px;' align='center' class=''>
    
                                    <a href='mailto:raaj.sachdeva1@gmail.com' style='color: #fff;text-decoration: none;width: 180px;display: block;' target='_blank' class='mailto-link'>
                                    	Reply to the Query    </a>
                                    </td>
                                    </tr>
                                    </tbody></table>
                                                                         

                                </td>
                                <td width='10' class='' style='padding: 1px;'></td>
                            </tr>
                            
                            <tr class=''>
                                <td width='10' class='' style='padding: 1px;'></td>
                                <td height='5' class='' style='padding: 1px;'></td>
                                <td width='10' class='' style='padding: 1px;'></td>
                            </tr>
                             
                            
                            <tr class=''>
                                <td width='10' class='' style='padding: 1px;'></td>
                                <td height='2' style='padding: 1px; text-align: center;' class=''><img src='https://static.99acres.com/mailers/mmm_html/demo-html/img/spacer.gif' class=''></td>
                                <td width='10' class='' style='padding: 1px;'></td>
                            </tr>
                            
                            <tr class=''>
                                <td width='10' class='' style='padding: 1px;'></td>
                                <td height='5' class='' style='padding: 1px;'></td>
                                <td width='10' class='' style='padding: 1px;'></td>
                            </tr>
                            <tr class=''>
                                <td width='10' class='' style='padding: 1px;'></td>
                                <td style='padding: 1px; font-size: 12px; text-align: center;' class=''>
                               		<a href='https://www.99acres.com/maillink?url=aHR0cHM6Ly93d3cuOTlhY3Jlcy5jb20vZG8vbXk5OWFjcmVzL2FsbF9yZXNwb25zZXM=' style='color: #000;font-weight: bold;color: #106c9f;text-decoration: none;' target='_blank' class='' rel='noopener'>View all responses</a>                                 </td>
                                <td width='10' class='' style='padding: 1px;'></td>
                            </tr>
                            
                            <tr class=''>
                                <td width='10' class='' style='padding: 1px;'></td>
                                <td height='10' class='' style='padding: 1px;'></td>
                                <td width='10' class='' style='padding: 1px;'></td>
                            </tr>
                        </tbody></table>";

        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(my);
        List<string> words = doc.DocumentNode.DescendantNodes()
                .Where(n => n.NodeType == HtmlNodeType.Text
                  && !string.IsNullOrWhiteSpace(HtmlEntity.DeEntitize(n.InnerText))
                  && n.ParentNode.Name != "style" && n.ParentNode.Name != "script")
                .Select(n => HtmlEntity.DeEntitize(n.InnerText).Trim())
                .Where(s => s.Length > 2).ToList();


        string mm = WebUtility.HtmlDecode(my);
        client.Connect("imap.gmail.com", 993, true);
        client.AuthenticationMechanisms.Remove("XOAUTH");
        client.Authenticate(user, pass);
        Int64 lastid = 0;
        try
        {
            lastid = Convert.ToInt64(sgen.seekval(cocd, "select  max(convert(int, messageid) ) as id from acres", "id"));
        }
        catch (Exception err)
        {
            lastid = 0;
        }
        client.Inbox.Open(FolderAccess.ReadWrite);
        //DateTime StartDate = DateTime.ParseExact("01/05/2018", "dd/MM/yyyy", null);
        DateTime StartDate = DateTime.ParseExact("2020-03-14 00:01 AM", "yyyy-MM-dd HH:mm tt", null);
        DateTime EndDate = DateTime.ParseExact("15/03/2020", "dd/MM/yyyy", null);

        for (DateTime dd = StartDate; dd.Date <= EndDate.Date; dd = dd.AddDays(1))
        {
            //DateTime dd = DateTime.Now.AddDays(-1);
            var mails = client.Inbox.Search(SearchQuery.And(SearchQuery.DeliveredOn(dd), SearchQuery.FromContains("ram@skyinfy.com"))).Where(UniqueId => UniqueId.Id > lastid).ToList();
            foreach (var uid in mails)
            {
                try
                {
                    MimeKit.MimeMessage message = client.Inbox.GetMessage(uid);
                    string body = WebUtility.HtmlEncode(message.HtmlBody);
                    body = mm;
                    string datet = message.Date.ToUniversalTime().ToString("yyy-MM-dd HH:mm:ss");
                    string mtype = "CU", requirement = "", search_dt = "", jobloc = "", name = "", mobile = "", email = "";
                    string xx = message.GetTextBody(MimeKit.Text.TextFormat.Plain).ToString().Replace("> ", "");
                    xx = Regex.Replace(xx, @"(\r|\n)+^ +", "~", RegexOptions.None | RegexOptions.Multiline).Replace('\u0009'.ToString(), "~");
                    char[] charSeparators = new char[] { '~', '\r', '\n' };

                    string[] blist = xx.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
                    int cnt = blist.Count();
                    int fk = 0, lk = 0;

                    if ("99" == "99")
                    {
                        for (int k = 0; k < cnt; k++)
                        {
                            if (blist[k].ToString().Trim().Equals("")) continue;
                            if (blist[k].ToString().Trim().StartsWith("Contact Details")) { fk = k; }
                            if (fk > 0 && k > fk)
                            {
                                try
                                {
                                    name = blist[k].ToString().Trim();
                                    email = blist[k + 1].ToString().Trim();
                                    mobile = blist[k + 2].ToString().Trim();
                                    requirement = blist[k + 3].ToString().Trim();
                                }
                                catch (Exception err) { }
                                break;
                            }
                            if (blist[k].ToString().Trim().StartsWith("Reply to the Query")) break;
                        }
                    }
                    if ("jd" == "jd")
                    {

                        for (int k = 0; k < cnt; k++)
                        {
                            if (blist[k].ToString().Trim().Equals("User Name:")) name = blist[k + 1].ToString().Trim();
                            if (blist[k].ToString().Trim().Equals("User Requirement:")) requirement = blist[k + 1].ToString().Trim();
                            if (blist[k].ToString().Trim().Equals("Search Date & Time:")) search_dt = blist[k + 1].ToString().Trim();
                            if (blist[k].ToString().Trim().Equals("User Phone:")) mobile = blist[k + 1].ToString().Trim();
                            if (blist[k].ToString().Trim().Equals("User Email:")) email = blist[k + 1].ToString().Trim();
                        }
                    }
                    if (name.Trim().Equals(""))
                    {

                    }
                    string mq0 = "insert into kc";
                    sgen.execute_cmd(cocd, mq0);
                }
                catch (Exception err)
                {
                    sgen.FILL_ERR(err.Message.ToString());
                }
            }
        }
        //e.Result = sb.ToString();// Send our result to the main thread!
    }
}