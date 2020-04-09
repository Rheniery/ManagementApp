using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Management.Business.StandardBusiness;
using System.ComponentModel.DataAnnotations.Schema;

namespace Management.Business.Business
{
    public enum EnumMemberType
    {
        Employee = 1,
        Contractor = 2 
    }

    public class Member : Standard
    {
        [Required(ErrorMessage = "Please sign in with the Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please select one Type of Member")]
        [EnumDataType(typeof(EnumMemberType))]
        [Display(Name = "Type of Member")]
        public EnumMemberType MemberType { get; set; }

        [Display(Name = "Register Date")]
        [DataType(DataType.Date, ErrorMessage = "Please enter with a valid Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date_Register { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date,ErrorMessage = "Please enter with a valid Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? Date_StartTerm { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date, ErrorMessage = "Please enter with a valid Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? Date_EndTerm { get; set; }

        public Role Role { get; set; }

        public decimal RoleId { get; set; }

        public List<Tag> listTags { get; set; }

       
    }
}
