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
using System.Configuration;
using System.Runtime.InteropServices;
using System.Data.Entity;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
        public IHttpActionResult EmployeeDutyDateTimeUpdate(string EmployeeCode, string EmployeeName, DateTime DutyIn_Datetime, DateTime DutyOut_Datetime, float DutyIn_Latitude, float DutyIn_Longitude, float DutyOut_Latitude, float DutyOut_Longitude)
        {
            var Result1 = "";
            try
            {
                var result = DAL_AccessLayer.Employee_Duty_IN_OUT_Update(EmployeeCode, EmployeeName, DutyIn_Datetime, DutyOut_Datetime, DutyIn_Latitude, DutyIn_Longitude, DutyOut_Latitude, DutyOut_Longitude);
                if (result.StatusCode == 1)
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
            catch (Exception ex)
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
        public IHttpActionResult Login(string Userid, string Password)
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
        public void Post([FromBody] string value)
        {
        }

        [HttpGet]
        [Route("api/bhskapi/GetLogin")]
        public HttpResponseMessage GetLogin(string Userid, string Password)
        {

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            if (Userid == "test" && Password == "test")
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

            if (targeted_pickup_datetime != "" && targeted_delivery_datetime != "")
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
        public IEnumerable<ApplicationModel.JobStatusActivities> ts_job_status_counts(string startTime, string endTime, string workgroup)
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
        public void Put(int id, [FromBody] string value)
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
        public IEnumerable<ApplicationModel.UserInfo> GetUserInfo()
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            var UserId = identity.Name;

            var result = (dynamic)null;
            if (UserId != "")
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
                    result = DAL_AccessLayer.Change_Reset_Password(Userid, encrptPw, "", 1);
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
        public IHttpActionResult ChangePassword(string OldPassword, string NewPassword)
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
                    result = DAL_AccessLayer.Change_Reset_Password(Userid, encrptPw1, encrptPw2, 2);
                    if (result.StatusCode == 1)
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


        [Authorize]
        [HttpPost]
        [Route("api/bhskapi/addusers")]
        public HttpResponseMessage AddUsers([FromBody] UserInfoAdd UserInfoAdd)
        {
            try
            {
                var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
                var UserId = identity.Name;
                var encrptPw1 = Convert.ToString(Encrypt_Dll.EncryptDLL.enCrypt(UserInfoAdd.Password));
                var result = DAL_AccessLayer.UserInfo_Add(UserInfoAdd.UserName, encrptPw1, UserInfoAdd.Role, UserInfoAdd.EmailId, UserInfoAdd.Contact, UserInfoAdd.Forwarder, UserInfoAdd.Workgroup, UserId);
                if (result.StatusCode == 1)
                    return Request.CreateResponse(HttpStatusCode.OK, "{ErrCode:1,ErrMsg:" + result.ErrMsg + "}");
                else
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "{ErrCode:-1,ErrMsg:" + result.ErrMsg + "}");

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("api/bhskapi/updateuser")]
        public HttpResponseMessage UpdateUser([FromBody] UserInfo userInfo)
        {
            try
            {
                var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
                var UserId = identity.Name;
                String encrptPw1 = null;
                if (!String.IsNullOrEmpty(userInfo.Password))
                {
                    encrptPw1 = Convert.ToString(Encrypt_Dll.EncryptDLL.enCrypt(userInfo.Password));
                }
                var result = DAL_AccessLayer.UserInfo_Update(userInfo.UserName, encrptPw1, userInfo.Role, userInfo.EmailId, userInfo.Contact, userInfo.Forwarder);
                if (result.StatusCode == 1)
                    return Request.CreateResponse(HttpStatusCode.OK, "{ErrCode:1,ErrMsg:" + result.ErrMsg + "}");
                else
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "{ErrCode:-1,ErrMsg:" + result.ErrMsg + "}");

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("api/bhskapi/deleteuser")]
        public HttpResponseMessage DeleteUser([FromBody] UserInfo userInfo)
        {
            string result1;
            try
            {
                var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
                var UserId = identity.Name;
                var result = DAL_AccessLayer.UserInfo_Delete(userInfo.UserName);
                if (result.StatusCode == 1)
                    result1 = "{ErrCode:1,ErrMsg:" + result.ErrMsg + "}";
                else
                    result1 = "{ErrCode:-1,ErrMsg:" + result.ErrMsg + "}";

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, result1);
        }

        [Authorize]
        [HttpGet]
        [Route("api/bhskapi/getuserlist")]
        public IEnumerable<ApplicationModel.UserInfo> GetUserList(string UserName = null, string EmailId = null, string Contact = null, string Workgroup = null, string Search_keyword = null, int Page = 1)
        {

            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            var Userid = identity.Name;
            List<ApplicationModel.UserInfo> lstUserList = new List<ApplicationModel.UserInfo>();
            var result = (dynamic)null;
            if (Userid != "")
            {
                result = DAL_AccessLayer.UserList(UserName, EmailId, Contact, Workgroup, Search_keyword, Page, 2);
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
                var httpRequest = HttpContext.Current.Request;
                HttpPostedFile Inputfile = null;
                Stream FileStream = null;
                if (httpRequest.Files.Count > 0 && httpRequest.Files[0].FileName.ToString() != "")
                {
                    for (int f = 0; f <= httpRequest.Files.Count - 1; f++)
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
                        throw new Exception("Something Went Wrong!, The Excel file uploaded has failed.");
                    }

                }
                else
                {
                    throw new Exception("File is not selected!, Please check it.");
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
        [Route("api/bhskapi/importtoollist")]
        public void ImportToolList()
        {
            int unchangedRecords = 0;
            int newRecords = 0;
            int updatedRecords = 0;
            int errors = 0;

            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            var Userid = identity.Name;

            ImportDetails importDetails = new ImportDetails
            {
                UserId = Userid,
                ImportDate = DateTime.Now,
                Details = "",
                Status = "Processing"
            };
            DAL_AccessLayer.AddImport(importDetails);
            try
            {
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0 && httpRequest.Files[0].FileName.ToString() != "")
                {
                    HttpPostedFile Inputfile = httpRequest.Files[0];
                    string filePath = HttpContext.Current.Server.MapPath("~/Excels/" + Inputfile.FileName);
                    Inputfile.SaveAs(filePath);

                    try
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(Inputfile.InputStream))
                        {
                            DataSet dataSet = reader.AsDataSet();
                            DataTable dataTable = dataSet.Tables["Tool and Accessories List"];
                            object[] columns = null;
                            for (int row = 0; row < dataTable.Rows.Count; row++)
                            {
                                bool rowError = false;
                                DataRow dataRow = dataTable.Rows[row];
                                if (row == 0)
                                {
                                    columns = dataRow.ItemArray;
                                }
                                else
                                {
                                    MainTool mainTool = new MainTool() { CreatedDateTime = DateTime.Now };
                                    Shipment shipment = new Shipment();
                                    for (int col = 0; col < columns.Length; col++)
                                    {
                                        try
                                        {
                                            switch (Regex.Replace(columns[col].ToString().ToUpper(), @"\t|\n|\r", ""))
                                            {
                                                case "PO NUMBER":
                                                    mainTool.PONumber = dataRow[col].ToString();
                                                    break;
                                                case "AREA":
                                                    mainTool.Area = dataRow[col].ToString();
                                                    break;
                                                case "ENTITY":
                                                    mainTool.Entity = dataRow[col].ToString();
                                                    break;
                                                case "TYPE - SAP":
                                                    mainTool.SubType = dataRow[col].ToString();
                                                    break;
                                                case "REMARK":
                                                case "REMARKS":
                                                    mainTool.Remarks = dataRow[col].ToString();
                                                    break;
                                                case "TYPE OF TOOLS":
                                                    mainTool.Remarks = dataRow[col].ToString();
                                                    mainTool.Type = dataRow[col].ToString();
                                                    break;
                                                case "VENDOR":
                                                    mainTool.Vendor = dataRow[col].ToString();
                                                    break;
                                                case "VEQPID":
                                                    mainTool.VEQPID = dataRow[col].ToString();
                                                    break;
                                                case "EQPID":
                                                case "UMC EQID":
                                                    mainTool.EQPID = dataRow[col].ToString();
                                                    shipment.EQPID = dataRow[col].ToString();
                                                    break;
                                                case "MODEL":
                                                case "TOOL MODEL":
                                                    mainTool.Model = dataRow[col].ToString();
                                                    break;
                                                case "PO DESCRIPTION":
                                                    mainTool.PODescription = dataRow[col].ToString();
                                                    break;
                                                case "Buyer Assignment":
                                                    mainTool.Custom1 = dataRow[col].ToString();
                                                    break;
                                                case "TRADE TERM":
                                                case "INCOTERM":
                                                    mainTool.TradeTerm = dataRow[col].ToString();
                                                    break;
                                                case "COUNTRY":
                                                    shipment.Country = dataRow[col].ToString();
                                                    break;
                                                case "MODE":
                                                    shipment.Mode = dataRow[col].ToString();
                                                    break;
                                                case "IEB ACTION":
                                                    mainTool.Custom2 = dataRow[col].ToString();
                                                    break;
                                                case "Y24 PRIORITY":
                                                    mainTool.Priority = ConvertToBoolean(dataRow[col]);
                                                    break;
                                                case "M/I DATE":
                                                case "MOVE IN DATE":
                                                    mainTool.MIDate = dataRow[col] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow[col]);
                                                    break;
                                                case "MOVE IN DAY":
                                                    break;
                                                case "FCA DATE":
                                                    mainTool.FCADate = dataRow[col] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow[col]);
                                                    break;
                                                case "FCA DAY":
                                                    break;
                                                case "ETA DATE":
                                                    shipment.Planned_SG_Arrival = dataRow[col] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow[col]);
                                                    break;
                                                case "ETA DAY":
                                                    break;
                                                case "FREIGHT DETAILS":
                                                    break;
                                                case "DUAL PICK UP LOCATION?":
                                                    shipment.DualPickup = ConvertToBoolean(dataRow[col]);
                                                    break;
                                                case "IF YES, PLS INDICATE":
                                                    break;
                                                case "TEMP CONTROL":
                                                case "TEMP CONTOL":
                                                    shipment.Temperature = ConvertToBoolean(dataRow[col]);
                                                    break;
                                                case "HUMIDITY CONTROL":
                                                case "HUMIDITY CONTOL":
                                                    shipment.Humidity = ConvertToBoolean(dataRow[col]);
                                                    break;
                                                case "DG CARGO":
                                                    shipment.DangerousCargo = ConvertToBoolean(dataRow[col]);
                                                    break;
                                                case "> 56M3":
                                                    break;
                                                case "21-55M3":
                                                    break;
                                                case "< 20M3":
                                                    break;
                                                case "NEED PERMIT?":
                                                case "NEED PERMIT FOR IMPORT?":
                                                    shipment.Permit = ConvertToBoolean(dataRow[col]);
                                                    break;
                                                case "NEED POLICE ESCORTS?":
                                                case "NEED POLICE ESSCORTS?":
                                                    shipment.Escort = ConvertToBoolean(dataRow[col]);
                                                    break;
                                                case "FORWARDER":
                                                case "APPOINTED FORWARDER":
                                                    shipment.Forwarder = dataRow[col].ToString();
                                                    break;
                                                case "STATUS":
                                                    break;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            rowError = true;
                                            DAL_AccessLayer.AddImportError(new ImportErrorDetails
                                            {
                                                ImportId = importDetails.ImportId,
                                                RowNumber = row + 1,
                                                Details = columns[col].ToString() + " : " + ex.Message
                                            });
                                        }
                                    }
                                    #region Validation of data in the row
                                    // Forwarder column cannot be empty
                                    if (String.IsNullOrEmpty(shipment.Forwarder))
                                    {
                                        rowError = true;
                                        DAL_AccessLayer.AddImportError(new ImportErrorDetails
                                        {
                                            ImportId = importDetails.ImportId,
                                            RowNumber = row + 1,
                                            Details = "Validation error - Forwarder cannot be empty"
                                        });
                                    }
                                    // EQPID column cannot be empty
                                    if (String.IsNullOrEmpty(mainTool.EQPID))
                                    {
                                        rowError = true;
                                        DAL_AccessLayer.AddImportError(new ImportErrorDetails
                                        {
                                            ImportId = importDetails.ImportId,
                                            RowNumber = row + 1,
                                            Details = "Validation error - EQID or EQPID cannot be empty"
                                        });
                                    }
                                    // Vendor column cannot be empty
                                    if (String.IsNullOrEmpty(mainTool.Vendor))
                                    {
                                        rowError = true;
                                        DAL_AccessLayer.AddImportError(new ImportErrorDetails
                                        {
                                            ImportId = importDetails.ImportId,
                                            RowNumber = row + 1,
                                            Details = "Validation error - Vendor cannot be empty"
                                        });
                                    }
                                    // TradeTerm column cannot be empty
                                    if (String.IsNullOrEmpty(mainTool.TradeTerm))
                                    {
                                        rowError = true;
                                        DAL_AccessLayer.AddImportError(new ImportErrorDetails
                                        {
                                            ImportId = importDetails.ImportId,
                                            RowNumber = row + 1,
                                            Details = "Validation error - TradeTerm or Incoterm cannot be empty"
                                        });
                                    }
                                    // FCADate date column cannot be empty
                                    if (mainTool.FCADate == null)
                                    {
                                        rowError = true;
                                        DAL_AccessLayer.AddImportError(new ImportErrorDetails
                                        {
                                            ImportId = importDetails.ImportId,
                                            RowNumber = row + 1,
                                            Details = "Validation error - FCADate cannot be empty"
                                        });
                                    }
                                    // Move in date column cannot be empty
                                    if (mainTool.MIDate == null)
                                    {
                                        rowError = true;
                                        DAL_AccessLayer.AddImportError(new ImportErrorDetails
                                        {
                                            ImportId = importDetails.ImportId,
                                            RowNumber = row + 1,
                                            Details = "Validation error - Move in date cannot be empty"
                                        });
                                    }
                                    // FCADate date cannot be later than move in date
                                    if (mainTool.FCADate != null && mainTool.MIDate != null && mainTool.FCADate > mainTool.MIDate)
                                    {
                                        rowError = true;
                                        DAL_AccessLayer.AddImportError(new ImportErrorDetails
                                        {
                                            ImportId = importDetails.ImportId,
                                            RowNumber = row + 1,
                                            Details = "Validation error - FCADate cannot be later than move in date"
                                        });
                                    }
                                    // Country column cannot be empty
                                    if (String.IsNullOrEmpty(shipment.Country))
                                    {
                                        rowError = true;
                                        DAL_AccessLayer.AddImportError(new ImportErrorDetails
                                        {
                                            ImportId = importDetails.ImportId,
                                            RowNumber = row + 1,
                                            Details = "Validation error - Country cannot be empty"
                                        });
                                    }
                                    // Mode column cannot be empty
                                    if (String.IsNullOrEmpty(shipment.Mode))
                                    {
                                        rowError = true;
                                        DAL_AccessLayer.AddImportError(new ImportErrorDetails
                                        {
                                            ImportId = importDetails.ImportId,
                                            RowNumber = row + 1,
                                            Details = "Validation error - Mode cannot be empty"
                                        });
                                    }
                                    // Forwarder must be in the list of configured forwarders
                                    List<String> forwarderList = DAL_AccessLayer.ConfigurationList("Forwarder", "Lookup", null, null, 0, 2).Select(config => config.Value).ToList();
                                    if (!String.IsNullOrEmpty(shipment.Forwarder) && !forwarderList.Contains(shipment.Forwarder))
                                    {
                                        rowError = true;
                                        DAL_AccessLayer.AddImportError(new ImportErrorDetails
                                        {
                                            ImportId = importDetails.ImportId,
                                            RowNumber = row + 1,
                                            Details = $"Validation error - Forwarder ({shipment.Forwarder}) is not in the list of configured forwarders"
                                        });
                                    }
                                    #endregion

                                    if (rowError)
                                    {
                                        errors++;
                                    }
                                    else
                                    {
                                        try
                                        {
                                            #region Check change in data in the row
                                            MainTool currentTool = DAL_AccessLayer.FindMainTool(mainTool.PONumber, mainTool.EQPID);

                                            if (currentTool != null)
                                            {
                                                bool shipmentChanged = false;
                                                bool toolChanged = CheckToolChange(mainTool, currentTool, Userid, importDetails.ImportId, row + 1);
                                                if (toolChanged)
                                                {
                                                    DAL_AccessLayer.UpdateMainTool(currentTool);
                                                }
                                                Shipment currentShipment = DAL_AccessLayer.FindShipmentInfo(currentTool);
                                                if (currentShipment != null)
                                                {
                                                    shipmentChanged = CheckShipmentChange(shipment, currentShipment, Userid, importDetails.ImportId, row + 1);
                                                    if (shipmentChanged)
                                                    {
                                                        DAL_AccessLayer.UpdateShipmentInfo(currentShipment);
                                                    }
                                                }
                                                else
                                                {
                                                    shipmentChanged = true;
                                                    shipment.ToolId = currentTool.Id;
                                                    DAL_AccessLayer.AddImportDetailsLog(new ImportDetailsLog
                                                    {
                                                        ImportId = importDetails.ImportId,
                                                        Activity = "Insert",
                                                        Column_Name = null,
                                                        Details = $"Insert New Shipment for Tool PO Number = {currentTool.PONumber}, EQPID = {currentTool.EQPID}",
                                                        RowNumber = row + 1,
                                                        UserId = Userid
                                                    });
                                                    DAL_AccessLayer.AddShipment(shipment);
                                                }
                                                if (toolChanged || shipmentChanged)
                                                {
                                                    updatedRecords++;
                                                }
                                                else
                                                {
                                                    unchangedRecords++;
                                                }
                                            }
                                            else
                                            {
                                                DAL_AccessLayer.AddImportDetailsLog(new ImportDetailsLog
                                                {
                                                    ImportId = importDetails.ImportId,
                                                    Activity = "Insert",
                                                    Column_Name = null,
                                                    Details = $"Insert New Tool PO Number = {mainTool.PONumber}, EQPID = {mainTool.EQPID}",
                                                    RowNumber = row + 1,
                                                    UserId = Userid
                                                });
                                                DAL_AccessLayer.AddMainTool(mainTool);
                                                shipment.ToolId = mainTool.Id;
                                                DAL_AccessLayer.AddImportDetailsLog(new ImportDetailsLog
                                                {
                                                    ImportId = importDetails.ImportId,
                                                    Activity = "Insert",
                                                    Column_Name = null,
                                                    Details = $"Insert New Shipment for Tool PO Number = {mainTool.PONumber}, EQPID = {mainTool.EQPID}",
                                                    RowNumber = row + 1,
                                                    UserId = Userid
                                                });
                                                DAL_AccessLayer.AddShipment(shipment);
                                                newRecords++;
                                            }
                                            #endregion
                                        }
                                        catch (Exception ex)
                                        {
                                            rowError = true;
                                            DAL_AccessLayer.AddImportError(new ImportErrorDetails
                                            {
                                                ImportId = importDetails.ImportId,
                                                RowNumber = row + 1,
                                                Details = $"Unexpected Error : Please contact the Administrator. Error Message = {ex.Message}"
                                            });
                                        }
                                    }
                                }
                                if (row % 100 == 0)
                                {
                                    // Update the status every 100 rows processed
                                    importDetails.Details = $"{newRecords} new records; {updatedRecords} records updated; {unchangedRecords} records unchanged; {errors} row errors";
                                    importDetails.Status = "Processing";
                                    DAL_AccessLayer.UpdateImport(importDetails);
                                }
                            }
                            importDetails.Details = $"{newRecords} new records; {updatedRecords} records updated; {unchangedRecords} records unchanged; {errors} row errors";
                            importDetails.Status = (errors != 0 ? "Failed" : "Success");
                            DAL_AccessLayer.UpdateImport(importDetails);
                        }
                    }
                    catch (Exception ex)
                    {
                        importDetails.Details = ex.Message;
                        importDetails.Status = "Failed";
                        DAL_AccessLayer.UpdateImport(importDetails);
                    }
                }
            }
            catch (Exception ex)
            {
                importDetails.Details = ex.Message;
                importDetails.Status = "Failed";
                DAL_AccessLayer.UpdateImport(importDetails);
            }
        }

        private static bool CheckShipmentChange(Shipment shipment, Shipment currentShipment, string Userid, int importId, int row)
        {
            bool changed = false;
            if (shipment.Country != currentShipment.Country)
            {
                DAL_AccessLayer.AddImportDetailsLog(new ImportDetailsLog
                {
                    ImportId = importId,
                    Activity = "Update",
                    Column_Name = "Country",
                    Details = currentShipment.Country + "→" + shipment.Country,
                    RowNumber = row,
                    UserId = Userid
                });
                currentShipment.Country = shipment.Country;
                changed = true;
            }
            if (shipment.Mode != currentShipment.Mode)
            {
                DAL_AccessLayer.AddImportDetailsLog(new ImportDetailsLog
                {
                    ImportId = importId,
                    Activity = "Update",
                    Column_Name = "Mode",
                    Details = currentShipment.Mode + "→" + shipment.Mode,
                    RowNumber = row,
                    UserId = Userid
                });
                currentShipment.Mode = shipment.Mode;
                changed = true;
            }
            if (shipment.Planned_SG_Arrival != currentShipment.Planned_SG_Arrival)
            {
                DAL_AccessLayer.AddImportDetailsLog(new ImportDetailsLog
                {
                    ImportId = importId,
                    Activity = "Update",
                    Column_Name = "Planned_SG_Arrival",
                    Details = currentShipment.Planned_SG_Arrival + "→" + shipment.Planned_SG_Arrival,
                    RowNumber = row,
                    UserId = Userid
                });
                currentShipment.Planned_SG_Arrival = shipment.Planned_SG_Arrival;
                changed = true;
            }
            if (shipment.DualPickup != currentShipment.DualPickup)
            {
                DAL_AccessLayer.AddImportDetailsLog(new ImportDetailsLog
                {
                    ImportId = importId,
                    Activity = "Update",
                    Column_Name = "DualPickup",
                    Details = currentShipment.DualPickup + "→" + shipment.DualPickup,
                    RowNumber = row,
                    UserId = Userid
                });
                currentShipment.DualPickup = shipment.DualPickup;
                changed = true;
            }
            if (shipment.Temperature != currentShipment.Temperature)
            {
                DAL_AccessLayer.AddImportDetailsLog(new ImportDetailsLog
                {
                    ImportId = importId,
                    Activity = "Update",
                    Column_Name = "Temperature",
                    Details = currentShipment.Temperature + "→" + shipment.Temperature,
                    RowNumber = row,
                    UserId = Userid
                });
                currentShipment.Temperature = shipment.Temperature;
                changed = true;
            }
            if (shipment.Humidity != currentShipment.Humidity)
            {
                DAL_AccessLayer.AddImportDetailsLog(new ImportDetailsLog
                {
                    ImportId = importId,
                    Activity = "Update",
                    Column_Name = "Humidity",
                    Details = currentShipment.Humidity + "→" + shipment.Humidity,
                    RowNumber = row,
                    UserId = Userid
                });
                currentShipment.Humidity = shipment.Humidity;
                changed = true;
            }
            if (shipment.Permit != currentShipment.Permit)
            {
                DAL_AccessLayer.AddImportDetailsLog(new ImportDetailsLog
                {
                    ImportId = importId,
                    Activity = "Update",
                    Column_Name = "Permit",
                    Details = currentShipment.Permit + "→" + shipment.Permit,
                    RowNumber = row,
                    UserId = Userid
                });
                currentShipment.Permit = shipment.Permit;
                changed = true;
            }
            if (shipment.Escort != currentShipment.Escort)
            {
                DAL_AccessLayer.AddImportDetailsLog(new ImportDetailsLog
                {
                    ImportId = importId,
                    Activity = "Update",
                    Column_Name = "Escort",
                    Details = currentShipment.Escort + "→" + shipment.Escort,
                    RowNumber = row,
                    UserId = Userid
                });
                currentShipment.Escort = shipment.Escort;
                changed = true;
            }
            if (shipment.Forwarder != currentShipment.Forwarder)
            {
                DAL_AccessLayer.AddImportDetailsLog(new ImportDetailsLog
                {
                    ImportId = importId,
                    Activity = "Update",
                    Column_Name = "Forwarder",
                    Details = currentShipment.Forwarder + "→" + shipment.Forwarder,
                    RowNumber = row,
                    UserId = Userid
                });
                currentShipment.Forwarder = shipment.Forwarder;
                changed = true;
            }

            return changed;
        }

        private static bool CheckToolChange(MainTool mainTool, MainTool currentTool, string Userid, int importId, int row)
        {
            bool changed = false;
            if (mainTool.FCADate != currentTool.FCADate)
            {
                DAL_AccessLayer.AddImportDetailsLog(new ImportDetailsLog
                {
                    ImportId = importId,
                    Activity = "Update",
                    Column_Name = "FCADate",
                    Details = currentTool.FCADate + "→" + mainTool.FCADate,
                    RowNumber = row,
                    UserId = Userid
                });

                currentTool.Previous_FCA_Changes = currentTool.FCADate;
                currentTool.FCADate = mainTool.FCADate;
                changed = true;
            }
            if (mainTool.Area != currentTool.Area)
            {
                DAL_AccessLayer.AddImportDetailsLog(new ImportDetailsLog
                {
                    ImportId = importId,
                    Activity = "Update",
                    Column_Name = "Area",
                    Details = currentTool.Area + "→" + mainTool.Area,
                    RowNumber = row,
                    UserId = Userid
                });
                currentTool.Area = mainTool.Area;
                changed = true;
            }
            if (mainTool.Entity != currentTool.Entity)
            {
                DAL_AccessLayer.AddImportDetailsLog(new ImportDetailsLog
                {
                    ImportId = importId,
                    Activity = "Update",
                    Column_Name = "Entity",
                    Details = currentTool.Entity + "→" + mainTool.Entity,
                    RowNumber = row,
                    UserId = Userid
                });
                currentTool.Entity = mainTool.Entity;
                changed = true;
            }
            if (mainTool.Model != currentTool.Model)
            {
                DAL_AccessLayer.AddImportDetailsLog(new ImportDetailsLog
                {
                    ImportId = importId,
                    Activity = "Update",
                    Column_Name = "Model",
                    Details = currentTool.Model + "→" + mainTool.Model,
                    RowNumber = row,
                    UserId = Userid
                });
                currentTool.Model = mainTool.Model;
                changed = true;
            }
            if (mainTool.Priority != currentTool.Priority)
            {
                DAL_AccessLayer.AddImportDetailsLog(new ImportDetailsLog
                {
                    ImportId = importId,
                    Activity = "Update",
                    Column_Name = "Priority",
                    Details = currentTool.Priority + "→" + mainTool.Priority,
                    RowNumber = row,
                    UserId = Userid
                });
                currentTool.Priority = mainTool.Priority;
                changed = true;
            }
            if (mainTool.Remarks != currentTool.Remarks)
            {
                DAL_AccessLayer.AddImportDetailsLog(new ImportDetailsLog
                {
                    ImportId = importId,
                    Activity = "Update",
                    Column_Name = "Remarks",
                    Details = currentTool.Remarks + "→" + mainTool.Remarks,
                    RowNumber = row,
                    UserId = Userid
                });
                currentTool.Remarks = mainTool.Remarks;
                changed = true;
            }
            if (mainTool.VEQPID != currentTool.VEQPID)
            {
                DAL_AccessLayer.AddImportDetailsLog(new ImportDetailsLog
                {
                    ImportId = importId,
                    Activity = "Update",
                    Column_Name = "VEQPID",
                    Details = currentTool.VEQPID + "→" + mainTool.VEQPID,
                    RowNumber = row,
                    UserId = Userid
                });
                currentTool.VEQPID = mainTool.VEQPID;
                changed = true;
            }
            if (mainTool.Vendor != currentTool.Vendor)
            {
                DAL_AccessLayer.AddImportDetailsLog(new ImportDetailsLog
                {
                    ImportId = importId,
                    Activity = "Update",
                    Column_Name = "Vendor",
                    Details = currentTool.Vendor + "→" + mainTool.Vendor,
                    RowNumber = row,
                    UserId = Userid
                });
                currentTool.Vendor = mainTool.Vendor;
                changed = true;
            }
            if (mainTool.MIDate != currentTool.MIDate)
            {
                DAL_AccessLayer.AddImportDetailsLog(new ImportDetailsLog
                {
                    ImportId = importId,
                    Activity = "Update",
                    Column_Name = "MIDate",
                    Details = currentTool.MIDate + "→" + mainTool.MIDate,
                    RowNumber = row,
                    UserId = Userid
                });
                currentTool.MIDate = mainTool.MIDate;
                changed = true;
            }
            if (mainTool.TradeTerm != currentTool.TradeTerm)
            {
                DAL_AccessLayer.AddImportDetailsLog(new ImportDetailsLog
                {
                    ImportId = importId,
                    Activity = "Update",
                    Column_Name = "TradeTerm",
                    Details = currentTool.TradeTerm + "→" + mainTool.TradeTerm,
                    RowNumber = row,
                    UserId = Userid
                });
                currentTool.TradeTerm = mainTool.TradeTerm;
                changed = true;
            }

            return changed;
        }

        private bool ConvertToBoolean(object obj)
        {
            if (obj == DBNull.Value)
            {
                return false;
            }
            else if (String.IsNullOrEmpty(obj.ToString()))
            {
                return false;
            }
            else if (Regex.IsMatch(obj.ToString(),@"\b[yY](es)?\b|\b[tT](rue)?\b|\b1\b"))
            {
                return true;
            }
            else if (Regex.IsMatch(obj.ToString(), @"\b[nN]o?\b|\b[fF](alse)?\b|\b#?[nN][aA]\b"))
            {
                return false;
            }
            else if (obj.ToString() == "0")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("api/bhskapi/gettoolslist")]
        /**
         * param name="Fromdate" - format of parameter yyyy-MM-ddTHH:mm:ss
         * param name="Todate" - format of parameter yyyy-MM-ddTHH:mm:ss
         */
        public IEnumerable<MainTool> GetToolsList(string Area = null, string Vendor = null, DateTime? Fromdate = null, DateTime? Todate = null, string Search_keyword = null, int Page = 1)
        {

            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            var Userid = identity.Name;
            var result = (dynamic)null;
            if (Userid != "")
            {
                result = DAL_AccessLayer.GetMainToolsList(Userid, Area, Vendor, Fromdate, Todate, Search_keyword, Page, 1);
            }
            return result;
        }

        [Authorize]
        [HttpGet]
        [Route("api/bhskapi/getexporttoolslist")]
        public IEnumerable<ApplicationModel.MainTool> GetExportToolsList(string Area, string Vendor, DateTime? Fromdate, DateTime? Todate, string Search_keyword)
        {

            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            var Userid = identity.Name;
            var result = (dynamic)null;
            if (Userid != "")
            {
                result = DAL_AccessLayer.GetMainToolsList(Userid, Area, Vendor, Fromdate, Todate, Search_keyword, 0, 2);
            }
            return result;
        }

        [Authorize]
        [HttpGet]
        [Route("api/bhskapi/getimportdetails")]
        public IEnumerable<ApplicationModel.ImportDetails> GetImportDetails(int Page, [Optional] string Search_keyword)
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
        [Route("api/bhskapi/getimporterrordetails")]
        public IEnumerable<ApplicationModel.ImportErrorDetails> GetImportErrorDetails(int ImportId, int Page)
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
        [Route("api/bhskapi/getimportdetailslog")]
        public IEnumerable<ImportDetailsLog> GetImportDetailsLog(int ImportId, int Page)
        {

            return DAL_AccessLayer.GetImportDetailsLog(ImportId, Page);
        }

        [Authorize]
        [HttpGet]
        [Route("api/bhskapi/getshipmentdetailslist")]
        public IEnumerable<ApplicationModel.Shipment> GetShipmentDetailsList(string ToolId, string Eqpid = null, string TradeTerm = null, string Country = null, string Mode = null, string Search_keyword = null, int Page = 1)
        {

            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            var Userid = identity.Name;
            var result = (dynamic)null;
            if (Userid != "")
            {
                result = DAL_AccessLayer.GetShipmentDetailsList(Userid, ToolId, Eqpid, TradeTerm, Country, Mode, Search_keyword, Page, 1);
            }
            return result;
        }

        [Authorize]
        [HttpGet]
        [Route("api/bhskapi/getdamagedetails")]
        public IEnumerable<DamageDetails> GetDamageDetails(int ShipmentId)
        {

            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            var Userid = identity.Name;
            var result = (dynamic)null;
            if (Userid != "")
            {
                result = DAL_AccessLayer.GetDamageDetails(ShipmentId);
            }
            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("api/bhskapi/adddamagedetails")]
        public IHttpActionResult AddDamageDetails(DamageDetails damageDetails)
        {

            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            var Userid = identity.Name;
            if (Userid != "")
            {
                string baseUrl = Url.Request.RequestUri.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped);

                if (damageDetails.DamagePhotos != null)
                {
                    foreach (DamagePhotos damagePhotos in damageDetails.DamagePhotos)
                    {
                        if (!String.IsNullOrEmpty(damagePhotos.Data))
                        {
                            String path = HttpContext.Current.Server.MapPath("~/Attachments/" + damagePhotos.FileName);
                            File.WriteAllBytes(path, Convert.FromBase64String(damagePhotos.Data));
                            damagePhotos.Photo_URL = baseUrl + "/Attachments/" + damagePhotos.FileName;
                            damagePhotos.Uploaded_Date = DateTime.Now;
                            damagePhotos.UserId = Userid;
                        }
                    }
                }

                DAL_AccessLayer.AddDamageDetails(damageDetails);
            }
            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("api/bhskapi/updatedamagedetails")]
        public IHttpActionResult UpdateDamageDetails(DamageDetails damageDetails)
        {

            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            var Userid = identity.Name;
            string baseUrl = Url.Request.RequestUri.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped);

            if (damageDetails.DamagePhotos != null)
            {
                foreach (DamagePhotos damagePhotos in damageDetails.DamagePhotos)
                {
                    if (!String.IsNullOrEmpty(damagePhotos.Data))
                    {
                        String path = HttpContext.Current.Server.MapPath("~/Attachments/" + damagePhotos.FileName);
                        File.WriteAllBytes(path, Convert.FromBase64String(damagePhotos.Data));
                        damagePhotos.Photo_URL = baseUrl + "/Attachments/" + damagePhotos.FileName;
                        damagePhotos.Uploaded_Date = DateTime.Now;
                        damagePhotos.UserId = Userid;
                    }
                }
            }

            try
            {
                DAL_AccessLayer.UpdateDamageDetails(damageDetails);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpPost]
        [Route("api/bhskapi/deldamagedetails")]
        public IHttpActionResult DeleteDamageDetails(DamageDetails damageDetails)
        {

            try
            {
                DAL_AccessLayer.DeleteDamageDetails(damageDetails);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpPost]
        [Route("api/bhskapi/deldamagephoto")]
        public IHttpActionResult DeleteDamagePhoto(DamagePhotos damagePhotos)
        {

            try
            {
                DAL_AccessLayer.DeleteDamagePhoto(damagePhotos);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpPost]
        [Route("api/bhskapi/delattachment")]
        public IHttpActionResult DeleteAttachment(Attachment attachment)
        {

            try
            {
                DAL_AccessLayer.DeleteAttachment(attachment);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpGet]
        [Route("api/bhskapi/getexportshipmentdetailslist")]
        public IEnumerable<ApplicationModel.Shipment> GetExportShipmentDetailsList(string Eqpid = null, string ToolId = null, string TradeTerm = null, string Country = null, string Mode = null, string Search_keyword = null)
        {

            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            var Userid = identity.Name;
            var result = (dynamic)null;
            if (Userid != "")
            {
                result = DAL_AccessLayer.GetShipmentDetailsList(Userid, ToolId, Eqpid, TradeTerm, Country, Mode, Search_keyword, 0, 2);
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
        public HttpResponseMessage AddConfiguration([FromBody] ConfigurationDetails configuration)
        {

            var Result1 = "";
            try
            {

                var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
                var UserId = identity.Name;

                var result = DAL_AccessLayer.Configuration_Add_Update(0, configuration.Name, configuration.Type, configuration.Value, 1, UserId);
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
        public HttpResponseMessage UpdateConfiguration([FromBody] ConfigurationDetails configuration)
        {

            var Result1 = "";
            try
            {

                var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
                var UserId = identity.Name;

                var result = DAL_AccessLayer.Configuration_Add_Update(configuration.Id, configuration.Name, configuration.Type, configuration.Value, 2, UserId);
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
        [Route("api/bhskapi/deleteconfiguration")]
        public HttpResponseMessage DeleteConfiguration([FromBody] ConfigurationDetails configuration)
        {

            string Result1;
            try
            {

                var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
                var UserId = identity.Name;

                var result = DAL_AccessLayer.Configuration_Delete(configuration.Id);
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
        public string AddShipmentDetails([FromBody] Shipment shipmentDetails)
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            var userId = identity.Name;

            try
            {
                string baseUrl = Url.Request.RequestUri.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped);

                if (shipmentDetails.Documents != null)
                {
                    foreach (Attachment document in shipmentDetails.Documents)
                    {
                        if (!String.IsNullOrEmpty(document.Data))
                        {
                            String path = HttpContext.Current.Server.MapPath("~/Attachments/" + document.FileName);
                            File.WriteAllBytes(path, Convert.FromBase64String(document.Data));
                            document.AttachmentFile_URL = baseUrl + "/Attachments/" + document.FileName;
                            document.Uploaded_Date = DateTime.Now;
                            document.UserId = userId;
                        }
                    }
                }


                DAL_AccessLayer.AddShipment(shipmentDetails);
                return "The shipment has been successfully added.";
            }
            catch (Exception ex)
            {
                return "Something Went Wrong!, The shipment creation has failed : " + ex.Message;
            }


        }

        [Authorize]
        [HttpPost]
        [Route("api/bhskapi/updateshipmentdetails")]
        public string UpdateShipmentDetails([FromBody] Shipment shipmentDetails)
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            var userId = identity.Name;
            try
            {
                string baseUrl = Url.Request.RequestUri.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped);

                if (shipmentDetails.Documents != null)
                {
                    foreach (Attachment document in shipmentDetails.Documents)
                    {
                        if (!String.IsNullOrEmpty(document.Data))
                        {
                            String path = HttpContext.Current.Server.MapPath("~/Attachments/" + document.FileName);
                            File.WriteAllBytes(path, Convert.FromBase64String(document.Data));
                            document.AttachmentFile_URL = baseUrl + "/Attachments/" + document.FileName;
                            document.Uploaded_Date = DateTime.Now;
                            document.UserId = userId;
                        }
                    }
                }
                Shipment currentShipment = DAL_AccessLayer.FindShipmentInfo(shipmentDetails.Id);
                CheckShipmentChange(shipmentDetails, currentShipment, userId, -1, -1);
                DAL_AccessLayer.UpdateShipmentInfo(shipmentDetails);

                return "The shipment has been successfully updated.";
            }
            catch (Exception ex)
            {
                return "Something Went Wrong!, The shipment update has failed : " + ex.Message;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("api/bhskapi/splitshipment")]
        public HttpResponseMessage SplitShipment([FromBody] SplitShipment splitShipment)
        {
            try
            {

                var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
                var UserId = identity.Name;

                DAL_AccessLayer.ShipmentInfo_Split(splitShipment.Id, splitShipment.SplitNumCrates);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("api/bhskapi/getconfigurationdetails")]

        public IEnumerable<ApplicationModel.ConfigurationDetails> GetConfigurationDetails([Optional] string Name, [Optional] string Type, [Optional] string Value, [Optional] string Search_keyword, int Page)
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
                            for (int r1 = 0; r1 <= ds.Tables[0].Rows.Count - 1; r1++)
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
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                typeMessage = ds.Tables[0].Rows[0][0].ToString();
                                string MessageBody = ds.Tables[0].Rows[0][1].ToString();
                                if (typeMessage == "extendedTextMessage")
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
                        final = idInstance + '~' + wid + '~' + timestamp + '~' + idMessage + '~' + chatId + '~' + chatName + '~' + senderName + '~' + typeMessage + '~' + textMessage;
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