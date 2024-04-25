using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.IO;
using BHSK_TMS_API.ApplicationModel;
using System.Web.Services.Description;
using ExcelDataReader.Log;
using System.Xml.Linq;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using Newtonsoft.Json.Linq;
namespace BHSK_TMS_API
{
    public class DAL_AccessLayer
    {
        public static BHSK_TMS_API.ApplicationModel.cOutMessage Update_DutyONOFF_Info(BHSK_TMS_API.ApplicationModel.DutyTime obj)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();

                    var result = conn.Query<BHSK_TMS_API.ApplicationModel.cOutMessage>(
                            "Sp_Update_Duty_ONOFF_QHR", new
                            {
                                @Employee_Code = obj.EmployeeCode,
                                @Employee_Name = obj.EmployeeName,
                                @DutyIn_Datetime = obj.DutyIN_Time,
                                @DutyOut_Datetime = obj.DutyOUT_Time,
                                @DutyIn_Latitude = obj.DutyIn_Latitude,
                                @DutyIn_Longitude = obj.DutyIn_Longitude,
                                @DutyOut_Latitude = obj.DutyOut_Latitude,
                                @DutyOut_Longitude = obj.DutyOut_Longitude,
                            }, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("BHS_NewSlotUpdate_V1 : " + ex.Message);
            }
        }
        public static List<EmployeeDetails> GET_VehicleCheckList(string UserId, string type)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();

                    var result = conn.Query<EmployeeDetails>(
                            "sp_Get_WebAPP_VehicleCheckList", new
                            {
                                @UserId = UserId,
                                @From = type
                            }, commandType: CommandType.StoredProcedure).ToList();

                    return result;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(" sp_Get_WebAPP_VehicleCheckList : " + ex.Message);
            }
        }
        public static cOutMessage UpdateCheckListDetails(string UserId, string Vehicleno, string TrailerNo, string SelectDate, string InputData)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();

                    var result = conn.Query<cOutMessage>(
                            "sp_CheckList_Answer_Details_Update", new
                            {
                                @UserId = UserId,
                                @Vehicleno = Vehicleno,
                                @TrailerNo = TrailerNo,
                                @SelectDate = SelectDate,
                                @InputData = InputData
                            }, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    return result;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(" sp_Insert_Update_MastersDetails : " + ex.Message);
            }
        }

        public static cOutMessage Employee_Duty_IN_OUT_Update(string EmployeeCode, string EmployeeName, DateTime DutyIn_Datetime, DateTime DutyOut_Datetime, float DutyIn_Latitude, float DutyIn_Longitude, float DutyOut_Latitude, float DutyOut_Longitude)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();

                    var result = conn.Query<cOutMessage>(
                            "Sp_Update_Duty_ONOFF_QHR", new
                            {
                                @Employee_Code = EmployeeCode,
                                @Employee_Name = EmployeeName,
                                @DutyIn_Datetime = DutyIn_Datetime,
                                @DutyOut_Datetime = DutyOut_Datetime,
                                @DutyIn_Latitude = DutyIn_Latitude,
                                @DutyIn_Longitude = DutyIn_Longitude,
                                @DutyOut_Latitude = DutyOut_Latitude,
                                @DutyOut_Longitude = DutyOut_Longitude,
                            }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(" Sp_Update_Duty_ONOFF_QHR : " + ex.Message);
            }
        }
        public static List<ApplicationModel.JobRequests> Get_JobRequests_aStar(string InputData1, string InputData2, string InputData3)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();
                    var result = conn.Query<ApplicationModel.JobRequests>(
                    "sp_Get_JobRequests_aStar", new
                    {
                        @InputData1 = InputData1,
                        @InputData2 = InputData2,
                        @InputData3 = InputData3,
                    }, commandType: CommandType.StoredProcedure).ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(" sp_Get_sp_JobRequests_aStar : " + ex.Message);
            }
        }

        public static List<ApplicationModel.JobRequests> Get_JobRequests_aStarWithoutPara()
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();
                    var result = conn.Query<ApplicationModel.JobRequests>(
                    "sp_Get_sp_JobRequests_aStar", new
                    {
                    }, commandType: CommandType.StoredProcedure).ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(" sp_Get_sp_JobRequests_aStar : " + ex.Message);
            }
        }

        public static List<ApplicationModel.PrimeMoversInfo> Get_PrimeMoversView_aStar(int Opt, string InputData1)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();
                    var result = conn.Query<ApplicationModel.PrimeMoversInfo>(
                    "sp_Get_AllMastersView_aStar", new
                    {
                        @Opt = Opt,
                        @InputValue1 = InputData1,

                    }, commandType: CommandType.StoredProcedure).ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(" sp_Get_sp_AllMastersView_aStar : " + ex.Message);
            }
        }
        public static List<ApplicationModel.LorryInfo> Get_LorryView_aStar(int Opt, string InputData1)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();
                    var result = conn.Query<ApplicationModel.LorryInfo>(
                    "sp_Get_AllMastersView_aStar", new
                    {
                        @Opt = Opt,
                        @InputValue1 = InputData1,

                    }, commandType: CommandType.StoredProcedure).ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(" sp_Get_sp_AllMastersView_aStar : " + ex.Message);
            }
        }
        public static List<ApplicationModel.TrailerInfo> Get_TrailerView_aStar(int Opt, string InputData1)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();
                    var result = conn.Query<ApplicationModel.TrailerInfo>(
                    "sp_Get_AllMastersView_aStar", new
                    {
                        @Opt = Opt,
                        @InputValue1 = InputData1,

                    }, commandType: CommandType.StoredProcedure).ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(" sp_Get_sp_AllMastersView_aStar : " + ex.Message);
            }
        }
        public static List<ApplicationModel.CustomerInfo> Get_CustomersView_aStar(int Opt, string InputData1)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();
                    var result = conn.Query<ApplicationModel.CustomerInfo>(
                    "sp_Get_AllMastersView_aStar", new
                    {
                        @Opt = Opt,
                        @InputValue1 = InputData1,

                    }, commandType: CommandType.StoredProcedure).ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(" sp_Get_sp_AllMastersView_aStar : " + ex.Message);
            }
        }
        public static List<ApplicationModel.DriversInfo> Get_DriversView_aStar(int Opt, string InputData1)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();
                    var result = conn.Query<ApplicationModel.DriversInfo>(
                    "sp_Get_AllMastersView_aStar", new
                    {
                        @Opt = Opt,
                        @InputValue1 = InputData1,

                    }, commandType: CommandType.StoredProcedure).ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(" sp_Get_sp_AllMastersView_aStar : " + ex.Message);
            }
        }
        public static cOutMessage ForgotPasswordVerification(string UserEmailId, string TempPassword, string TempEncryptPwd)
        {

            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();

                    var result = conn.Query<cOutMessage>(
                            "sp_Validate_ForgotPassword_UMC", new
                            {
                                @UserId = UserEmailId,
                                @TempPassword = TempPassword,
                                @TempEncryptPwd = TempEncryptPwd
                            }, commandType: CommandType.StoredProcedure).FirstOrDefault();


                    return result;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(" ForgotPassword: Verification : " + ex.Message);
            }
        }
        public static List<ApplicationModel.JobStatusActivities> Get_Jobactivities_aStar(string starttime, string endtime, string workgroup)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();
                    var result = conn.Query<ApplicationModel.JobStatusActivities>(
                    "sp_Get_JobstatusActivitiesView_aStar", new
                    {
                        @StartTime = starttime,
                        @EndTime = endtime,
                        @Workgroup = workgroup,

                    }, commandType: CommandType.StoredProcedure).ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(" sp_Get_sp_AllMastersView_aStar : " + ex.Message);
            }
        }
        public static List<ApplicationModel.UserInfo> CheckUserLogin(string UserName, string Password)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();
                    var result = conn.Query<ApplicationModel.UserInfo>(
                            "sp_GetLogin_API", new
                            {
                                @UserName = UserName,
                                @Password = Password
                            }, commandType: CommandType.StoredProcedure).ToList();

                    return result;

                }
            }
            catch (Exception ex)
            {
                throw new Exception("sp_GetLogin_API : " + ex.Message);
            }
        }
        public static cOutMessage Change_Reset_Password(string UserId, string OldPassword, string NewPassword, int Opt)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();
                    var result = conn.Query<cOutMessage>(
                            "sp_UserPassword_Change_API", new
                            {
                                @UserId = UserId,
                                @OldPassword = OldPassword,
                                @NewPassword = NewPassword,
                                @Opt = Opt
                            }, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    return result;

                }
            }
            catch (Exception ex)
            {
                throw new Exception("sp_UserPassword_Change_API : " + ex.Message);
            }
        }
        public static cOutMessage CheckUserLogin_API(string UserName, string Password)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();
                    var result = conn.Query<cOutMessage>(
                            "sp_GetLogin_API", new
                            {
                                @UserName = UserName,
                                @Password = Password
                            }, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    return result;

                }
            }
            catch (Exception ex)
            {
                throw new Exception("sp_GetLogin_API : " + ex.Message);
            }
        }
        public static List<ApplicationModel.UserInfo> UserList(string UserName, string EmailId, string Contact, string Workgroup, string Search_keyword, int Page, int Opt)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();
                    var result = conn.Query<ApplicationModel.UserInfo>(
                            "sp_Get_UserList_API", new
                            {
                                @UserName = UserName,
                                @EmailId = EmailId,
                                @Contact = Contact,
                                @Workgroup = Workgroup,
                                @Search_keyword = Search_keyword,
                                @Page = Page,
                                @Opt = Opt
                            }, commandType: CommandType.StoredProcedure).ToList();

                    return result;

                }
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Get_UserList_API : " + ex.Message);
            }
        }
        public static List<ApplicationModel.ConfigurationDetails> ConfigurationList(string Name, string Type, string Value, string Search_keyword, int Page, int Opt)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();
                    var result = conn.Query<ApplicationModel.ConfigurationDetails>(
                            "sp_Get_Configuration_UMC_API", new
                            {
                                @name = Name,
                                @type = Type,
                                @value = Value,
                                @Search_keyword = Search_keyword,
                                @Page = Page,
                                @Opt = Opt
                            }, commandType: CommandType.StoredProcedure).ToList();

                    return result;

                }
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Get_Configuration_UMC_API : " + ex.Message);
            }
        }

        public static List<MainTool> GetMainToolsList(string UserId, string Area, string Vendor, DateTime? FromDate, DateTime? ToDate, string Search_keyword, int Page, int Opt)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();
                    var result = conn.Query<MainTool>(
                            "sp_Get_MainToolsList_UMC_API", new
                            {
                                @UserId = UserId,
                                @Area = Area,
                                @Vendor = Vendor,
                                @FromDate = FromDate,
                                @ToDate = ToDate,
                                @Search_keyword = Search_keyword,
                                @Page = Page,
                                @Opt = Opt
                            }, commandType: CommandType.StoredProcedure).ToList();

                    return result;

                }
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Get_MainToolsList_UMC_API : " + ex.Message);
            }
        }
        public static List<ApplicationModel.ShipmentDetails> GetShipmentDetailsList(string UserId, string Eqpid, string TradeTerm, string Country, string Mode, string Search_keyword, int Page, int Opt)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();
                    List<ShipmentDetails> result = conn.Query<ApplicationModel.ShipmentDetails>(
                            "sp_Get_ShipmentsList_UMC_API", new
                            {
                                @UserId = UserId,
                                @Eqpid = Eqpid,
                                @TradeTerm = TradeTerm,
                                @Country = Country,
                                @Mode = Mode,
                                @Search_keyword = Search_keyword,
                                @Page = Page,
                                @Opt = Opt
                            }, commandType: CommandType.StoredProcedure).ToList();

                    foreach (ShipmentDetails item in result) {
                        item.Documents = new Collection<Attachment>(conn.Query<Attachment>("SELECT * FROM UMC_AttachmentDetails WHERE Shipment_Id=" + item.Id).ToList());
                        item.Damages = new Collection<DamageDetails>(conn.Query<DamageDetails>("SELECT * FROM UMC_Damages WHERE Shipment_Id=" + item.Id).ToList());
                        foreach (DamageDetails damageDetails in item.Damages)
                        {
                            damageDetails.DamagePhotos = new Collection<DamagePhotos>(conn.Query<DamagePhotos>("SELECT * FROM UMC_DamagePhotos WHERE Damage_Id=" + damageDetails.Id).ToList());
                        }
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Get_ShipmentsList_UMC_API : " + ex.Message);
            }
        }

        public static List<DamageDetails> GetDamageDetails(int Shipment_Id)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();
                    List<DamageDetails> result = conn.Query<DamageDetails>("SELECT * FROM UMC_Damages WHERE Shipment_Id=@ShipmentID", new { ShipmentId = Shipment_Id }).ToList();

                    foreach (DamageDetails damageDetails in result)
                    {
                        damageDetails.DamagePhotos = new Collection<DamagePhotos>(conn.Query<DamagePhotos>("SELECT * FROM UMC_DamagePhotos WHERE Damage_Id=" + damageDetails.Id).ToList());
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(" GetDamageDetails : " + ex.Message);
            }
        }

        public static void AddDamageDetails(DamageDetails damageDetails)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();

                    int damageId = (int)conn.ExecuteScalar("INSERT INTO UMC_Damages (CrateNum,Location,DamageType,Shipment_Id) OUTPUT INSERTED.ID VALUES (@CrateNum,@Location,@DamageType,@ShipmentID)", new { CrateNum = damageDetails.CrateNum, Location = damageDetails.Location, DamageType = damageDetails.DamageType, ShipmentID = damageDetails.Shipment_Id });

                    foreach (DamagePhotos damagePhotos in damageDetails.DamagePhotos)
                    {
                        conn.Execute("INSERT INTO UMC_DamagePhotos (Damage_Id, Photo_URL, Uploaded_Date, UserId, FileName) VALUES (@Damage_Id, @Photo_URL, @Uploaded_Date, @UserId, @FileName)", new { Damage_Id = damageId, Photo_URL = damagePhotos.Photo_URL, Uploaded_Date = damagePhotos.Uploaded_Date, UserId = damagePhotos.UserId, FileName = damagePhotos.FileName });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(" AddDamageDetails : " + ex.Message);
            }
        }

        public static void UpdateDamageDetails(DamageDetails damageDetails)
        {
            int rowsAffected = 0;
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();

                    rowsAffected = conn.Execute("UPDATE UMC_Damages SET CrateNum=@CrateNum,Location=@Location,DamageType=@DamageType WHERE Id=@Id", damageDetails);

                    foreach (DamagePhotos damagePhotos in damageDetails.DamagePhotos)
                    {
                        conn.Execute("UPDATE UMC_DamagePhotos SET Photo_URL=@Photo_URL,Uploaded_Date=@Uploaded_Date,UserId=@UserId,FileName=@FileName WHERE Id=@Id", damagePhotos);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("UpdateDamageDetails : " + ex.Message);
            }
            if (rowsAffected <= 0)
            {
                throw new KeyNotFoundException();
            }
        }

        public static void DeleteDamageDetails(DamageDetails damageDetails)
        {
            int rowsAffected = 0;
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();

                    rowsAffected = conn.Execute("DELETE UMC_Damages WHERE Id=@Id", damageDetails);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DeleteDamageDetails : " + ex.Message);
            }
            if (rowsAffected <= 0)
            {
                throw new KeyNotFoundException();
            }
        }

        public static void DeleteDamagePhoto(DamagePhotos damagePhotos)
        {
            int rowsAffected = 0;
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();

                    rowsAffected = conn.Execute("DELETE UMC_DamagePhotos WHERE Id=@Id", damagePhotos);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DeleteDamagePhoto : " + ex.Message);
            }
            if (rowsAffected <= 0)
            {
                throw new KeyNotFoundException();
            }
        }

        public static void DeleteAttachment(Attachment attachment)
        {
            int rowsAffected = 0;
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();

                    rowsAffected = conn.Execute("DELETE UMC_AttachmentDetails WHERE Id=@Id", attachment);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DeleteAttachment : " + ex.Message);
            }
            if (rowsAffected <= 0)
            {
                throw new KeyNotFoundException();
            }
        }

        public static List<ApplicationModel.ImportDetails> GetImportDetails(int Page, string Search_keyword, int Opt)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();
                    var result = conn.Query<ApplicationModel.ImportDetails>(
                            "sp_Get_ImportFiles_UMC_API", new
                            {
                                @ImportId = 0,
                                @Page = Page,
                                @Opt = Opt,
                                @Search_keyword = Search_keyword
                            }, commandType: CommandType.StoredProcedure).ToList();

                    return result;

                }
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Get_ImportFiles_UMC_API : " + ex.Message);
            }
        }
        public static List<ApplicationModel.ImportErrorDetails> GetImportErrorDetails(int ImportId, int Page, int Opt)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();
                    var result = conn.Query<ApplicationModel.ImportErrorDetails>(
                            "sp_Get_ImportFiles_UMC_API", new
                            {
                                @ImportId = ImportId,
                                @Page = Page,
                                @Opt = Opt,
                                @Search_keyword = ""
                            }, commandType: CommandType.StoredProcedure).ToList();

                    return result;

                }
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Get_ImportFiles_UMC_API : " + ex.Message);
            }
        }
        public static List<ApplicationModel.ImportDetailsLog> GetImportUpdateDetails(int ImportId, int Page, int Opt)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();
                    var result = conn.Query<ApplicationModel.ImportDetailsLog>(
                            "sp_Get_ImportFiles_UMC_API", new
                            {
                                @ImportId = ImportId,
                                @Page = Page,
                                @Opt = Opt,
                                @Search_keyword = ""
                            }, commandType: CommandType.StoredProcedure).ToList();

                    return result;

                }
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Get_ImportFiles_UMC_API : " + ex.Message);
            }
        }
        public static cOutMessage UploadAttachmentDetails(int ShipmentId, string AttachementFile, string UserId)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();
                    var result = conn.Query<cOutMessage>(
                            "sp_UMC_AttachementDetails", new
                            {
                                @Shipment_Id = ShipmentId,
                                @FileName = AttachementFile,
                                @UserId = UserId
                            }, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    return result;

                }
            }
            catch (Exception ex)
            {
                throw new Exception("sp_UMC_AttachementDetails : " + ex.Message);
            }
        }
        public static cOutMessage UserInfo_Add(string UserName, string Password, string Roles, string EmailId, string Contact, string Forwarder, string WorkGroup, string CreatedBy)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();

                    var result = conn.Query<cOutMessage>(
                            "sp_UserInfo_Add_API", new
                            {
                                @UserName = UserName,
                                @Password = Password,
                                @Roles = Roles,
                                @EmailId = EmailId,
                                @Contact = Contact,
                                @Forwarder = Forwarder,
                                @WorkGroup = WorkGroup,
                                @CreatedBy = CreatedBy

                            }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(" sp_UserInfo_Add_API : " + ex.Message);
            }
        }

        public static cOutMessage UserInfo_Update(string UserName, string Password, string Roles, string EmailId, string Contact, string Forwarder)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();

                    var result = conn.Query<cOutMessage>(
                            "sp_UserInfo_Update_API", new
                            {
                                @UserName = UserName,
                                @Password = Password,
                                @Roles = Roles,
                                @EmailId = EmailId,
                                @Contact = Contact,
                                @Forwarder = Forwarder,

                            }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(" sp_UserInfo_Add_API : " + ex.Message);
            }
        }

        public static cOutMessage UserInfo_Delete(string UserName)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();

                    var result = conn.Query<cOutMessage>(
                            "sp_UserInfo_Delete_API", new
                            {
                                @UserName = UserName
                            }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(" sp_UserInfo_Add_API : " + ex.Message);
            }
        }

        public static cOutMessage Configuration_Add_Update(int Id, string Name, string Type, string Value, int Opt, string CreatedBy)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();

                    var result = conn.Query<cOutMessage>(
                            "sp_UMC_Configuration_Insert_Update", new
                            {
                                @Id = Id,
                                @Name = Name,
                                @Type = Type,
                                @Value = Value,
                                @Opt = Opt,
                                @CreatedBy = CreatedBy
                            }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(" sp_UMC_Configuration_Insert_Update : " + ex.Message);
            }
        }

        public static cOutMessage Configuration_Delete(int Id)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();

                    var result = conn.Query<cOutMessage>(
                            "sp_UMC_Configuration_Delete", new
                            {
                                @Id = Id,
                            }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(" sp_UMC_Configuration_Delete : " + ex.Message);
            }
        }

        public static MainTool FindMainTool(string PONumber, string EQPID)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();

                    MainTool result = conn.Query<MainTool>("SELECT * FROM UMC_Tools WHERE PONumber=@PONumber AND EQPID=@EQPID", new { PONumber = PONumber, EQPID = EQPID }).FirstOrDefault();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(" FindMainTool : " + ex.Message);
            }
        }

        public static void UpdateMainTool(MainTool mainTool)
        {
            int rowsAffected = 0;
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();

                    rowsAffected = conn.Execute("UPDATE UMC_Tools SET EQPID=@EQPID,VEQPID=@VEQPID,Vendor=@Vendor,Entity=@Entity,Area=@Area,Model=@Model,MIDate=@MIDate,FCADate=@FCADate,Remarks=@Remarks," +
                        "Actual_MoveInDate=@Actual_MoveInDate,Previous_FCA_Changes=@Previous_FCA_Changes,CreateDateTime=@CreateDateTime,Priority=@Priority,PONumber=@PONumber,TradeTerm=@TradeTerm) WHERE Id=@Id", mainTool);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(" UpdateMainTool : " + ex.Message);
            }
            if (rowsAffected <= 0)
            {
                throw new KeyNotFoundException();
            }
        }

        public static void AddMainTool(MainTool mainTool)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();

                    int toolId = (int)conn.ExecuteScalar("INSERT INTO UMC_Tools (EQPID,VEQPID,Vendor,Entity,Area,Model,MIDate,FCADate,Remarks,Actual_MoveInDate,Previous_FCA_Changes,CreatedDatetime,Priority,PONumber,TradeTerm)" +
                        " OUTPUT INSERTED.ID VALUES (@EQPID,@VEQPID,@Vendor,@Entity,@Area,@Model,@MIDate,@FCADate,@Remarks,@Actual_MoveInDate,@Previous_FCA_Changes,@CreatedDateTime,@Priority,@PONumber,@TradeTerm)", mainTool);
                    mainTool.Id = toolId;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(" AddMainTool : " + ex.Message);
            }
        }

        public static void AddImportDetailsLog(ImportDetailsLog importDetailsLog)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();

                    conn.Execute("INSERT INTO UMC_Imports_DetailsLog (ImportId,Column_Name,Active_Type,RowNumber,UserId) OUTPUT INSERTED.ID VALUES (@ImportId,@Column_Name,@Active_Type,@RowNumber,@UserId)", importDetailsLog);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(" AddImportDetailsLog : " + ex.Message);
            }
        }

        public static void AddImport(ImportDetails importDetails)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();

                    int importId = (int)conn.ExecuteScalar("INSERT INTO UMC_Imports (ImportDate,UserId,Details,Status) OUTPUT INSERTED.ID VALUES (@ImportDate,@UserId,@Details,@Status)", importDetails);
                    importDetails.ImportId = importId;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(" AddImport : " + ex.Message);
            }
        }

        public static void UpdateImport(ImportDetails importDetails)
        {
            int rowsAffected = 0;
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();

                    rowsAffected = conn.Execute("UPDATE UMC_Imports SET Details=@Details, Status=@Status WHERE Id=@ImportId", importDetails);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(" UpdateImport : " + ex.Message);
            }
            if (rowsAffected <= 0)
            {
                throw new KeyNotFoundException();
            }
        }

        public static void AddImportError(ImportErrorDetails importErrorDetails)
        {
            int rowsAffected = 0;
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();

                    rowsAffected = conn.Execute("INSERT INTO UMC_Errors (ImportId,RowNumber,Details) VALUES (@ImportId,@RowNumber,@Details)", importErrorDetails);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(" AddImportError : " + ex.Message);
            }
            if (rowsAffected <= 0)
            {
                throw new KeyNotFoundException();
            }
        }

        public static cOutMessage Import_DetailsUpdate(int NewRec, int UpdRec, int Fail, int Opt, string ErrorsMsg, int RowNumber, string CreatedBy)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();

                    var result = conn.Query<cOutMessage>(
                            "sp_UMC_ImportDetails_Insert", new
                            {
                                @NewCount = NewRec,
                                @UpdateCount = UpdRec,
                                @FailCount = Fail,
                                @Opt = Opt,
                                @Errors = ErrorsMsg,
                                @RowNumber = RowNumber,
                                @CreatedBy = CreatedBy
                            }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(" sp_UMC_ImportDetails_Insert : " + ex.Message);
            }
        }

        public static cOutMessage ShipmentInfo_Add(string PONumber, string EQPID, string VEQPID, string Vendor, string Entity, string Area, string Model, DateTime MIDate, DateTime FCADate, string Remarks, string TradeTerm, string Country, string Mode, string TempContol, string Humidity, string M3Val1, string M3Val2, string M3Val3, string Permit, string Esscorts, string Forwarder, string Status, int RowNumber, string CreatedBy)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();

                    var result = conn.Query<cOutMessage>(
                            "sp_UMC_ShipmentDetails_Insert_Update", new
                            {
                                @PONumber = PONumber,
                                @EQPID = EQPID,
                                @VEQPID = VEQPID,
                                @Vendor = Vendor,
                                @Entity = Entity,
                                @Area = Area,
                                @Model = Model,
                                @MIDate = MIDate,
                                @FCADate = FCADate,
                                @Remarks = Remarks,
                                @TradeTerm = TradeTerm,
                                @Country = Country,
                                @Mode = Mode,
                                @TempContol = TempContol,
                                @Humidity = Humidity,
                                @M3Val1 = M3Val1,
                                @M3Val2 = M3Val2,
                                @M3Val3 = M3Val3,
                                @Permit = Permit,
                                @Esscorts = Esscorts,
                                @Forwarder = Forwarder,
                                @Status = Status,
                                @RowNumber = RowNumber,
                                @CreatedBy = CreatedBy

                            }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(" sp_UMC_ShipmentDetails_Insert_Update : " + ex.Message);
            }
        }

        public static ShipmentDetails FindShipmentInfo(int toolId)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();

                    ShipmentDetails result = conn.Query<ShipmentDetails>("SELECT * FROM UMC_Shipments WHERE ToolId=@ToolId", new { ToolId = toolId }).FirstOrDefault();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(" FindShipmentInfo : " + ex.Message);
            }
        }

        public static void UpdateShipmentInfo(ShipmentDetails shipmentDetails)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();

                    SqlTransaction transaction = conn.BeginTransaction();
                    conn.Execute("UPDATE UMC_Shipments" +
                        " SET EQPID=@EQPID,Country=@Country,Forwarder=@Forwarder,Temperature=@Temperature,Humidity=@Humidity,Permit=@Permit,Escort=@Escort,Mode=@Mode,TotalArea=@TotalArea," +
                        "NumCrates=@NumCrates,TotalVolume=@TotalVolume,TotalWeight=@TotalWeight,Pickup_Planned=@Pickup_Planned,Pickup_Actual=@Pickup_Actual,AirShippingLine=@AirShippingLine,FlightVesselNumber=@FlightVesselNumber," +
                        "FlightVessel_ETD=@FlightVessel_ETD,FlightVessel_ATD=@FlightVessel_ATD,Transit=@Transit,Transit_ETA=@Transit_ETA,Transit_ATA=@Transit_ATA,Transit_ETD=@Transit_ETD,Transit_ATD=@Transit_ATD," +
                        "SG_ETA=@Planned_SG_Arrival,Confirm_SG_ETA=@Confirm_SG_Arrival,SG_ATA=@Actual_SG_Arrival,DocumentReady=@DocumentReady,CargoReady=@CargoReady,Delayed=@Delayed,DelayedReason=@DelayedReason,DualPickup=@DualPickup" +
                        " WHERE id=@Id",
                        shipmentDetails,
                        transaction
                        );

                    foreach (Attachment document in shipmentDetails.Documents)
                    {
                        int count = (int)conn.ExecuteScalar("SELECT COUNT(*) FROM UMC_AttachmentDetails WHERE Id=@Id", document, transaction);
                        if (count <= 0)
                        {
                            document.Shipment_Id = shipmentDetails.Id;
                            conn.Execute("INSERT INTO UMC_AttachmentDetails (Shipment_Id,AttachmentFile_URL,Uploaded_Date,UserId, FileName) VALUES (@Shipment_Id, @AttachmentFile_URL, @Uploaded_Date, @UserId, @FileName)",
                                document,
                                transaction);
                        }
                    }
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(" UpdateShipmentInfo : " + ex.Message);
            }
        }
        public static void AddShipmentInfo(ShipmentDetails shipmentDetails)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();

                    SqlTransaction transaction = conn.BeginTransaction();
                    int shipmentId = (int)conn.ExecuteScalar("INSERT INTO UMC_Shipments" +
                        " (EQPID,Country,Forwarder,Temperature,Humidity,Permit,Escort,Mode,TotalArea,NumCrates,TotalVolume,TotalWeight,Pickup_Planned,Pickup_Actual,AirShippingLine,FlightVesselNumber," +
                        "FlightVessel_ETD,FlightVessel_ATD,Transit,Transit_ETA,Transit_ATA,Transit_ETD,Transit_ATD,SG_ETA,Confirm_SG_ETA,SG_ATA,DocumentReady,CargoReady,Delayed,DelayedReason,DualPickup,ToolId)" +
                        " OUTPUT INSERTED.ID" +
                        " VALUES (@EQPID,@Country,@Forwarder,@Temperature,@Humidity,@Permit,@Escort,@Mode,@TotalArea,@NumCrates,@TotalVolume,@TotalWeight,@Pickup_Planned,@Pickup_Actual,@AirShippingLine," +
                        "@FlightVesselNumber,@FlightVessel_ETD,@FlightVessel_ATD,@Transit,@Transit_ETA,@Transit_ATA,@Transit_ETD,@Transit_ATD,@Planned_SG_Arrival,@Confirm_SG_Arrival,@Actual_SG_Arrival," +
                        "@DocumentReady,@CargoReady,@Delayed,@DelayedReason,@DualPickup,@ToolId)",
                        shipmentDetails,
                        transaction
                        );

                    if (shipmentDetails.Documents != null)
                    {
                        foreach (Attachment document in shipmentDetails.Documents)
                        {
                            document.Shipment_Id = shipmentId;
                            conn.Execute("INSERT INTO UMC_AttachmentDetails (Shipment_Id,AttachmentFile_URL,Uploaded_Date,UserId, FileName) VALUES (@Shipment_Id, @AttachmentFile_URL, @Uploaded_Date, @UserId, @FileName)",
                                document,
                                transaction);
                        }
                    }
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(" AddShipmentInfo : " + ex.Message);
            }
        }
        public static void ShipmentInfo_Split(int id, int splitNumCrates)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();

                    SqlTransaction transaction = conn.BeginTransaction();
                    int numCrates = (int)conn.ExecuteScalar("SELECT NumCrates FROM UMC_Shipments WHERE Id=@ShipmentID",
                        new
                        {
                            ShipmentID = id,
                        },
                        transaction
                        );

                    if (numCrates > splitNumCrates)
                    {
                        conn.Execute("UPDATE UMC_Shipments SET NumCrates = NumCrates - @SplitCrates WHERE Id=@ShipmentID",
                            new
                            {
                                SplitCrates = splitNumCrates,
                                ShipmentID = id,
                            },
                            transaction
                            );
                        conn.Execute("INSERT INTO UMC_Shipments (EQPID, ToolId, TradeTerm, Country, Forwarder, Temperature, Humidity, Permit, Escort, Mode, TotalArea, NumCrates, TotalVolume, TotalWeight, Pickup_Planned," +
                            " Pickup_Actual, AirShippingLine, MasterAWB, HAWB)" +
                            " SELECT EQPID, ToolId, TradeTerm, Country, Forwarder, Temperature, Humidity, Permit, Escort, Mode, TotalArea, @SplitCrates, TotalVolume, TotalWeight, Pickup_Planned, Pickup_Actual, AirShippingLine," +
                            " MasterAWB, HAWB FROM UMC_Shipments" +
                            " WHERE  id=@ShipmentID",
                            new
                            {
                                SplitCrates = splitNumCrates,
                                ShipmentID = id,
                            },
                            transaction
                            );
                    }
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(" ShipmentInfo_Split : " + ex.Message);
            }
        }

        public static cOutMessage POD_Upload_Details_Insert(string DrivID, string TripNo, string JobNo, string Img_Name, string Img_type, string Name, string Status, string Lat, string Long, string Location, string PhotoSize)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();

                    var result = conn.Query<cOutMessage>(
                            "BHS_MOB_PODUpload", new
                            {
                                @DrivID = DrivID,
                                @TripNo = TripNo,
                                @JobNo = JobNo,
                                @Img_Name = Img_Name,
                                @Img_type = Img_type,
                                @Name = Name,
                                @Status = Status,
                                @Lat = Lat,
                                @Long = Long,
                                @Location = Location,
                                @PhotoSize = PhotoSize,

                            }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(" Sp_Update_Duty_ONOFF_QHR : " + ex.Message);
            }
        }
    }
}