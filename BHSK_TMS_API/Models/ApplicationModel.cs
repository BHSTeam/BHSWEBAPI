
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using System.Data.Entity;
using System.Globalization;
using System.Web.Mvc;
using System.Web.WebPages.Html;
using System.Web;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace BHSK_TMS_API.ApplicationModel
{
    public class ErrorMsg
    {
        public int ErrorCode1 { get; set; }
        public string ErrorMsg1 { get; set; }
        public bool isSuccessful1 { get; set; }
    }

    public class DutyINOUT_Info
    {
        public List<DutyTime> DutyOnOutList { get; set; }
        public ErrorMsg ErrorMsg { get; set; }
    }
    public class DutyTime
    {
        [Key]
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DutyIN_Time { get; set; }
        public string DutyOUT_Time { get; set; }
        public float DutyIn_Latitude { get; set; }
        public float DutyIn_Longitude { get; set; }
        public float DutyOut_Latitude { get; set; }
        public float DutyOut_Longitude { get; set; }

        internal class ApplicationModel
        {
        }
    }
    public class JobRequests
    {
        public string job_id { get; set; }
        public string job_seq_id { get; set; }
        public string job_category { get; set; }
        public string job_type { get; set; }
        public string preload_day_type { get; set; }
        public string origin_customer_name { get; set; }
        public string destination_customer_name { get; set; }
        public string expected_time_duration_loading { get; set; }
        public string expected_time_duration_unloading { get; set; }
        public string creation_datetime { get; set; }
        public string targeted_pickup_datetime { get; set; }
        public string targeted_delivery_datetime { get; set; }
        public string planned_pickup_datetime { get; set; }
        public string planned_delivery_datetime { get; set; }
        public string actual_pickup_datetime { get; set; }
        public string actual_delivery_datetime { get; set; }
        public string driver_assigned { get; set; }
        public string prime_mover_assigned { get; set; }
        public string trailer_assigned { get; set; }
        public string lorry_assigned { get; set; }
        public string job_total_volume_in_m3 { get; set; }
        public string job_total_weight_in_kg { get; set; }
        public string job_number_of_crates { get; set; }
        public string job_max_length { get; set; }
        public string job_max_width { get; set; }
        public string job_max_height { get; set; }
        public string job_allow_rotate_binary { get; set; }
        public string required_trailer_ids { get; set; }
        public string required_trailer_type { get; set; }
        public string required_trailer_size { get; set; }
        public string required_trailer_facility { get; set; }
        public string required_trailer_axle_type { get; set; }
        public string required_number_of_axles { get; set; }
        public string job_time_sensitive_binary { get; set; }
        public string allow_co_load_binary { get; set; }
        public string allow_stacking_binary { get; set; }
        public string job_remarks { get; set; }
        public string current_job_status { get; set; }
        public string job_document_ready { get; set; }
        public string job_cargo_ready { get; set; }
        public string required_job_skills { get; set; }
        public string require_airport_pass { get; set; }
        public string required_lorry_size { get; set; }
        public string job_sub_category { get; set; }

    }

    public class PrimeMoversInfo
    {

        public string prime_mover_id { get; set; }
        public string prime_mover_category { get; set; }
        public string prime_mover_type { get; set; }
        public float parking_location_postal_code { get; set; }
        public float parking_location_longitude { get; set; }
        public string parking_location_latitude { get; set; }
        public string prime_mover_available_time_today { get; set; }
        public string prime_mover_available_time_tomorrow { get; set; }
        public string prime_mover_current_status { get; set; }
        public string prime_mover_current_milestone { get; set; }
        public float prime_mover_current_location_longitude { get; set; }
        public float prime_mover_current_location_latitude { get; set; }
    }

    public class DriversInfo
    {
        public string driver_id { get; set; }
        public string driver_name { get; set; }
        public string driver_available_time_today { get; set; }
        public string driver_available_time_tomorrow { get; set; }
        public string current_driver_status { get; set; }
        public string driving_skills { get; set; }
        public string job_skills { get; set; }
        public string airport_pass_status { get; set; }
        public string driver_category { get; set; }


    }
    public class CustomerInfo
    {
        public string customer_name { get; set; }
        public string customer_category { get; set; }
        public string customer_postal_code { get; set; }
        public string customer_zone { get; set; }
        public float customer_location_longitude { get; set; }
        public float customer_location_latitude { get; set; }
        public float customer_accessibility_turning_radius_limit { get; set; }
        public float customer_accessibility_height_limit { get; set; }
        public int customer_time_sensitive_binary { get; set; }
        public int customer_allow_co_load_binary { get; set; }
        public int customer_allow_stacking_binary { get; set; }
        public float customer_current_queue_time { get; set; }

    }


    public class LorryInfo
    {
        public string lorry_id { get; set; }
        public string lorry_category { get; set; }
        public string lorry_size { get; set; }
        public string lorry_type { get; set; }
        public string parking_location_postal_code { get; set; }
        public float parking_location_longitude { get; set; }
        public float parking_location_latitude { get; set; }
        public string lorry_available_time_today { get; set; }
        public string lorry_avaliable_time_tomorrow { get; set; }
        public string lorry_current_status { get; set; }
        public string lorry_current_milestone { get; set; }
        public string lorry_current_location_longitude { get; set; }
        public string lorry_current_location_latitude { get; set; }

    }
    public class TrailerInfo
    {
        public string trailer_id { get; set; }
        public string trailer_category { get; set; }
        public string parking_location_postal_code { get; set; }
        public float parking_location_longitude { get; set; }
        public float parking_location_latitude { get; set; }
        public string trailer_type { get; set; }
        public string trailer_size { get; set; }
        public string trailer_facility { get; set; }
        public string trailer_axle_type { get; set; }
        public int trailer_number_of_axle { get; set; }
        public int trailer_door_length { get; set; }
        public int trailer_length { get; set; }
        public int trailer_width { get; set; }
        public int trailer_height { get; set; }
        public int trailer_door1_length { get; set; }
        public int trailer_door1_width { get; set; }
        public int trailer_door1_height { get; set; }
        public int trailer_door2_length { get; set; }
        public int trailer_door2_width { get; set; }
        public int trailer_door2_height { get; set; }
        public int trailer_rear_door_width { get; set; }
        public int trailer_rear_door_height { get; set; }
        public string trailer_capacity_by_volume { get; set; }
        public string trailer_available_time_today { get; set; }
        public string trailer_available_time_tomorrow { get; set; }
        public string trailer_current_status { get; set; }
        public string trailer_current_milestone { get; set; }
        public float trailer_current_location_longitude { get; set; }
        public float trailer_current_location_latitude { get; set; }
    }
    public class EmployeeDetails
    {

        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DutyIN_Time { get; set; }
        public string DutyOUT_Time { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
    public class GPSData
    {

        public string DeviceId { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string Iginition_Status { get; set; }

        public int GPS_Status { get; set; }
        public string Location { get; set; }
    }

    public class JobStatusActivities
    {
        public string all_jobs { get; set; }
        public string un_assigned { get; set; }
        public string assigned { get; set; }
        public string loading { get; set; }
        public string pickedup { get; set; }
        public string unloading { get; set; }
        public string delivered { get; set; }
        public string exception { get; set; }
        public string workgroup { get; set; }
        public string timestamp { get; set; }

    }

    public class WhatsAppMessage
    {

        public string typeWebhook { get; set; }
        public string idInstance { get; set; }
        public string wid { get; set; }
        public string timestamp { get; set; }

        public string idMessage { get; set; }
        public string chatId { get; set; }
        public string chatName { get; set; }
        public string sender { get; set; }
        public string senderName { get; set; }
        public string typeMessage { get; set; }
        public string textMessage { get; set; }

    }
    public class cOutMessage
    {
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Empcode { get; set; }
        public int Record_Status { get; set; }

    }
    public class cOutMessageDetails
    {
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }

    }
    public class UserInfo
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string EmailId { get; set; }
        public string Contact { get; set; }
        public string Forwarder { get; set; }
        public string Workgroup { get; set; }
        public string Createdby { get; set; }
    }
    public class UserInfoAdd
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string EmailId { get; set; }
        public string Contact { get; set; }
        public string Forwarder { get; set; }
        public string Workgroup { get; set; }
        public string Createdby { get; set; }

    }

    public class SplitShipment
    {
        public int Id { get; set; }
        public int SplitNumCrates { get; set; }
    }

    public class ShipmentDetails
    {
        public int Id { get; set; }
        public int ToolId { get; set; }
        public string EQPID { get; set; }
        public string TradeTerm { get; set; }
        public string Country { get; set; }
        public string Forwarder { get; set; }
        public bool Temperature { get; set; }
        public bool Humidity { get; set; }
        public bool Permit { get; set; }
        public bool Escort { get; set; }
        public string Mode { get; set; }
        public int TotalArea { get; set; }
        public int NumCrates { get; set; }
        public int TotalVolume { get; set; }
        public int TotalWeight { get; set; }
        public DateTime? Pickup_Planned { get; set; }
        public DateTime? Pickup_Actual { get; set; }
        public string FlightVesselNumber { get; set; }
        public string AirShippingLine { get; set; }
        public DateTime? FlightVessel_ETD { get; set; }
        public DateTime? FlightVessel_ATD { get; set; }
        public string Transit { get; set; }
        public DateTime? Transit_ETA { get; set; }
        public DateTime? Transit_ATA { get; set; }
        public DateTime? Transit_ETD { get; set; }
        public DateTime? Transit_ATD { get; set; }
        public string MasterAWB { get; set; }
        public string HAWB { get; set; }
        public DateTime? Planned_SG_Arrival { get; set; }
        public bool Confirm_SG_Arrival { get; set; }
        public DateTime? Actual_SG_Arrival { get; set; }
        public bool Delayed { get; set; }
        public string DelayedReason { get; set; }
        public bool DocumentReady { get; set; }
        public bool CargoReady { get; set; }
        public bool DualPickup { get; set; }
        public Collection<Attachment> Documents { get; set; }
        public Collection<DamageDetails> Damages { get; set; }
    }

    public class Attachment
    {
        public int Id { get; set; }
        public int Shipment_Id { get; set; }
        public string AttachmentFile_URL { get; set; }
        public DateTime? Uploaded_Date { get; set; }
        public string FileName { get; set; }
        public string Data { get; set; }
        public string UserId { get; set; }
    }

    public class MainTool
    {
        public int Id { get; set; }
        public string PONumber { get; set; }
        public string TradeTerm { get; set; }
        public string EQPID { get; set; }
        public string VEQPID { get; set; }
        public string Vendor { get; set; }
        public string Entity { get; set; }
        public string Area { get; set; }
        public string Model { get; set; }
        public DateTime MIDate { get; set; }
        public DateTime Actual_MoveInDate { get; set; }
        public DateTime FCADate { get; set; }
        public DateTime Previous_FCA_Changes { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string Remarks { get; set; }
        public bool Priorty { get; set; }

    }


    public class ShipmentListDetails
    {
        public int Id { get; set; }
        public string EQPID { get; set; }
        public string MasterAWB { get; set; }
        public string HAWB { get; set; }
        public string TradeTerm { get; set; }
        public string Country { get; set; }
        public string Mode { get; set; }
        public int NumCrates { get; set; }
        public bool Temperature { get; set; }
        public bool Humidity { get; set; }
        public bool Permit { get; set; }
        public bool Escort { get; set; }
        public string Forwarder { get; set; }
    }

    public class ImportDetails
    {
        public int ImportId { get; set; }
        public DateTime ImportDate { get; set; }
        public string UserId { get; set; }
        public string Details { get; set; }
        public string Status { get; set; }
    }
    public class ImportErrorDetails
    {
        public int Id { get; set; }
        public int ImportId { get; set; }
        public int RowNumber { get; set; }
        public string Details { get; set; }

    }
    public class ImportUpdateDetails
    {
        public int ImportId { get; set; }
        public string Column_Name { get; set; }
        public string Active_Type { get; set; }
        public string RowNumber { get; set; }
        public string UserId { get; set; }
    }
    public class ConfigurationDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }


    }
    public class DamageDetails
    {
        public int Id { get; set; }
        public int Shipment_Id { get; set; }
        public int CrateNum { get; set; }
        public string Location { get; set; }
        public string DamageType { get; set; }
        public Collection<DamagePhotos> DamagePhotos { get; set; }
    }
    public class DamagePhotos
    {
        public int Id { get; set; }
        public int Damage_Id { get; set; }
        public string Photo_URL { get; set; }
        public DateTime? Uploaded_Date { get; set; }
        public string FileName { get; set; }
        public string UserId { get; set; }
        public string Data {  get; set; }
    }
}