using Management.Business.Business;
using Management.Services.StandardService;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Management.Services.Services
{
    public class TagsService
    {
        /// <summary>
        /// connectionString to access database
        /// </summary>
        string connectionString = new StandardConnection().GetConnectionString();

        /// <summary>
        /// Insert a Tag
        /// </summary>
        /// <param name="obj">Tag object</param>
        /// <returns>boolean</returns>
        public bool Insert(Tag obj)
        {
            StringBuilder str = new StringBuilder()
                        .AppendFormat("INSERT INTO Tag_Roles (Description) VALUES ('{0}')", obj.Description);

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(str.ToString(), connection);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Consulte all Tags
        /// </summary>
        /// <returns>List of Tags Ordered by Description</returns>
        public List<Tag> Consult()
        {
            try
            {
                var tagList = new List<Tag>();
                StringBuilder str = new StringBuilder()
                        .AppendLine("SELECT * FROM Tag_Roles ORDER BY Description ASC");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(str.ToString(), connection);
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    // Call Read before accessing data.
                    while (reader.Read())
                    {
                        var tag = new Tag()
                        {
                            Id = decimal.Parse(reader.GetValue(reader.GetOrdinal("Id")).ToString()),
                            Description = reader.GetValue(reader.GetOrdinal("Description")).ToString(),
                        };
                        tagList.Add(tag);
                    }

                    reader.Close();
                }

                return tagList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// List Tags By Member's Id
        /// </summary>
        /// <param name="id">Id Member</param>
        /// <returns>List of Tags</returns>
        public List<Tag> FindTagsByMemberId(decimal id)
        {
            var tagList = new List<Tag>();

            StringBuilder str = new StringBuilder()
                    .AppendLine("SELECT T.* FROM Member_Tag M")
                    .AppendLine("INNER JOIN Tag_Roles T ON T.Id = M.Tag_Id")
                    .AppendFormat("WHERE M.Member_Id = {0}", id);
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
                        var tag = new Tag()
                        {
                            Id = decimal.Parse(reader.GetValue(reader.GetOrdinal("Id")).ToString()),
                            Description = reader.GetValue(reader.GetOrdinal("Description")).ToString(),
                        };
                        tagList.Add(tag);
                    }

                    // Call Close when done reading.
                    reader.Close();
                }

                return tagList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete Tags From Member's Id
        /// </summary>
        /// <param name="id">Member's Id</param>
        /// <returns>boolean</returns>
        public bool DeleteTagsByMemberId(decimal id)
        {
            StringBuilder str = new StringBuilder()
                    .AppendLine("DELETE FROM Member_Tag ")
                    .AppendFormat("WHERE Member_Id = {0}", id);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(str.ToString(), connection);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Insert Member's Tags 
        /// </summary>
        /// <param name="idTag">Id Tag</param>
        /// <param name="idMember">Id Member</param>
        public void InsertMemberTags(decimal idTag, decimal idMember)
        {
            StringBuilder str = new StringBuilder()
                        .AppendFormat("INSERT INTO Member_Tag (Member_Id, Tag_Id) VALUES ({0},{1})", idMember, idTag);

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(str.ToString(), connection);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
