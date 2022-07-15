using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace skyinfyMVC.Models
{
    public class BulkSMS
    {

        private UsersClient client;
        List<SMSObjs> SmSs = new List<SMSObjs>();
        string SmsUser = "", SmsPass = "", SmsSender = "", SmsRoute = "", UserCode = "", Client_id = "", Unit_Id = "", MyGuid = "";
        public BulkSMS(string userCode, string clinet_Id, string unit_Id,string Myguid)
        {
            MyGuid = Myguid;
            client = new UsersClient();
            UserCode = userCode;
            Client_id = clinet_Id;
            Unit_Id = unit_Id;
            getSmsSettings();
            SmSs = new List<SMSObjs>();
            

        }
        public void Add_sms(string MobileNos, string SmsText)
        {

            SMSObjs sms = new SMSObjs();

            string baseurl = "", DCS = "0";

            switch (UserCode)
            {

                default:
                    baseurl = "http://login.smsmantra.online/api/mt/SendSMS?user=" + Uri.EscapeUriString(SmsUser) +
                         "&password=" + Uri.EscapeUriString(SmsPass) + "&senderid=" + Uri.EscapeUriString(SmsSender) +
                         "&channel=Trans&DCS=" + Uri.EscapeUriString(DCS) + "&flashsms=0&number=" + Uri.EscapeUriString(MobileNos) +
                         "&text=" +SmsText + "&route=" + Uri.EscapeUriString(SmsRoute) + "";
                    break;
            }
            sms.ApiUrl = baseurl;
            SmSs.Add(sms);
        }
        public void Add_sms(string MobileNos, string SmsText, bool Unicode)
        {

            SMSObjs sms = new SMSObjs();
            string baseurl = "", DCS = "0";
            if (Unicode) DCS = "8";
            switch (UserCode)
            {

                default:
                    baseurl = "http://login.smsmantra.online/api/mt/SendSMS?user=" + Uri.EscapeUriString(SmsUser) +
                         "&password=" + Uri.EscapeUriString(SmsPass) + "&senderid=" + Uri.EscapeUriString(SmsSender) +
                         "&channel=Trans&DCS=" + Uri.EscapeUriString(DCS) + "&flashsms=0&number=" + Uri.EscapeUriString(MobileNos) +
                         "&text=" + SmsText + "&route=" + Uri.EscapeUriString(SmsRoute) + "";
                    break;
            }
            sms.ApiUrl = baseurl;
            SmSs.Add(sms);
        }

        public async Task<IEnumerable<UserDto>> Send_SMS_Bulk()
        {
            var users = new List<UserDto>();
            //var batchSize = 100;
            //int numberOfBatches = (int)Math.Ceiling((double)SmSs.Count() / batchSize);

            //for (int i = 0; i < numberOfBatches; i++)
            //{
            //var currentIds = SmSs.Skip(i * batchSize).Take(batchSize);
            try
            {
                var tasks = SmSs.Select(id => client.GetUser(id.ApiUrl));
                //users.AddRange(await Task.WhenAll(tasks));
            }
            catch (Exception err)
            {

            }
            //}

            return users;
        }


        public void getSmsSettings()
        {

            try
            {
                sgenFun sgen = new sgenFun(MyGuid);


                string mq = "select a.rec_id,a.client_name,a.master_id,a.master_name,a.master_entby as ent_by,a.master_entdate as ent_date,a.vch_date,a.status,a.master_editby edit_by,a.master_editdate as edit_date," +
                        "a.client_id,a.client_unit_id,a.cont_person_name,a.cont_person_email,a.group_refrence_number,a.group_id,to_char(convert_tz(Inactive_date,'+0:00','+05:30'),'YYYY-MM-dd HH24:MI:SS') " +
                        "from master_setting a where type='API' and client_id='" + Client_id + "' and client_unit_id='" + Unit_Id + "'";
                DataTable dt = sgen.getdata(UserCode, mq);
                if (dt.Rows.Count > 0)
                {
                    SmsUser = dt.Rows[0]["cont_person_name"].ToString();
                    SmsPass = dt.Rows[0]["cont_person_email"].ToString();
                    SmsSender = dt.Rows[0]["group_refrence_number"].ToString();
                    SmsRoute = dt.Rows[0]["group_id"].ToString();

                }

            }
            catch (Exception ex)
            {
                //Log Exception
            }


        }

    }
    public class UserDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime LastModified { get; set; }
    }
    public class UsersClient
    {
        private HttpClient client;
        

        public UsersClient()
        {
            client = new HttpClient();
        }

        public async Task<UserDto> GetUser(string ApiUrl)
        {
            UserDto user = null;
            //var user = null;
            try
            {
                var response = await client.GetAsync(
                    ApiUrl)
                    .ConfigureAwait(false);
                user = JsonConvert.DeserializeObject<UserDto>(await response.Content.ReadAsStringAsync());

            }
            catch (Exception err) { }
            return user;

        }
    }
}