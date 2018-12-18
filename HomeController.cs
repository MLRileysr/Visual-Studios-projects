using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;
using System.Runtime.ConstrainedExecution;
using System.Security;

namespace December2_2.Controllers
{
    public class HomeController : Controller
    {
        private string ConnMembers = ConfigurationManager.ConnectionStrings["ConnMembers"].ConnectionString;
        private string ConnYellow = ConfigurationManager.ConnectionStrings["ConnYellow"].ConnectionString;
        DataTable dtbl = new DataTable();

        public ActionResult Index()
        {
                string UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name; // Gives NT AUTHORITY\SYSTEM
          //      String UserName2 = Request.LogonUserIdentity.Name; // Gives NT AUTHORITY\SYSTEM
            ViewBag.USERid = "test Identity Name " + UserName;
            switchUsers();

            return View();
        }

        public ActionResult About()
        {

            ViewBag.Title = "Walking Fingers";
            ViewBag.Name = "Mike";
            string query = "SELECT * FROM  Member order by 'LastName','FullName'";
            fillDataTable(query, ConnMembers);
            List<string> list = new List<string>();
            List<string> listid = new List<string>();
            List<string> listFull = new List<string>();
            int z = 0;
            foreach (DataRow myProperty in dtbl.Rows)
            {
                string str = dtbl.Rows[z]["LastName"].ToString().Trim();
                string str1 = dtbl.Rows[z]["FullName"].ToString().Trim();
                string str2 = convtoImagloc(str1);
                string memid = dtbl.Rows[z]["Id"].ToString().Trim();
                list.Add(str2);  // + "  (" + str1 + ")  " + str2);
                listFull.Add(str);
                listid.Add(memid);
                z++;
            }
            ViewBag.Count = list.Count();
            ViewBag.NameList = list;
            ViewBag.NameFull = listFull;
            ViewBag.MembId = listid;
            return View();
        }

        public ActionResult Contact(string id)
        {
                string query = "SELECT * FROM Member where Id = '" + id + "'";
                fillDataTable(query, ConnMembers);
                ViewBag.FullName = dtbl.Rows[0]["FullName"].ToString().Trim();
                ViewBag.Addr = dtbl.Rows[0]["Address"].ToString().Trim();
                ViewBag.HPhone = dtbl.Rows[0]["HousePhone"].ToString().Trim();
                ViewBag.Pstatus = dtbl.Rows[0]["PersonalStatus"].ToString().Trim();
                string imgloc = dtbl.Rows[0]["FullName"].ToString().Trim();
                ViewBag.ImageLoc = convtoImagloc(imgloc);
                return View();
        }
        public ActionResult YellowPages()
        {
            string query = "SELECT * FROM Advertiser ORDER BY advHeading, adHeadingGroup";
            fillDataTable(query,ConnYellow);
            List<string> listName = new List<string>();
            List<string> listAddr = new List<string>();
            List<string> listImag = new List<string>();
            List<string> listHead = new List<string>();
            List<string> listheadGroup = new List<string>();
            int z = 0;
            foreach (DataRow myProperty in dtbl.Rows)
            {
                listImag.Add(convtoImagloc(dtbl.Rows[z]["adImageLocation"].ToString().Trim()));
                listHead.Add(dtbl.Rows[z]["advHeading"].ToString().Trim());
                listheadGroup.Add(dtbl.Rows[z]["adHeadingGroup"].ToString().Trim());
                listName.Add(dtbl.Rows[z]["advNAME"].ToString().Trim());
                listAddr.Add(dtbl.Rows[z]["advADDRESS"].ToString().Trim());
                z++;
            }
            ViewBag.NameList = listName;
            ViewBag.NameAddr = listAddr;
            ViewBag.ImgLocation = listImag;
            ViewBag.ListHead = listHead;
            ViewBag.ListheadGroup = listheadGroup;
            return View();
        }
        public string convtoImagloc(string fname)
        {
            string cvstr = fname.Replace(" ", "_");
            cvstr = cvstr.Replace("'", "");
            cvstr = cvstr.ToUpper();
            cvstr = cvstr.ToLower();
            if (cvstr.IndexOf(".") == -1)
                return "~/Pictures/" + cvstr + ".jpg";
            return "~/Pictures/" + cvstr;
        }

        public void fillDataTable(string Query,string connstring)
        {
            //  string query = "SELECT * FROM  Member order by 'LastName','FullName'";
            //  SqlConnection sqlcon = new SqlConnection("Data Source=MIKES-TOWER\\SQLEXPRESS;Initial Catalog=ChurchMembers;Integrated Security=True");
            SqlConnection sqlcon = new SqlConnection(connstring);
            if (sqlcon.State != ConnectionState.Open) sqlcon.Open();
            SqlCommand cmd = new SqlCommand(Query, sqlcon);
            dtbl.Load(cmd.ExecuteReader());
            sqlcon.Close();
            return;
        }
        private void switchUsers()
        {
            //  SafeTokenHandle safeTokenHandle;
            string OldUserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            WindowsIdentity newId = new WindowsIdentity("NT AUTHORITY//SYSTEM");
            WindowsImpersonationContext impersonatedUser = newId.Impersonate();
            string NewUserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        }
       
    }
}