using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Management.Business.Business;
using Management.Services.Services;
using ManagementAppplication.Controllers.StandardController;
using Microsoft.AspNetCore.Mvc;

namespace ManagementAppplication.Controllers
{
    public class MemberController : StandardController<Member>
    {
        /// <summary>
        /// Load list of Roles to be used on pages, via ViewBags
        /// </summary>
        List<Role> rolesList = new RolesService().Consult();

        /// <summary>
        /// Load list of Tags to be used on pages, via ViewBags
        /// </summary>
        List<Tag> tagsList = new TagsService().Consult();

        /// <summary>
        /// Member Index
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Page to create a new member
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            ViewBag.RoleList = rolesList;
            ViewBag.TagList = tagsList;
            return View();
        }

        /// <summary>
        /// Action used to Create and Update a member
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateMember(Member model)
        {
            try
            {

                var listTags = Request.Form["listTags"];
                if (!string.IsNullOrEmpty(listTags))
                {
                    foreach (var item in listTags)
                    {
                        model.listTags.Add(new Tag()
                        {
                            Id = decimal.Parse(item)
                        });
                    }
                    model.listTags = model.listTags.Where(x => x.Id > 0).ToList();
                }
                if (Save(model))
                {
                    ViewData["ValidMember"] = "Success";
                }
                else
                {
                    ViewData["ValidMember"] = "Error";
                }
                ViewBag.RoleList = rolesList;
                ViewBag.TagList = tagsList;
                return View("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Load all members in a Page
        /// </summary>
        /// <returns>List of Members</returns>
        public IActionResult ListMembers()
        {
            try
            {
                var listMembers = Consult();
                return View(listMembers);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Action used to Delete a Member
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult DeleteMember(decimal id)
        {
            try
            {
                var member = Consult(id);
                var userDeleted = Delete(member);
                if (userDeleted)
                {
                    TempData["UserDeleted"] = "Success";
                }
                else
                {
                    TempData["UserDeleted"] = "Erro";
                }

                return RedirectToAction("ListMembers");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Load View to Filter a list by Parameters
        /// </summary>
        /// <param name="member">Member</param>
        /// <param name="tagId">Tag Id</param>
        /// <returns>A Partial showing a lista with filters</returns>
        [HttpGet]
        public ActionResult GetMembersByParam(Member member, [FromQuery(Name = "TagId")] string tagId)
        {
            try
            {
                ViewBag.TagList = tagsList;
                if (!string.IsNullOrEmpty(tagId))
                {
                    var listMembers = Consult();
                    listMembers = listMembers.Where(x => x.listTags.Any(y => y.Id.Equals(decimal.Parse(tagId)))).ToList();
                    return PartialView("_FindMembers", listMembers);
                }
                else if (!string.IsNullOrEmpty(member.Name))
                {
                    var listMembers = new MembersService().Consult(member.Name);
                    return PartialView("_FindMembers", listMembers);
                }
                else
                {
                    return PartialView("_FindMembers", null);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// Page used to Edit a member
        /// </summary>
        /// <param name="id">Member Id</param>
        /// <returns>Load View Edit with member data</returns>
        public IActionResult Edit(decimal id)
        {
            try
            {
                var member = Consult(id);
                ViewBag.RoleList = rolesList;
                ViewBag.TagList = tagsList;
                return View(member);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// Method use to connect with service to Save a member
        /// </summary>
        /// <param name="obj">Member object</param>
        /// <returns>Result of the Save or Update</returns>
        public override bool Save(Member obj)
        {
            try
            {
                return new MembersService().Save(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Connect with service to consult all Members
        /// </summary>
        /// <returns>All Members</returns>
        public override List<Member> Consult()
        {
            try
            {
                return new MembersService().Consult();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Use the service to bring a specific member
        /// </summary>
        /// <param name="id">Member Id</param>
        /// <returns>The object member</returns>
        public override Member Consult(decimal id)
        {
            try
            {
                return new MembersService().Consult(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Use the service to delete a member
        /// </summary>
        /// <param name="obj">Object Member</param>
        /// <returns>Result of the delete</returns>
        public override bool Delete(Member obj)
        {
            try
            {
                return new MembersService().Delete(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Normally use to connect with Consult By Name service
        /// </summary>
        /// <param name="name">Members Name</param>
        /// <returns>A list of members with that name</returns>
        public override List<Member> Consult(string name)
        {
            throw new NotImplementedException();
        }
    }
}