using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Source.Auth.Midels.AccountViewModels
{
    public class UserInfoViewModel
    {
        [Display(Name = "Id")]
        public string Id { get; set; }
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Display(Name = "FullName")]
        public string FullName { get; set; }

        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [Display(Name = "DeptCode")]
        public int? DeptCode { get; set; }

        [Display(Name = "DeptName")]
        public string DeptName { get; set; }

        [Display(Name = "DeptAddress")]
        public string DeptAddress { get; set; }

        [Display(Name = "DeptPhone")]
        public string DeptPhone { get; set; }

        [Display(Name = "CertNo")]
        public string CertNo { get; set; }

        [JsonIgnore]
        [Display(Name = "RoleList")]
        public IEnumerable<string> RoleList { get; set; }

        [Display(Name = "Roles")]
        public string Roles
        {
            get { return RoleList == null ? null : string.Join(",", RoleList); }
        }
    }
}