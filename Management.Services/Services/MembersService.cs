using Management.Business.Business;
using Management.Services.StandardService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Management.Services.Services
{
    public class MembersService : StandardService<Member>
    {
        /// <summary>
        /// Connection String to access the database
        /// </summary>
        string connectionString = new StandardConnection().GetConnectionString();

        /// <summary>
        /// Consult All Members
        /// </summary>
        /// <returns>List of All members</returns>
        public override List<Member> Consult()
        {
            var memberList = new List<Member>();
            StringBuilder str = new StringBuilder()
                    .AppendLine("SELECT M.*,R.Description FROM Member M")
                    .AppendLine("LEFT JOIN Roles R ON R.Id = M.Role_Id");

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
                        var member = FillMember(reader);
                        memberList.Add(member);
                    }

                    // Call Close when done reading.
                    reader.Close();
                }

                foreach (var member in memberList)
                {
                    member.listTags = new TagsService().FindTagsByMemberId(member.Id);
                }

                return memberList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Consult By Id
        /// </summary>
        /// <param name="id">Member Id</param>
        /// <returns>Object Member</returns>
        public override Member Consult(decimal id)
        {
            try
            {
                var member = new Member();
                string connectionString = new StandardConnection().GetConnectionString();
                StringBuilder str = new StringBuilder()
                        .AppendLine("SELECT M.*,R.Description FROM Member M")
                        .AppendLine("LEFT JOIN Roles R ON R.Id = M.Role_Id")
                        .AppendFormat("WHERE M.Id = {0}", id);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(str.ToString(), connection);
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    // Call Read before accessing data.
                    while (reader.Read())
                    {
                        member = FillMember(reader);
                    }

                    // Call Close when done reading.
                    reader.Close();
                }

                member.listTags = new TagsService().FindTagsByMemberId(id);

                return member;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Consult by name
        /// </summary>
        /// <param name="name">Member's Name</param>
        /// <returns>List of members that contains this name</returns>
        public override List<Member> Consult(string name)
        {
            var listMember = new List<Member>();
            StringBuilder str = new StringBuilder()
                    .AppendLine("SELECT M.*,R.Description FROM Member M")
                    .AppendLine("LEFT JOIN Roles R ON R.Id = M.Role_Id")
                    .AppendFormat("WHERE M.Name LIKE UPPER('%{0}%')", name.ToUpper());

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(str.ToString(), connection);
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var member = FillMember(reader);
                        listMember.Add(member);
                    }

                    reader.Close();
                }

                foreach(var member in listMember)
                {
                    member.listTags = new TagsService().FindTagsByMemberId(member.Id);
                }

                return listMember;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public override bool Delete(Member obj)
        {
            try
            {
                if (obj.listTags != null && obj.listTags.Count > 0)
                {
                    new TagsService().DeleteTagsByMemberId(obj.Id);
                }

                StringBuilder str = new StringBuilder()
                            .AppendFormat("DELETE FROM Member WHERE Id = {0}", obj.Id);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(str.ToString(), connection);
                    command.CommandType = CommandType.Text;
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

        public override bool Save(Member obj)
        {
            var res = false;
            if (obj.Id == default)
            {
                res = InsertMember(obj);
            }
            else
            {
                res = UpdateMember(obj);
            }
            return res;
        }

        public bool InsertMember(Member obj)
        {
            try
            {
                decimal memberId = 0;
                obj.Date_Register = DateTime.Now;
                StringBuilder str = new StringBuilder()
                        .AppendLine("INSERT INTO Member (")
                        .AppendLine("Name,")
                        .AppendLine("MemberType,")
                        .AppendLine("Date_Register,")
                        .AppendLine("Date_StartTerm,")
                        .AppendLine("Date_EndTerm,")
                        .AppendLine("Role_Id)")
                        .AppendLine(" VALUES ")
                        .AppendLine(" (@Name, ")
                        .AppendLine(" @MemberType, ")
                        .AppendLine(" @Date_Register, ")
                        .AppendLine(" @Date_StartTerm, ")
                        .AppendLine(" @Date_EndTerm, ")
                        .AppendLine(" @Role_Id) ")
                        .AppendLine(" SELECT SCOPE_IDENTITY() ");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(str.ToString(), connection);
                    command.CommandType = CommandType.Text;
                    command.Connection.Open();
                    command.Parameters.AddRange(getParametersMembers(obj).ToArray());
                    memberId = (decimal)command.ExecuteScalar();
                }

                if (obj.listTags != null && obj.listTags.Count > 0)
                {
                    foreach (var item in obj.listTags)
                    {
                        new TagsService().InsertMemberTags(item.Id, memberId);
                    }
                }

                return memberId > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool UpdateMember(Member obj)
        {
            StringBuilder str = new StringBuilder()
                    .AppendLine("UPDATE Member SET ")
                    .AppendLine("Name = @Name,")
                    .AppendLine("Date_StartTerm = @Date_StartTerm,")
                    .AppendLine("Date_EndTerm = @Date_EndTerm,")
                    .AppendLine("Role_Id = @Role_Id")
                    .AppendFormat(" WHERE Id = {0}", obj.Id);

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(str.ToString(), connection);
                    command.CommandType = CommandType.Text;
                    command.Connection.Open();
                    command.Parameters.AddRange(getParametersMembers(obj).ToArray());
                    command.ExecuteNonQuery();
                }

                //Verify listTags to Delete and Update
                if (obj.listTags != null && obj.listTags.Count > 0)
                {
                    new TagsService().DeleteTagsByMemberId(obj.Id);

                    foreach (var item in obj.listTags)
                    {
                        new TagsService().InsertMemberTags(item.Id, obj.Id);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Fill the object by reader
        /// </summary>
        /// <param name="reader">Reader with object</param>
        /// <returns>Object Member</returns>
        public Member FillMember(SqlDataReader reader)
        {
            var member = new Member();
            member.Id = decimal.Parse(reader.GetValue(reader.GetOrdinal("Id")).ToString());
            member.Name = reader.GetValue(reader.GetOrdinal("Name")).ToString();
            member.MemberType = (EnumMemberType)(int)reader.GetValue(reader.GetOrdinal("MemberType"));
            member.Date_Register = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("Date_Register")));
            member.Date_StartTerm = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Date_StartTerm")).ToString()) ? (DateTime?)null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("Date_StartTerm")));
            member.Date_EndTerm = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Date_EndTerm")).ToString()) ? (DateTime?)null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("Date_EndTerm")));
            if (member.MemberType == EnumMemberType.Employee)
            {
                member.Role = new Role()
                {
                    Id = decimal.Parse(reader.GetValue(reader.GetOrdinal("Role_Id")).ToString()),
                    Description = reader.GetValue(reader.GetOrdinal("Description")).ToString()
                };
                member.RoleId = decimal.Parse(reader.GetValue(reader.GetOrdinal("Role_Id")).ToString());
            }
            return member;
        }

        /// <summary>
        /// Mount List<SQLParameters> to Save or Update Member
        /// </summary>
        /// <param name="member">Member object</param>
        /// <returns>A List of Parameters</returns>
        public List<SqlParameter> getParametersMembers(Member member)
        {
            var list = new List<SqlParameter>();
            list.Add(new SqlParameter("Name", member.Name));
            list.Add(new SqlParameter("MemberType", member.MemberType));
            list.Add(new SqlParameter("Date_Register", string.Format("{0:yyyy-MM-dd}", member.Date_Register)));

            if (member.Date_StartTerm == null)
            {
                list.Add(new SqlParameter("Date_StartTerm", DBNull.Value));
            }
            else
            {
                list.Add(new SqlParameter("Date_StartTerm", member.Date_StartTerm));
            }

            if (member.Date_EndTerm == null)
            {
                list.Add(new SqlParameter("Date_EndTerm", DBNull.Value));
            }
            else
            {
                list.Add(new SqlParameter("Date_EndTerm", member.Date_EndTerm));
            }

            if (member.RoleId == default(decimal))
            {
                list.Add(new SqlParameter("Role_Id", DBNull.Value));
            }
            else
            {
                list.Add(new SqlParameter("Role_Id", member.RoleId));
            }

            return list;
        }

    }
}
