using Management.Business.StandardBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Management.Business.Business
{
    public class Tag : Standard
    {
        [Required(ErrorMessage = "Description is mandatory")]
        [Display(Name = "Description *")]
        public string Description { get; set; }
    }
}
