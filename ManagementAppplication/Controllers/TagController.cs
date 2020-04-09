using System;
using Management.Business.Business;
using Management.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace ManagementAppplication.Controllers
{
    public class TagController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Create Tag
        /// </summary>
        /// <param name="model">Tag Model</param>
        /// <returns></returns>
        public IActionResult Create(Tag model)
        {
            try
            {
                if (Save(model))
                {
                    ViewData["ValidTag"] = "Success";
                }
                else
                {
                    ViewData["ValidTag"] = "Error";
                }
                return View("Index");
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        /// <summary>
        /// Method to connect to database
        /// </summary>
        /// <param name="obj">Tag object</param>
        /// <returns>boolean result</returns>
        public bool Save(Tag obj)
        {
            try
            {
                return new TagsService().Insert(obj);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}