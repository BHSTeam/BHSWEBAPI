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
                                @InputData= InputData
                            }, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    return result;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(" sp_Insert_Update_MastersDetails : " + ex.Message);
            }
        }
        
        public static cOutMessage Employee_Duty_IN_OUT_Update(string EmployeeCode,string EmployeeName, DateTime DutyIn_Datetime, DateTime DutyOut_Datetime, float DutyIn_Latitude, float DutyIn_Longitude, float DutyOut_Latitude, float DutyOut_Longitude)
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
        public static List<ApplicationModel.JobRequests> Get_JobRequests_aStar(string InputData1,string InputData2,string InputData3)
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
        public static List<ApplicationModel.JobStatusActivities> Get_Jobactivities_aStar(string starttime,string endtime,string workgroup)
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
        public static cOutMessage Change_Reset_Password(string UserId, string OldPassword,string NewPassword, int Opt)
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
                                @EmailId= EmailId,
                                @Contact= Contact,
                                @Workgroup= Workgroup,
                                @Search_keyword= Search_keyword,
                                @Page= Page,
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

        public static List<ApplicationModel.MainToolsList> GetMainToolsList(string Area, string Vendor, string Fromdate, string Todate, string Search_keyword, int Page, int Opt)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();
                    var result = conn.Query<ApplicationModel.MainToolsList>(
                            "sp_Get_MainToolsList_UMC_API", new
                            {
                                @Area = Area,
                                @Vendor = Vendor,
                                @FromeDate = Fromdate,
                                @ToDate = Todate,
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
        public static List<ApplicationModel.ShipmentListDetails> GetShipmentDetailsList(string Eqpid, string TradeTerm, string Country, string Mode, string Search_keyword, int Page, int Opt)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();
                    var result = conn.Query<ApplicationModel.ShipmentListDetails>(
                            "sp_Get_ShipmentsList_UMC_API", new
                            {
                                @Eqpid = Eqpid,
                                @TradeTerm = TradeTerm,
                                @Country = Country,
                                @Mode = Mode,
                                @Search_keyword = Search_keyword,
                                @Page = Page,
                                @Opt = Opt
                            }, commandType: CommandType.StoredProcedure).ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Get_ShipmentsList_UMC_API : " + ex.Message);
            }
        }
        public static List<ApplicationModel.ImportDetails> GetImportDetails(int ImportId,int Page, int Opt)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();
                    var result = conn.Query<ApplicationModel.ImportDetails>(
                            "sp_Get_ImportFiles_UMC_API", new
                            {
                                @ImportId = ImportId,
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
        public static List<ApplicationModel.ImportUpdateDetails> GetImportUpdateDetails(int ImportId,int Page, int Opt)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();
                    var result = conn.Query<ApplicationModel.ImportUpdateDetails>(
                            "sp_Get_ImportFiles_UMC_API", new
                            {
                                @ImportId = ImportId,
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
        public static cOutMessage UserInfo_Add(string UserName, string Password, string Roles, string EmailId, string Contact, string WorkGroup, string CreatedBy)
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

        public static cOutMessage Configuration_Add_Update(int Id,string Name, string Type, string Value, int Opt,string CreatedBy)
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
        public static cOutMessage Import_DetailsUpdate(int NewRec, int UpdRec,int Fail,int Opt,string ErrorsMsg, int RowNumber,string CreatedBy)
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
                                @Opt= Opt,
                                @Errors=ErrorsMsg,
                                @RowNumber= RowNumber,
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
        public static cOutMessage ShipmentInfo_Add(string EQPID, string VEQPID, string Vendor, string Entity, string Area, string Model, string MIDate, string FCADate,string Remarks, string TradeTerm, string Country, string Mode, string TempContol, string Humidity, string M3Val1, string M3Val2, string M3Val3, string Permit, string Esscorts, string Forwarder, string Status,int RowNumber, string CreatedBy)
        {
            try
            {
                using (var conn = new SqlConnection(Config.Helpers.Config.BHSDBConnection))
                {
                    conn.Open();

                    var result = conn.Query<cOutMessage>(
                            "sp_UMC_ShipmentDetails_Insert_Update", new
                            {
                                @EQPID = EQPID,
                                @VEQPID = VEQPID,
                                @Vendor = Vendor,
                                @Entity = Entity,
                                @Area = Area,
                                @Model = Model,
                                @MIDate= MIDate,
                                @FCADate= FCADate,
                                @Remarks = Remarks,
                                @TradeTerm = TradeTerm,
                                @Country =Country,
                                @Mode = Mode,
                                @TempContol= TempContol,
                                @Humidity = Humidity,
                                @M3Val1= M3Val1,
                                @M3Val2= M3Val2,
                                @M3Val3= M3Val3,
                                @Permit= Permit,
                                @Esscorts= Esscorts,
                                @Forwarder= Forwarder,
                                @Status = Status,
                                @RowNumber=RowNumber,
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
        public static cOutMessage POD_Upload_Details_Insert(string DrivID,string TripNo, string JobNo,string Img_Name,string Img_type,string Name,string Status,string Lat,string Long,string Location,string PhotoSize)
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