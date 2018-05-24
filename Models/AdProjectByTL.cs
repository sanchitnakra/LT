using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;

namespace sampleMVC.Models
{
    public class AdProjectByTL
    {
        public int InsertProjectToDB(AdprojectDetails AdprojectDetails)
        {
            int row = 0;

            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("insert into LTProject values('" + AdprojectDetails.ProjectName + "')", conn);
                //   flag = Convert.ToBoolean(cmd.ExecuteScalar());
                row = cmd.ExecuteNonQuery();
                if (row > 0)
                {
                    return 1;
                }
                else { return 0; }
            }

            

        }


        public int InsertLocationToDB(AdlocationDetails AdlocationDetails)
        {
            int row = 0;

            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("insert into LTLocation values('" + AdlocationDetails.LocationName + "')", conn);
                //   flag = Convert.ToBoolean(cmd.ExecuteScalar());
                row = cmd.ExecuteNonQuery();
                if (row > 0)
                {
                    return 1;
                }
                else { return 0; }
            }



        }


        public int InsertBranchToDB(AdlocationDetails AdlocationDetails)
        {
            int row = 0;

            string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("insert into LTBranch values('" + AdlocationDetails.BranchName + "')", conn);
                //   flag = Convert.ToBoolean(cmd.ExecuteScalar());
                row = cmd.ExecuteNonQuery();
                if (row > 0)
                {
                    return 1;
                }
                else { return 0; }
            }



        }


    }
}