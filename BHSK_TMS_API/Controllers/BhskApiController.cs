using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BHSK_TMS_API.ApplicationModel;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.IO;
using System.Web;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Xml;
using ExcelDataReader;
using System.Data.OleDb;
using System.Web.Http.Results;
using System.Web.Services.Description;

namespace BHSK_TMS_API.Controllers
{
    public class BhskApiController : ApiController
    {

        [Authorize]
        [Route("api/bhskapi/Authorize")]
        public IHttpActionResult Authorize()
        {
            return Ok("Authorized");
        }

        [Authorize]
        [Route("api/bhskapi/EmployeeDutyDateTimeUpdate")]
        public IHttpActionResult EmployeeDutyDateTimeUpdate(string EmployeeCode, string EmployeeName, DateTime DutyIn_Datetime, DateTime DutyOut_Datetime, float DutyIn_Latitude, float DutyIn_Longitude,float DutyOut_Latitude, float DutyOut_Longitude)
        {
            var Result1 = "";
            try
            {
                var result = DAL_AccessLayer.Employee_Duty_IN_OUT_Update(EmployeeCode, EmployeeName, DutyIn_Datetime, DutyOut_Datetime, DutyIn_Latitude, DutyIn_Longitude, DutyOut_Latitude, DutyOut_Longitude);
                if (result.StatusCode==1)
                    Result1 = "{Message:Successfully updated}";
                else
                    Result1 = "{Message:The request is invalid}";
            }
            catch
            {
                Result1 = "{Message:The request is invalid}";
            }
           
            return Ok(Result1);
        }
    
        [Authorize]
        [Route("api/bhskapi/DutyDateTimeUpdate")]
        public IHttpActionResult DutyDateTimeUpdate(JObject objData)
        {

            DutyINOUT_Info objList = new DutyINOUT_Info();

            ErrorMsg Err = new ErrorMsg();

            List<ApplicationModel.DutyTime> lstItemDetails = new List<ApplicationModel.DutyTime>();
            dynamic jsonData = objData;
            string Result = "";
            string Result1 = "";
            int chk = 0;

            JArray itemDetailsJson = jsonData.DutyOnOutList;

            foreach (var item in itemDetailsJson)
            {
                lstItemDetails.Add(item.ToObject<ApplicationModel.DutyTime>());
            }

            foreach (ApplicationModel.DutyTime itemDetail in lstItemDetails)
            {
                var result = DAL_AccessLayer.Update_DutyONOFF_Info(itemDetail);
                if (result.StatusCode == 1)
                {
                    Result = Result + "{Empcode: " + result.Empcode + ",Message:Successfully updated},";
                    chk = 0;
                }
                else
                {
                    Result1 = Result1 + "{Empcode: " + result.Empcode + ",Message:The request is invalid},";
                    chk = 1;
                }
            }

            if (Result != "")
            {
                Result = Result.Remove(Result.Length - 1, 1);
                Result = "{Response:[" + Result + "]}";
            }
            if (Result1 != "")
            {
                Result1 = Result1.Remove(Result1.Length - 1, 1);
                Result1 = "{Response:[" + Result1 + "]";
            }
            try
            {
                if (chk == 0)
                {
                    return Ok(Result);
                }
                else
                {
                    return BadRequest(Result1);
                }
            }
            catch
            {
                return BadRequest(Result1);
            }
        }
        [Route("api/bhskapi/Upload")]
        [HttpPost]
        public HttpResponseMessage Upload()
        {
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                var docfiles = new List<string>();
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var filePath = HttpContext.Current.Server.MapPath("~/" + postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    docfiles.Add(filePath);
                }
                result = Request.CreateResponse(HttpStatusCode.Created, docfiles);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return result;
        }

        [Route("api/bhskapi/UploadFiles")]
        [HttpPost]
        public HttpResponseMessage UploadFiles()
        {
            //Create the Directory.
            try
            {
                string path = HttpContext.Current.Server.MapPath("~/POD_Images/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //Fetch the File.
                HttpPostedFile postedFile = HttpContext.Current.Request.Files[0];

                string Str_Driv = HttpContext.Current.Request.Form["Str_Driv"];
                string Str_TripNo = HttpContext.Current.Request.Form["Str_TripNo"];
                string Str_JobNo = HttpContext.Current.Request.Form["Str_JobNo"];
                string fileName = HttpContext.Current.Request.Form["fileName"];
                string fType = HttpContext.Current.Request.Form["fType"];
                string Rev_Name = HttpContext.Current.Request.Form["Rev_Name"];
                string Str_Sts = HttpContext.Current.Request.Form["Str_Sts"];
                string Str_Lat = HttpContext.Current.Request.Form["Str_Lat"];
                string Str_Long = HttpContext.Current.Request.Form["Str_Long"];
                string Str_Loc = HttpContext.Current.Request.Form["Str_Loc"];
                string Str_PhotoSize = HttpContext.Current.Request.Form["Str_PhotoSize"]; 

                //Fetch the File Name.
                fileName = HttpContext.Current.Request.Form["fileName"] + Path.GetExtension(postedFile.FileName);

                //Save the File.
                postedFile.SaveAs(path + fileName);


                var result = DAL_AccessLayer.POD_Upload_Details_Insert(Str_Driv, Str_TripNo, Str_JobNo, fileName, fType, Rev_Name, Str_Sts, Str_Lat, Str_Long, Str_Loc, Str_PhotoSize);

                var ResponseMsg = "{recived:1,file_Name:" + fileName.ToString() + "}";

                //Send OK Response to Client.
                return Request.CreateResponse(HttpStatusCode.OK, ResponseMsg);
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "{recived:,file_Name:" + ex.Message + "}");
            }
        }

        [Authorize]
        [Route("api/bhskapi/LeaveList")]
        public IHttpActionResult LeaveList()
        {
            var Result1 = "{JobNo:101,Response:Unsuccessful},";
            return Ok(Result1);
        }

        [Authorize]
        [Route("api/bhskapi/Login")]
        public IHttpActionResult Login(string Userid,string Password)
        {
            var Result1 = "{JobNo:101,Response:Unsuccessful},";
            return Ok(Result1);
        }
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        public string Answer(string Question)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        [HttpGet]
        [Route("api/bhskapi/GetLogin")]
        public HttpResponseMessage GetLogin(string Userid, string Password)
        {
           
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            if (Userid =="test" && Password == "test")
            {
                response.StatusCode = HttpStatusCode.OK;
                response.ReasonPhrase = string.Format("Sucess");
                return response;

            }
            else
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.ReasonPhrase = string.Format("User info invalid");
                return response;
            }

        }
        
       
    //Astar API

        [Authorize]
        [HttpGet]
        [Route("api/bhskapi/jobrequests")]
        public IEnumerable<ApplicationModel.JobRequests> JobRequests(string targeted_pickup_datetime, string targeted_delivery_datetime, string current_job_status)
        {
            List<ApplicationModel.JobRequests> lstMilestoneDetails = new List<ApplicationModel.JobRequests>();
            
            var result = (dynamic)null;
            int chk = 0;

            if (targeted_pickup_datetime != "" && targeted_delivery_datetime != "" )
            {
                result = DAL_AccessLayer.Get_JobRequests_aStar(targeted_pickup_datetime, targeted_delivery_datetime, current_job_status).ToList();
            }
            return result;
        }
        [Authorize]
        [HttpGet]
        [Route("api/bhskapi/jobrequestswithoutparam")]
        public IEnumerable<ApplicationModel.JobRequests> JobRequestsWithoutparam()
        {
            List<ApplicationModel.JobRequests> lstMilestoneDetails = new List<ApplicationModel.JobRequests>();

            var result = (dynamic)null;
            int chk = 0;

            result = DAL_AccessLayer.Get_JobRequests_aStarWithoutPara().ToList();
            return result;
        }

        [Authorize]
        [HttpGet]
        [Route("api/bhskapi/primemovers")]
        public IEnumerable<ApplicationModel.PrimeMoversInfo> PrimeMovers(string Param1)
        {
            List<ApplicationModel.PrimeMoversInfo> lstMilestoneDetails = new List<ApplicationModel.PrimeMoversInfo>();

            var result = (dynamic)null;
            int chk = 0;

            if (Param1 != "")
            {
                result = DAL_AccessLayer.Get_PrimeMoversView_aStar(1, Param1).ToList();
            }
            return result;
        }
        [Authorize]
        [HttpGet]
        [Route("api/bhskapi/lorries")]
        public IEnumerable<ApplicationModel.LorryInfo> Lorries(string Param1)
        {
            List<ApplicationModel.LorryInfo> lstMilestoneDetails = new List<ApplicationModel.LorryInfo>();

            var result = (dynamic)null;
            int chk = 0;

            if (Param1 != "")
            {
                result = DAL_AccessLayer.Get_LorryView_aStar(2, Param1).ToList();
            }
            return result;
        }
        [Authorize]
        [HttpGet]
        [Route("api/bhskapi/trailers")]
        public IEnumerable<ApplicationModel.TrailerInfo> Trailers(string Param1)
        {
            List<ApplicationModel.TrailerInfo> lstMilestoneDetails = new List<ApplicationModel.TrailerInfo>();

            var result = (dynamic)null;
        
            if (Param1 != "")
            {
                result = DAL_AccessLayer.Get_TrailerView_aStar(3, Param1).ToList();
            }
            return result;
        }
        [Authorize]
        [HttpGet]
        [Route("api/bhskapi/drivers")]
        public IEnumerable<ApplicationModel.DriversInfo> Drivers(string Param1)
        {
            List<ApplicationModel.DriversInfo> lstMilestoneDetails = new List<ApplicationModel.DriversInfo>();

            var result = (dynamic)null;
          
            if (Param1 != "")
            {
                result = DAL_AccessLayer.Get_DriversView_aStar(4, Param1).ToList();
            }
            return result;
        }

        [Authorize]
        [HttpGet]
        [Route("api/bhskapi/customers")]
        public IEnumerable<ApplicationModel.CustomerInfo> Customers(string Param1)
        {
            List<ApplicationModel.CustomerInfo> lstMilestoneDetails = new List<ApplicationModel.CustomerInfo>();

            var result = (dynamic)null;
         
            if (Param1 != "")
            {
                result = DAL_AccessLayer.Get_CustomersView_aStar(5, Param1).ToList();
            }
            return result;
        }


        [Authorize]
        [HttpGet]
        [Route("api/bhskapi/ts_job_status_counts")]
        public IEnumerable<ApplicationModel.JobStatusActivities> ts_job_status_counts(string startTime,string endTime,string workgroup)
        {
            List<ApplicationModel.JobStatusActivities> lstMilestoneDetails = new List<ApplicationModel.JobStatusActivities>();

            var result = (dynamic)null;
        
            if (startTime != "" && endTime != "" && workgroup != "")
            {
                result = DAL_AccessLayer.Get_Jobactivities_aStar(startTime, endTime, workgroup).ToList();
            }
            return result;
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        //UMC

        [Authorize]
        [HttpGet]
        [Route("api/bhskapi/getuserinfo")]
        public IEnumerable<ApplicationModel.UserInfo> getuserinfo()
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            var UserId = identity.Name;
            
            var result = (dynamic)null;
            if (UserId != "" )
            {
                result = DAL_AccessLayer.UserList(UserId, "", "", "", "", 0, 1);
            }
            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("api/bhskapi/resetpassword")]
        public IHttpActionResult resetpassword(string Password)
        {
            var Result1 = (dynamic)null;
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            var Userid = identity.Name;

            try
            {
                var result = (dynamic)null;
                var encrptPw = Convert.ToString(Encrypt_Dll.EncryptDLL.enCrypt(Password));
                if (Userid != "" && Password != "")
                {
                    result = DAL_AccessLayer.Change_Reset_Password(Userid, encrptPw,"",1);
                    if (result.StatusCode == 1)
                        Result1 = "{ErrCode:1,ErrMsg:Password Successfully Reseted}";
                    else
                        Result1 = "{ErrCode:-1,ErrMsg:User information is invalid}";
                }
                
            }
            catch
            {
                Result1 = "{ErrCode:-1,ErrMsg:User information is invalid}";
            }

            return Ok(Result1);
        }

        [Authorize]
        [HttpPost]
        [Route("api/bhskapi/forgotpassword")]
        public IHttpActionResult forgotpassword(string Userid)
        {
            var Result1 = (dynamic)null;
           // var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
           // var Userid = identity.Name;

            try
            {
                var TempPassword = "";
                TempPassword = CreateRandomPassword(8);
                string TempEncryptPwd = Convert.ToString(Encrypt_Dll.EncryptDLL.enCrypt(TempPassword));

                var result = (dynamic)null;
                 if (Userid != "" && TempEncryptPwd != "")
                {
                    result = DAL_AccessLayer.ForgotPasswordVerification(Userid, TempPassword, TempEncryptPwd);
                    if (result.ErrCode == 0)
                        Result1 = "{ErrCode:1,ErrMsg:" + result.ErrMsg + "}";
                    else
                        Result1 = "{ErrCode:-1,ErrMsg:" + result.ErrMsg + "}";
                }

            }
            catch
            {
                Result1 = "{ErrCode:-1,ErrMsg:User information is invalid}";
            }

            return Ok(Result1);
        }
        public static string CreateRandomPassword(int PasswordLength)
        {
            string _allowedChars = "0123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ!@#$%&*";
            Random randNum = new Random();
            char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length;
            for (int i = 0; i < PasswordLength; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }
            return new string(chars);
        }

        [Authorize]
        [HttpPost]
        [Route("api/bhskapi/changepassword")]
        public IHttpActionResult changepassword(string OldPassword, string NewPassword)
        {
            var Result1 = (dynamic)null;
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            var Userid = identity.Name;
            try
            {
                var result = (dynamic)null;
                var encrptPw1 = Convert.ToString(Encrypt_Dll.EncryptDLL.enCrypt(OldPassword));
                var encrptPw2 = Convert.ToString(Encrypt_Dll.EncryptDLL.enCrypt(NewPassword));
                if (Userid != "" && OldPassword != "" && NewPassword != "")
                {
                    result = DAL_AccessLayer.Change_Reset_Password(Userid, encrptPw1, encrptPw2 ,2);
                    if (result.StatusCode == 1)
                        Result1 = "{ErrCode:1,ErrMsg:" + result.ErrMsg +"}";
                    else
                        Result1 = "{ErrCode:-1,ErrMsg:" + result.ErrMsg + "}";
                }
            }
            catch
            {
                Result1 = "{ErrCode:-1,ErrMsg:User information is invalid}";
            }

            return Ok(Result1);
        }
       

        [Authorize]
        [HttpPost]
        [Route("api/bhskapi/addusers")]
        public HttpResponseMessage addusers([FromBody] UserInfoAdd UserInfoAdd)
        {
            List<UserInfoAdd> lsusers = new List<UserInfoAdd>();
            var Result1 ="";
            try
            {
                string UserName = UserInfoAdd.UserName;
                string password = UserInfoAdd.Password;
                string Role = UserInfoAdd.Role;
                string EmailId = UserInfoAdd.EmailId;
                string Contact = UserInfoAdd.Contact;
                string Workgroup = UserInfoAdd.Workgroup;
                var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
                var UserId = identity.Name;
                var encrptPw1 = Convert.ToString(Encrypt_Dll.EncryptDLL.enCrypt(password));
                var result = DAL_AccessLayer.UserInfo_Add(UserName, encrptPw1, Role, EmailId, Contact, Workgroup, UserId);
                if (result.StatusCode == 1)
                    Result1 = "{ErrCode:1,ErrMsg:" + result.ErrMsg + "}";
                else
                    Result1 = "{ErrCode:-1,ErrMsg:" + result.ErrMsg + "}";
                
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, Result1);
        }

        [Authorize]
        [HttpGet]
        [Route("api/bhskapi/getuserlist")]
        public IEnumerable<ApplicationModel.UserInfo> getUserList(string UserName,string EmailId,string Contact,string Workgroup,string Search_keyword,int Page)
        {

            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            var Userid = identity.Name;
            List<ApplicationModel.UserInfo> lstUserList = new List<ApplicationModel.UserInfo>();
            var result = (dynamic)null;
            if (Userid != "" )
            {
                result = DAL_AccessLayer.UserList(UserName, EmailId, Contact, Workgroup, Search_keyword, Page,2);
            }
            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("api/bhskapi/uploadshipmentattachments")]
        public string UploadShipmentAttachments(int ShipmentId)
        {
            var path = "";
            var result = (dynamic)null;
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            var Userid = identity.Name;
            try
            {
                string message = "";
                HttpResponseMessage ResponseMessage = null;
                var httpRequest = HttpContext.Current.Request;
                HttpPostedFile Inputfile = null;
                Stream FileStream = null;
                if (httpRequest.Files.Count > 0 && httpRequest.Files[0].FileName.ToString() != "")
                {
                    for (int f=0;f<= httpRequest.Files.Count-1;f++)
                    {
                        Inputfile = httpRequest.Files[f];
                        FileStream = Inputfile.InputStream;

                        path = HttpContext.Current.Server.MapPath("~/Attachments/" + Inputfile.FileName);
                        Inputfile.SaveAs(path);

                        if (Userid != "")
                        {
                            result = DAL_AccessLayer.UploadAttachmentDetails(ShipmentId, Inputfile.FileName, Userid);
                        }
                    }
                    if (result.StatusCode == 1)
                    {
                        message = "The documents has been successfully uploaded.";
                    }
                    else
                    {
                        message = "Something Went Wrong!, The Excel file uploaded has fiald.";
                    }

                }
                else
                {
                    message = "File does not selected!, Please check it.";
                }
               
                return message;
            }
            catch (Exception)
            {
                throw;
            }

        }

        [Authorize]
        [HttpPost]
        [Route("api/bhskapi/uploadtoolsdetails")]
        public string UploadToolsDetails()
        {
            ExcelDataReader.IExcelDataReader reader = null;
            var path = "";
            int intNewRec = 0;
            int intUptRec = 0;
            int intFail = 0;
            int intRows = 0;
            var result = (dynamic)null;
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            var Userid = identity.Name;
            try
            {
                string message = "";
                HttpResponseMessage ResponseMessage = null;
                var httpRequest = HttpContext.Current.Request;
                HttpPostedFile Inputfile = null;
                Stream FileStream = null;
                string conStr = "";
                string RowErrors = "";
                DataSet dsexcelRecords1 = new DataSet();
                if (httpRequest.Files.Count > 0 && httpRequest.Files[0].FileName.ToString() !="")
                {
                    Inputfile = httpRequest.Files[0];
                    FileStream = Inputfile.InputStream;

                    path = HttpContext.Current.Server.MapPath("~/Excels/" + Inputfile.FileName);
                    Inputfile.SaveAs(path);

                    if (Inputfile != null && FileStream != null)
                    {
                        if (Inputfile.FileName.EndsWith(".xls"))
                        {
                            reader = ExcelDataReader.ExcelReaderFactory.CreateBinaryReader(FileStream);
                            conStr = System.Configuration.ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        }
                        else if (Inputfile.FileName.EndsWith(".xlsx"))
                        {
                            reader = ExcelDataReader.ExcelReaderFactory.CreateOpenXmlReader(FileStream);
                            conStr = System.Configuration.ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                        }
                        else
                        {
                            message = "The file format is not supported.";
                            var result1 = DAL_AccessLayer.Import_DetailsUpdate(intNewRec, intUptRec, intFail, 2, message, 0, Userid);
                        }
                            
                        if(conStr !="")
                        {
                            conStr = String.Format(conStr, path, "YES");
                            OleDbConnection connExcel = new OleDbConnection(conStr);
                            OleDbCommand cmdExcel = new OleDbCommand();
                            OleDbDataAdapter oda = new OleDbDataAdapter();
                            DataTable dsexcelRecords = new DataTable();

                            
                            cmdExcel.Connection = connExcel;
                            //Get the name of First Sheet
                            connExcel.Open();
                            DataTable dtExcelSchema;
                            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                            connExcel.Close();

                            //Read Data from First Sheet
                            connExcel.Open();
                            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
                            oda.SelectCommand = cmdExcel;
                            oda.Fill(dsexcelRecords);
                            connExcel.Close();

                            Inputfile.InputStream.Dispose();
                            Inputfile.InputStream.Close();

                            //DataSet ds = new DataSet();
                            //ds = reader.AsDataSet();
                            //reader.Close();

                            //DataTable dsexcelRecords = ds.Tables[0];
                            int RowNum = 0;
                            int ChkRow = 0;
                            if (dsexcelRecords != null && dsexcelRecords.Rows.Count > 0)
                            {
                                for (int row = 0; row < dsexcelRecords.Rows.Count; row++)
                                {
                                    ShipmentDetails objShipment = new ShipmentDetails();
                                    string Remarks = "";
                                    string Area = "";
                                    string Entity = "";
                                    string EQPID = "";
                                    string Vendor = "";
                                    string Model = "";
                                    string TradeTerm = "";
                                    string Country = "";
                                    string Mode = "";
                                    string MIDate = "";
                                    string FCADate = "";
                                    string TempContol = "0";
                                    string Humidity = "0";
                                    string M3Val1 = "";
                                    string M3Val2 = "";
                                    string M3Val3 = "";
                                    string Permit = "0";
                                    string Esscorts = "0";
                                    string Forwarder = "0";
                                    string Status = "";

                                    try
                                    {
                                        for (int col = 0; col < dsexcelRecords.Columns.Count; col++)
                                        {
                                            string colname = dsexcelRecords.Columns[col].ColumnName;
                                            if (dsexcelRecords.Rows[row][col].ToString() != "")
                                            {
                                                if (colname == "Remark" || colname == "Column0" || colname == "Type of Tools")
                                                {
                                                    Remarks = dsexcelRecords.Rows[row][col].ToString();
                                                    ChkRow = 1;
                                                }                                                   
                                                else if (colname == "Area" || colname == "Column1")
                                                {
                                                    Area = dsexcelRecords.Rows[row][col].ToString();
                                                    ChkRow = 1;
                                                }
                                                else if (colname == "Entity" || colname == "Column2" || colname == "Type - SAP")
                                                {
                                                    Entity = dsexcelRecords.Rows[row][col].ToString();
                                                    ChkRow = 1;
                                                }                                                   
                                                else if (colname == "EQPID" || colname == "Column3" || colname == "UMC EQID")
                                                {
                                                    EQPID = dsexcelRecords.Rows[row][col].ToString();
                                                    ChkRow = 1;
                                                }                                                    
                                                else if (colname == "Vendor" || colname == "Column4")
                                                {
                                                    Vendor = dsexcelRecords.Rows[row][col].ToString();
                                                    ChkRow = 1;
                                                }                                                    
                                                else if (colname == "Model" || colname == "Column5" || colname == "Tool Model")
                                                    Model = dsexcelRecords.Rows[row][col].ToString();
                                                else if (colname == "Trade Term" || colname == "Column6" || colname == "Incoterm")
                                                    TradeTerm = dsexcelRecords.Rows[row][col].ToString();
                                                else if (colname == "Country" || colname == "Column7")
                                                    Country = dsexcelRecords.Rows[row][col].ToString();
                                                else if (colname == "Mode" || colname == "Column8")
                                                    Mode = dsexcelRecords.Rows[row][col].ToString();
                                                else if (colname == "M / I Date\n(12 / 21)" || colname == "M/I Date_(12/21)" || colname == "Column9"  || colname == "Move in Date")
                                                    MIDate = dsexcelRecords.Rows[row][col].ToString();
                                                else if (colname == "FCA Date\n(12 / 21)" || colname == "FCA Date_(12/21)" || colname == "Column10" || colname== "FCA Date 2")
                                                    FCADate = dsexcelRecords.Rows[row][col].ToString();
                                                else if (colname == "Temp Contol" || colname == "Column11")
                                                    TempContol = dsexcelRecords.Rows[row][col].ToString();
                                                else if (colname == "Humidity Contol" || colname == "Column12")
                                                    Humidity = dsexcelRecords.Rows[row][col].ToString();
                                                else if (colname == "> 56m3" || colname == "Column13")
                                                    M3Val1 = dsexcelRecords.Rows[row][col].ToString();
                                                else if (colname == "21-55m3" || colname == "Column14")
                                                    M3Val2 = dsexcelRecords.Rows[row][col].ToString();
                                                else if (colname == "< 20m3" || colname == "Column15")
                                                    M3Val3 = dsexcelRecords.Rows[row][col].ToString();
                                                else if (colname == "Need Permit?" || colname == "Column16")
                                                    Permit = dsexcelRecords.Rows[row][col].ToString();
                                                else if (colname == "Need Police Esscorts?" || colname == "Column18")
                                                    Esscorts = dsexcelRecords.Rows[row][col].ToString();
                                                else if (colname == "Appointed Forwarder" || colname == "Column19")
                                                    Forwarder = dsexcelRecords.Rows[row][col].ToString();
                                                else if (colname == "Status" || colname == "Column20")
                                                    Status = dsexcelRecords.Rows[row][col].ToString();
                                                if (TempContol == "")
                                                    TempContol = "0";
                                                if (Humidity == "")
                                                    Humidity = "0";
                                                if (Permit == "")
                                                    Permit = "0";
                                                if (Esscorts == "")
                                                    Esscorts = "0";
                                                if (Forwarder == "")
                                                    Forwarder = "0";
                                            }

                                        }
                                        if (ChkRow==1)
                                        {
                                            RowNum = RowNum + 1;
                                            string inputString = EQPID + "," + EQPID + "," + Vendor + "," + Entity + "," + Area + "," + Model + "," + MIDate + "," + FCADate + "," + Remarks + "," + TradeTerm + "," + Country + "," + Mode + "," + TempContol + "," + Humidity + "," + M3Val1 + "," + M3Val2 + "," + M3Val3 + "," + Permit + "," + Esscorts + "," + Forwarder + "," + Status + "," + Userid;
                                            result = DAL_AccessLayer.ShipmentInfo_Add(EQPID, EQPID, Vendor, Entity, Area, Model, MIDate, FCADate, Remarks, TradeTerm, Country, Mode, TempContol, Humidity, M3Val1, M3Val2, M3Val3, Permit, Esscorts, Forwarder, Status, RowNum, Userid);
                                            if (result.Record_Status == 1)
                                                intNewRec = intNewRec + 1;
                                            if (result.Record_Status == 2)
                                                intUptRec = intUptRec + 1;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        string err = ex.Message;
                                        if (err.Contains("bit"))
                                        {
                                            RowErrors = "Data Converson Errors: Data type :data type bit";
                                        }
                                        else if (err.Contains("Date"))
                                        {
                                            RowErrors = "Data Converson Errors: Data type :data type Datetime";
                                        }
                                        else
                                        {
                                            RowErrors = "Data Converson Errors";
                                        }

                                        //var result1 = DAL_AccessLayer.Import_DetailsUpdate(intNewRec, intUptRec, intFail, 2, RowErrors, RowNum, Userid);
                                        intFail = intFail + 1;
                                    }
                                }

                                if (result.StatusCode == 1)
                                {

                                    //if (System.IO.File.Exists(path))
                                    //{
                                    //    System.IO.File.Delete(path);
                                    //}
                                    dtExcelSchema.Dispose();

                                    var result1 = DAL_AccessLayer.Import_DetailsUpdate(intNewRec, intUptRec, intFail, 1, RowErrors, 0, Userid);
                                    message = "The Excel file has been successfully uploaded.";

                                    FileStream.Close();
                                    reader.Dispose();
                                    reader.Close();
                                }
                                else
                                {
                                    message = "Something Went Wrong!, The Excel file uploaded has fiald.";
                                    var result1 = DAL_AccessLayer.Import_DetailsUpdate(intNewRec, intUptRec, intFail, 2, message, 0, Userid);
                                }

                            }
                            else
                            {
                                message = "Selected file is empty.";
                                var result1 = DAL_AccessLayer.Import_DetailsUpdate(intNewRec, intUptRec, intFail, 2, message, 0, Userid);
                            }
                        }
                    }
                    else
                    {
                        message = "Invalid File.";
                        var result1 = DAL_AccessLayer.Import_DetailsUpdate(intNewRec, intUptRec, intFail, 2, message, 0, Userid);
                    }
                       
                }
                else
                {
                    message = "File does not selected!, Please check it.";
                    var result1 = DAL_AccessLayer.Import_DetailsUpdate(intNewRec, intUptRec, intFail, 2, message, 0, Userid);
                    ResponseMessage = Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                  
               return message;

            }
            catch (Exception)
            {
               
                //if (System.IO.File.Exists(path))
                //{
                //    System.IO.File.Delete(path);
                //}
                throw;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("api/bhskapi/gettoolslist")]
        public IEnumerable<ApplicationModel.MainToolsList> gettoolslist(string Area, string Vendor, string Fromdate, string Todate, string Search_keyword, int Page)
        {

            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            var Userid = identity.Name;
            var result = (dynamic)null;
            if (Userid != "")
            {
                result = DAL_AccessLayer.GetMainToolsList(Area, Vendor, Fromdate, Todate, Search_keyword, Page, 1);
            }
            return result;
        }
        [Authorize]
        [HttpGet]
        [Route("api/bhskapi/getexporttoolslist")]
        public IEnumerable<ApplicationModel.MainToolsList> getexporttoolslist(string Area, string Vendor, string Fromdate, string Todate, string Search_keyword)
        {

            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            var Userid = identity.Name;
            var result = (dynamic)null;
            if (Userid != "")
            {
                result = DAL_AccessLayer.GetMainToolsList(Area, Vendor, Fromdate, Todate, Search_keyword, 0, 2);
            }
            return result;
        }

        [Authorize]
        [HttpGet]
        [Route("api/bhskapi/getimportdetails")]
        public IEnumerable<ApplicationModel.ImportDetails> getimportdetails(int Page, string Search_keyword)
        {

            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            var Userid = identity.Name;
            var result = (dynamic)null;
            if (Userid != "")
            {
                result = DAL_AccessLayer.GetImportDetails(Page, Search_keyword, 1);
            }
            return result;
        }

        [Authorize]
        [HttpGet]
        [Route("api/bhskapi/getImporterrordetails")]
        public IEnumerable<ApplicationModel.ImportErrorDetails> getImporterrordetails(int ImportId,int Page)
        {

            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            var Userid = identity.Name;
            var result = (dynamic)null;
            if (Userid != "")
            {
                result = DAL_AccessLayer.GetImportErrorDetails(ImportId, Page, 2);
            }
            return result;
        }

        [Authorize]
        [HttpGet]
        [Route("api/bhskapi/getImportUpdatedetails")]
        public IEnumerable<ApplicationModel.ImportUpdateDetails> getImportUpdatedetails(int ImportId, int Page)
        {

            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            var Userid = identity.Name;
            var result = (dynamic)null;
            if (Userid != "")
            {
                result = DAL_AccessLayer.GetImportUpdateDetails(ImportId, Page, 3);
            }
            return result;
        }

        [Authorize]
        [HttpGet]
        [Route("api/bhskapi/getshipmentdetailslist")]
        public IEnumerable<ApplicationModel.ShipmentListDetails> getshipmentdetailslist(string Eqpid, string TradeTerm, string Country, string Mode, string Search_keyword, int Page)
        {

            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            var Userid = identity.Name;
            var result = (dynamic)null;
            if (Userid != "")
            {
                result = DAL_AccessLayer.GetShipmentDetailsList(Eqpid, TradeTerm, Country, Mode, Search_keyword, Page, 1);
            }
            return result;
        }
        [Authorize]
        [HttpGet]
        [Route("api/bhskapi/getexportshipmentdetailslist")]
        public IEnumerable<ApplicationModel.ShipmentListDetails> getexportshipmentdetailslist(string Eqpid, string TradeTerm, string Country, string Mode, string Search_keyword)
        {

            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            var Userid = identity.Name;
            var result = (dynamic)null;
            if (Userid != "")
            {
                result = DAL_AccessLayer.GetShipmentDetailsList(Eqpid, TradeTerm, Country, Mode, Search_keyword, 0, 2);
            }
            return result;
        }

        [Authorize]
        [Route("api/bhskapi/Shipments")]
        public IHttpActionResult Shipments(JObject objData)
        {
            DutyINOUT_Info objList = new DutyINOUT_Info();
            ErrorMsg Err = new ErrorMsg();
            List<ApplicationModel.DutyTime> lstItemDetails = new List<ApplicationModel.DutyTime>();
            dynamic jsonData = objData;
            string Result = "";
            string Result1 = "";
            int chk = 0;

            JArray itemDetailsJson = jsonData.DutyOnOutList;

            foreach (var item in itemDetailsJson)
            {
                lstItemDetails.Add(item.ToObject<ApplicationModel.DutyTime>());
            }

            foreach (ApplicationModel.DutyTime itemDetail in lstItemDetails)
            {
                var result = DAL_AccessLayer.Update_DutyONOFF_Info(itemDetail);
                if (result.StatusCode == 1)
                {
                    Result = Result + "{Empcode: " + result.Empcode + ",Message:Successfully updated},";
                    chk = 0;
                }
                else
                {
                    Result1 = Result1 + "{Empcode: " + result.Empcode + ",Message:The request is invalid},";
                    chk = 1;
                }
            }

            if (Result != "")
            {
                Result = Result.Remove(Result.Length - 1, 1);
                Result = "{Response:[" + Result + "]}";
            }
            if (Result1 != "")
            {
                Result1 = Result1.Remove(Result1.Length - 1, 1);
                Result1 = "{Response:[" + Result1 + "]";
            }
            try
            {
                if (chk == 0)
                {
                    return Ok(Result);
                }
                else
                {
                    return BadRequest(Result1);
                }
            }
            catch
            {
                return BadRequest(Result1);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("api/bhskapi/addconfiguration")]
        public HttpResponseMessage addConfiguration(string name,string type,string value)
        {
     
            var Result1 = "";
            try
            {
               
                var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
                var UserId = identity.Name;
            
                var result = DAL_AccessLayer.Configuration_Add_Update(0, name, type, value,1,UserId);
                if (result.StatusCode == 1)
                    Result1 = "{ErrCode:1,ErrMsg:" + result.ErrMsg + "}";
                else
                    Result1 = "{ErrCode:-1,ErrMsg:" + result.ErrMsg + "}";

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, Result1);
        }
        [Authorize]
        [HttpPost]
        [Route("api/bhskapi/updateconfiguration")]
        public HttpResponseMessage UpdateConfiguration(int id,string name, string type, string value)
        {

            var Result1 = "";
            try
            {

                var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
                var UserId = identity.Name;

                var result = DAL_AccessLayer.Configuration_Add_Update(id, name, type, value, 2, UserId);
                if (result.StatusCode == 1)
                    Result1 = "{ErrCode:1,ErrMsg:" + result.ErrMsg + "}";
                else
                    Result1 = "{ErrCode:-1,ErrMsg:" + result.ErrMsg + "}";

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, Result1);
        }

        [Authorize]
        [HttpPost]
        [Route("api/bhskapi/addshipmentdetails")]
        public string Addshipmentdetails(string EQPID, string TradeTerm, string Country, string Forwarder, bool Temperature, bool Humidity, bool Permit, bool Escort, string Mode, int TotalArea, int Num_Crates, int TotalVolume, DateTime Pickup_Planned, DateTime Pickup_Actual, string FlightVesselNumber, string AirShippingLine, DateTime FlightVessel_ETD, DateTime FlightVessel_ATD, string Transit, DateTime Transit_ETA, DateTime Transit_ATA, DateTime Transit_ETD, DateTime Transit_ATD, DateTime Planned_SG_Arrival, bool Confirm_SG_Arrival, DateTime Actual_SG_Arrival, bool DocumentReady, bool CargoReady, bool Delayed, string DelayedReason, bool Shock_Watch_Activated)
        {
            String path;
            var result = (dynamic)null;
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            var Userid = identity.Name;
            try
            {
                string message;
                var httpRequest = HttpContext.Current.Request;
                HttpPostedFile Inputfile = null;
                Stream FileStream = null;
             
                if (httpRequest.Files.Count > 0 && httpRequest.Files[0].FileName.ToString() != "")
                {
                    string[] filenames = new string[httpRequest.Files.Count];
                    for (int f = 0; f <= httpRequest.Files.Count - 1; f++)
                    {
                        Inputfile = httpRequest.Files[f];
                        FileStream = Inputfile.InputStream;

                        path = HttpContext.Current.Server.MapPath("~/Attachments/" + Inputfile.FileName);
                        Inputfile.SaveAs(path);

                        filenames[f] = Inputfile.FileName;
                    }
                    if (Userid != "")
                    {
                        result = DAL_AccessLayer.ShipmentInfo_CreateNew(EQPID, TradeTerm, Country, Forwarder, Temperature, Humidity, Permit, Escort, Mode, TotalArea, Num_Crates, TotalVolume, Pickup_Planned, Pickup_Actual, FlightVesselNumber, AirShippingLine, FlightVessel_ETD, FlightVessel_ATD, Transit, Transit_ETA, Transit_ATA, Transit_ETD, Transit_ATD, Planned_SG_Arrival, Confirm_SG_Arrival, Actual_SG_Arrival, DocumentReady, CargoReady, Delayed, DelayedReason, Shock_Watch_Activated, filenames, Userid);
                    }
                    if (result.StatusCode == 1)
                    {
                        message = "The documents has been successfully uploaded.";
                    }
                    else
                    {
                        message = "Something Went Wrong!, The Excel file uploaded has fiald.";
                    }

                }
                else
                {
                    message = "File does not selected!, Please check it.";
                }

                return message;
            }
            catch (Exception)
            {
                throw;
            }

           
        }

        [Authorize]
        [HttpPost]
        [Route("api/bhskapi/updateshipmentdetails")]
        public string  UpdateShipmentDetails(int ShipmentID, string TradeTerm, string Country, string Forwarder, bool Temperature, bool Humidity, bool Permit, bool Escort, string Mode, int TotalArea, int Num_Crates, int TotalVolume, DateTime Pickup_Planned, DateTime Pickup_Actual, string FlightVesselNumber, string AirShippingLine, DateTime FlightVessel_ETD, DateTime FlightVessel_ATD, string Transit, DateTime Transit_ETA, DateTime Transit_ATA, DateTime Transit_ETD, DateTime Transit_ATD, DateTime Planned_SG_Arrival, bool Confirm_SG_Arrival, DateTime Actual_SG_Arrival, bool DocumentReady, bool CargoReady, bool Delayed, string DelayedReason, bool Shock_Watch_Activated)
        {
            String path;
            var result = (dynamic)null;
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            var UserId = identity.Name;
            try
            {
                string message;
                var httpRequest = HttpContext.Current.Request;
                HttpPostedFile Inputfile = null;
                Stream FileStream = null;

                if (httpRequest.Files.Count > 0 && httpRequest.Files[0].FileName.ToString() != "")
                {
                    string[] filenames = new string[httpRequest.Files.Count];
                    for (int f = 0; f <= httpRequest.Files.Count - 1; f++)
                    {
                        Inputfile = httpRequest.Files[f];
                        FileStream = Inputfile.InputStream;

                        path = HttpContext.Current.Server.MapPath("~/Attachments/" + Inputfile.FileName);
                        Inputfile.SaveAs(path);

                        filenames[f] = Inputfile.FileName;
                    }
                    if (UserId != "")
                    {
                        result = DAL_AccessLayer.ShipmentInfo_Update(ShipmentID, TradeTerm, Country, Forwarder, Temperature, Humidity, Permit, Escort, Mode, TotalArea, Num_Crates, TotalVolume, Pickup_Planned, Pickup_Actual, FlightVesselNumber, AirShippingLine, FlightVessel_ETD, FlightVessel_ATD, Transit, Transit_ETA, Transit_ATA, Transit_ETD, Transit_ATD, Planned_SG_Arrival, Confirm_SG_Arrival, Actual_SG_Arrival, DocumentReady, CargoReady, Delayed, DelayedReason, Shock_Watch_Activated, filenames, UserId);
                        //result = DAL_AccessLayer.ShipmentInfo_CreateNew(EQPID, TradeTerm, Country, Forwarder, Temperature, Humidity, Permit, Escort, Mode, TotalArea, Num_Crates, TotalVolume, Pickup_Planned, Pickup_Actual, FlightVesselNumber, AirShippingLine, FlightVessel_ETD, FlightVessel_ATD, Transit, Transit_ETA, Transit_ATA, Transit_ETD, Transit_ATD, Planned_SG_Arrival, Confirm_SG_Arrival, Actual_SG_Arrival, DocumentReady, CargoReady, Delayed, DelayedReason, Shock_Watch_Activated, filenames, Userid);
                    }
                    if (result.StatusCode == 1)
                    {
                        message = "The documents has been successfully uploaded.";
                    }
                    else
                    {
                        message = "Something Went Wrong!, The Excel file uploaded has fiald.";
                    }

                }
                else
                {
                    message = "File does not selected!, Please check it.";
                }

                return message;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("api/bhskapi/getconfigurationdetails")]
 
        public IEnumerable<ApplicationModel.ConfigurationDetails> getConfigurationDetails(string Name, string Type, string Value, string Search_keyword, int Page)
        {

            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            var Userid = identity.Name;
            List<ApplicationModel.UserInfo> lstUserList = new List<ApplicationModel.UserInfo>();
            var result = (dynamic)null;
            if (Userid != "")
            {
                result = DAL_AccessLayer.ConfigurationList(Name, Type, Value, Search_keyword, Page, 2);
            }
            return result;
        }
        //Gps
        [Route("api/bhskapi/GPSPush")]
        [HttpPost]
        public IHttpActionResult GPSPush([FromBody] JObject jsonData)
        {
            try
            {
                var jsonLinq = jsonData;

                StreamWriter sw2 = new StreamWriter("D:\\mGPS\\gpsdata3.txt");
                //Write a line of text
                sw2.WriteLine("============");
                //Write a second line of text
                sw2.WriteLine(jsonData.ToString());

                sw2.Close();

                dynamic jsonObject = JsonConvert.DeserializeObject(jsonData.ToString());

                // Creating an XmlDocument and adding root element
                XmlDocument xmlDocument = new XmlDocument();
                XmlNode rootNode = xmlDocument.CreateElement("Root");
                xmlDocument.AppendChild(rootNode);

                // Adding elements and attributes from JSON data
                foreach (var property in jsonObject)
                {
                    XmlNode node = xmlDocument.CreateElement(property.Name);
                    node.InnerText = property.Value.ToString();
                    rootNode.AppendChild(node);
                }

                // Output the XML
                string xmlOutput = xmlDocument.OuterXml;

                XmlNodeList xmlEqnode;
                xmlEqnode = xmlDocument.GetElementsByTagName("Root");
                string typeWebhook = "";
                string instanceData = "";
                string senderData = "";
                string timestamp = "";
                string idMessage = "";
                string messageData = "";

                string idInstance = "", wid = "", chatId = "", chatName = "", typeInstance = "", sender = "", senderName = "", typeMessage = "", textMessage = "";
                string text = "", description = "", title = "";

                for (int i = 0; i <= xmlEqnode.Count - 1; i++)
                {
                    string EqNo = xmlEqnode[i].ChildNodes.Item(0).InnerText.Trim();
                    for (int j = 0; j <= xmlEqnode[i].ChildNodes.Count - 1; j++)
                    {
                        if (xmlEqnode[i].ChildNodes.Item(j).Name.Trim() == "typeWebhook")
                        {
                            typeWebhook = xmlEqnode[i].ChildNodes.Item(j).InnerText.Trim();
                        }
                        else if (xmlEqnode[i].ChildNodes.Item(j).Name.Trim() == "instanceData")
                        {
                            instanceData = xmlEqnode[i].ChildNodes.Item(j).InnerText.Trim();
                            dynamic jsonObject1 = JsonConvert.DeserializeObject(instanceData.ToString());


                            XmlDocument xmlDocument1 = new XmlDocument();
                            XmlNode rootNode1 = xmlDocument1.CreateElement("instanceData");
                            xmlDocument1.AppendChild(rootNode1);

                            // Adding elements and attributes from JSON data
                            foreach (var property in jsonObject1)
                            {
                                XmlNode node1 = xmlDocument1.CreateElement(property.Name);
                                node1.InnerText = property.Value.ToString();
                                rootNode1.AppendChild(node1);
                            }
                            DataSet ds = new DataSet();
                            ds.ReadXml(new XmlNodeReader(xmlDocument1));
                            for(int r1=0;r1<= ds.Tables[0].Rows.Count-1;r1++)
                            {
                                idInstance = ds.Tables[0].Rows[r1][0].ToString();
                                wid = ds.Tables[0].Rows[r1][1].ToString();
                                typeInstance = ds.Tables[0].Rows[r1][2].ToString();
                            }

                        }
                        else if (xmlEqnode[i].ChildNodes.Item(j).Name.Trim() == "timestamp")
                        {
                            timestamp = xmlEqnode[i].ChildNodes.Item(j).InnerText.Trim();
                        }
                        else if (xmlEqnode[i].ChildNodes.Item(j).Name.Trim() == "idMessage")
                        {
                            idMessage = xmlEqnode[i].ChildNodes.Item(j).InnerText.Trim();
                        }
                        else if (xmlEqnode[i].ChildNodes.Item(j).Name.Trim() == "senderData")
                        {
                            senderData = xmlEqnode[i].ChildNodes.Item(j).InnerText.Trim();
                            dynamic jsonObject2 = JsonConvert.DeserializeObject(senderData.ToString());


                            XmlDocument xmlDocument2 = new XmlDocument();
                            XmlNode rootNode2 = xmlDocument2.CreateElement("instanceData");
                            xmlDocument2.AppendChild(rootNode2);

                            // Adding elements and attributes from JSON data
                            foreach (var property in jsonObject2)
                            {
                                XmlNode node2 = xmlDocument2.CreateElement(property.Name);
                                node2.InnerText = property.Value.ToString();
                                rootNode2.AppendChild(node2);
                            }
                            DataSet ds = new DataSet();
                            ds.ReadXml(new XmlNodeReader(xmlDocument2));
                            for (int r1 = 0; r1 <= ds.Tables[0].Rows.Count - 1; r1++)
                            {
                                chatId = ds.Tables[0].Rows[r1][0].ToString(); 
                                chatName = ds.Tables[0].Rows[r1][1].ToString();
                                sender = ds.Tables[0].Rows[r1][2].ToString();
                                senderName = ds.Tables[0].Rows[r1][3].ToString();
                            }
                        }
                        else if (xmlEqnode[i].ChildNodes.Item(j).Name.Trim() == "messageData")
                        {
                            messageData = xmlEqnode[i].ChildNodes.Item(j).InnerText.Trim();
                            dynamic jsonObject3 = JsonConvert.DeserializeObject(messageData.ToString());


                            XmlDocument xmlDocument3 = new XmlDocument();
                            XmlNode rootNode3 = xmlDocument3.CreateElement("instanceData");
                            xmlDocument3.AppendChild(rootNode3);

                            // Adding elements and attributes from JSON data
                            foreach (var property in jsonObject3)
                            {
                                XmlNode node3 = xmlDocument3.CreateElement(property.Name);
                                node3.InnerText = property.Value.ToString();
                                rootNode3.AppendChild(node3);
                            }
                            DataSet ds = new DataSet();
                            ds.ReadXml(new XmlNodeReader(xmlDocument3));
                            if (ds.Tables[0].Rows.Count>0)
                            {
                                typeMessage = ds.Tables[0].Rows[0][0].ToString();
                                string MessageBody = ds.Tables[0].Rows[0][1].ToString();
                                if (typeMessage== "extendedTextMessage")
                                {
                                    dynamic jsonObject4 = JsonConvert.DeserializeObject(MessageBody.ToString());


                                    XmlDocument xmlDocument4 = new XmlDocument();
                                    XmlNode rootNode4 = xmlDocument4.CreateElement("extendedTextMessage");
                                    xmlDocument4.AppendChild(rootNode4);

                                    // Adding elements and attributes from JSON data
                                    foreach (var property in jsonObject4)
                                    {
                                        XmlNode node4 = xmlDocument4.CreateElement(property.Name);
                                        node4.InnerText = property.Value.ToString();
                                        rootNode4.AppendChild(node4);
                                    }
                                    DataSet ds1 = new DataSet();
                                    ds1.ReadXml(new XmlNodeReader(xmlDocument4));
                                    for (int r1 = 0; r1 <= ds1.Tables[0].Rows.Count - 1; r1++)
                                    {
                                        text = ds1.Tables[0].Rows[r1][0].ToString();
                                        description = ds1.Tables[0].Rows[r1][1].ToString();
                                        title = ds1.Tables[0].Rows[r1][2].ToString();
                                      
                                    }  
                                }

                               
                            }
                            
                        }
                        else if (xmlEqnode[i].ChildNodes.Item(j).Name.Trim() == "senderData")
                        {
                            int val = xmlEqnode[i].ChildNodes.Item(j).ChildNodes.Item(0).ChildNodes.Count;
                            for (int k = 0; k <= xmlEqnode[i].ChildNodes.Item(j).ChildNodes.Item(0).ChildNodes.Count - 1; k++)
                            {

                            }
                        }
                    }
                }
                 dynamic response = JsonConvert.DeserializeObject(jsonData.ToString());

                JArray paramsArray = (JArray)JToken.FromObject(response);

                foreach (JToken param in paramsArray)
                {
                   
                }
                DataSet myDataSet = JsonConvert.DeserializeObject<DataSet>(jsonData.ToString());

                 
                string strvalue = "";
                for (int row = 0; row <= myDataSet.Tables[0].Rows.Count - 1; row++)
                {
                    for (int col = 0; col <= myDataSet.Tables[0].Rows.Count - 1; col++)
                    {
                        strvalue = strvalue + myDataSet.Tables[0].Rows[row][col].ToString();
                    }

                }


                // Find the first array using Linq
                var srcArray = jsonData.Descendants().Where(d => d is JArray).First();
                //Console.WriteLine("extarcted data:" + srcArray);
                var trgArray = new JArray();
                foreach (JObject row in srcArray.Children())
                {
                    var cleanRow = new JObject();
                    foreach (JProperty column in row.Properties())
                    {
                        // Only include JValue types
                        if (column.Value is JValue)
                        {
                            cleanRow.Add(column.Name, column.Value);
                        }
                    }
                    trgArray.Add(cleanRow);
                }
             
                DataTable dataTable1 = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(trgArray));
               

                // Deserialize the received JSON data into a strongly-typed object
                var receivedData = JsonConvert.DeserializeObject<GPSData>(jsonData.ToString());

                string Data = jsonData.ToString().Replace('"', ' ').Replace('{', ' ').Replace('}', ' ').ToString();
                string sdata1 = jsonData.ToString(Newtonsoft.Json.Formatting.None);
                string sdata = sdata1.Replace('"', ' ').Replace('{', ' ').Replace('}', ' ').ToString();
                StreamWriter sw1 = new StreamWriter("D:\\mGPS\\gpsdata2.txt");
                //Write a line of text
                sw1.WriteLine("============");
                //Write a second line of text
                sw1.WriteLine(Data.ToString());
                sw1.WriteLine(sdata1.ToString());
                sw1.WriteLine("============");
                sw1.WriteLine(sdata.ToString());
                sw1.WriteLine("============");
                sw1.WriteLine(strvalue.ToString());
                //Close the file
                sw1.Close();

                int opt = 0;
                string final = "";
                for (int i = 0; i <= sdata.Split(',').Length - 1; i++)
                {
                    if (sdata.Split(',')[i].Split(':')[1].ToString().Trim() == "incomingMessageReceived")
                    {
                        opt = 1;
                    }
                    if (opt == 1)
                    {
                        if (sdata.Split(',')[i].Split(':')[0].ToString().Trim() == "idInstance")
                        {
                            idInstance = sdata.Split(',')[i].Split(':')[1].ToString();
                        }
                        else if (sdata.Split(',')[i].Split(':')[0].ToString().Trim() == "wid")
                        {
                            wid = sdata.Split(',')[i].Split(':')[1].ToString();
                        }
                        else if (sdata.Split(',')[i].Split(':')[0].ToString().Trim() == "timestamp")
                        {
                            timestamp = sdata.Split(',')[i].Split(':')[1].ToString();
                        }
                        else if (sdata.Split(',')[i].Split(':')[0].ToString().Trim() == "idMessage")
                        {
                            idMessage = sdata.Split(',')[i].Split(':')[1].ToString();
                        }
                        else if (sdata.Split(',')[i].Split(':')[0].ToString().Trim() == "chatId")
                        {
                            chatId = sdata.Split(',')[i].Split(':')[1].ToString();
                        }
                        else if (sdata.Split(',')[i].Split(':')[0].ToString().Trim() == "chatName")
                        {
                            chatName = sdata.Split(',')[i].Split(':')[1].ToString();
                        }
                        else if (sdata.Split(',')[i].Split(':')[0].ToString().Trim() == "senderName")
                        {
                            senderName = sdata.Split(',')[i].Split(':')[1].ToString();
                        }
                        else if (sdata.Split(',')[i].Split(':')[0].ToString().Trim() == "typeMessage")
                        {
                            typeMessage = sdata.Split(',')[i].Split(':')[1].ToString();
                        }
                        else if (sdata.Split(',')[i].Split(':')[0].ToString().Trim() == "textMessage")
                        {
                            textMessage = sdata.Split(',')[i].Split(':')[1].ToString();
                        }
                        final = idInstance + '~' + wid + '~' + timestamp + '~' + idMessage + '~' + chatId + '~' + chatName + '~' +  senderName + '~' + typeMessage + '~' + textMessage;
                    }
                }
                StreamWriter sw = new StreamWriter("D:\\mGPS\\gpsdata.txt");
                //Write a line of text
                sw.WriteLine("============");
                //Write a second line of text
                sw.WriteLine(jsonData.ToString());
                sw.WriteLine(final.ToString());
                //Close the file
                sw.Close();
                // Process the received data
                // ...

                // Return a success response
                return Ok("Data received and processed successfully.");
            }
            catch (Exception ex)
            {
                StreamWriter sw = new StreamWriter("D:\\mGPS\\gpsdata1.txt");
                //Write a line of text
                sw.WriteLine("============");
                //Write a second line of text
                sw.WriteLine(ex.Message.ToString());
                sw.Close();
                //Close the file
                // Handle any exceptions that occurred during data processing
                return BadRequest("An error occurred while processing the data: " + ex.Message);
            }
        }

        
    }
}