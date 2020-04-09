using Management.Business.Business;
using Management.Services.StandardService;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Management.Services.Services
{
    public class RolesService
    {
        /// <summary>
        /// Consult All Roles
        /// </summary>
        /// <returns>List of Roles</returns>
        public List<Role> Consult()
        {
            var roleList = new List<Role>();
            string connectionString = new StandardConnection().GetConnectionString();
            StringBuilder str = new StringBuilder()
                    .AppendLine("SELECT * FROM Roles ORDER BY Description ASC");
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(str.ToString(), connection);
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    // Call Read before accessing data.
                    while (reader.Read())
                    {
                        var role = new Role()
                        {
                            Id = decimal.Parse(reader.GetValue(reader.GetOrdinal("Id")).ToString()),
                            Description = reader.GetValue(reader.GetOrdinal("Description")).ToString(),
                        };
                        roleList.Add(role);
                    }

                    reader.Close();
                }

                return roleList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
