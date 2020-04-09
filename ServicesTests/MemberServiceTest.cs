using Management.Business.Business;
using Management.Services.Services;
using Management.Services.StandardService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ServicesTests
{
    [TestClass]
    public class MemberServiceTest
    {
        /// <summary>
        /// Instance of Member Service
        /// </summary>
        MembersService serviceMember = new MembersService();

        /// <summary>
        /// Member Employee object
        /// </summary>
        /// <returns></returns>
        public Member MountMemberEmployee()
        {
            var member = new Member()
            {
                Name = "Rheniery",
                MemberType = EnumMemberType.Employee,
                Date_Register = DateTime.Now,
                Role = new Role()
                {
                    Id = 1
                },
                RoleId = 1,
                Date_StartTerm = null,
                Date_EndTerm = null,
                listTags = new List<Tag>()
                {
                    new Tag()
                    {
                        Id = 1
                    },
                    new Tag()
                    {
                        Id = 2
                    }
                }
            };

            return member;
        }

        /// <summary>
        /// Member Contractor Object
        /// </summary>
        /// <returns></returns>
        public Member MountMemberContractor()
        {
            var member = new Member()
            {
                Name = "Codelitt",
                MemberType = EnumMemberType.Contractor,
                Date_Register = DateTime.Now,
                Role = null,
                Date_StartTerm = DateTime.Today.AddMonths(1),
                Date_EndTerm = DateTime.Today.AddMonths(2),
                listTags = new List<Tag>()
                {
                    new Tag()
                    {
                        Id = 1
                    },
                    new Tag()
                    {
                        Id = 2
                    }
                }
            };

            return member;
        }

        /// <summary>
        /// Create Member Employee
        /// </summary>
        [TestMethod]
        public void CreateMemberEmployee()
        {
            var member = MountMemberEmployee();
            Assert.IsTrue(serviceMember.Save(member));

            var memberCreated = serviceMember.Consult("Rheniery");
            Assert.IsTrue(memberCreated.Count > 0);
        }

        /// <summary>
        /// Create Member Contractor
        /// </summary>
        [TestMethod]
        public void CreateMemberContractor()
        {
            var member = MountMemberContractor();
            Assert.IsTrue(serviceMember.Save(member));

            var memberCreated = serviceMember.Consult("Codelitt");
            Assert.IsTrue(memberCreated.Count > 0);
        }

        /// <summary>
        /// Consult All Members
        /// </summary>
        [TestMethod]
        public void GetAllMembers()
        {
            var list = serviceMember.Consult();

            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 0);
            Assert.IsTrue(list[0].Id > 0);
        }

        /// <summary>
        /// Edit a Member
        /// </summary>
        [TestMethod]
        public void EditMember()
        {
            var member = serviceMember.Consult(7);
            Assert.IsTrue(member.Id == 7);
            member.Name = "Rheniery2";
            var memberEdited = serviceMember.Save(member);
            Assert.IsTrue(memberEdited);
            member = serviceMember.Consult(7);
            Assert.IsTrue(member.Name == "Rheniery2");
        }

        /// <summary>
        /// Delete a member
        /// </summary>
        [TestMethod]
        public void DeleteMember()
        {
            var member = serviceMember.Consult();
            var memberDeleted = serviceMember.Delete(member[0]);
            Assert.IsTrue(memberDeleted);
            var memberConsult = serviceMember.Consult(member[0].Id);
            Assert.IsNull(memberConsult);
        }
    }
}
