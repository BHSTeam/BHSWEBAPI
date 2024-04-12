 
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Config.Helpers
{
    public class Config
    {
        

        /// <summary>
        /// Connection into CRM_API as a default connection site-less
        /// </summary>
        public static string DefaultConnection
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            }
        }

        /// <summary>
        /// Connection into CRM_API as a default connection site-less
        /// </summary>
        public static string PlannerDBConnection
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["PlannerDBConnection"].ToString();
            }
        }
        public static string BHSDBConnection
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["BHSDBConnection"].ToString();
            }
        }
        public static string ServerName
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ServerName"].ToString();
            }
        }

    }
}