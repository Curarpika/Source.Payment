using System.ComponentModel.DataAnnotations;

namespace Source.Auth.Midels.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [EmailAddress]
        [Display(Name = "邮箱")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "名")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "姓")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "手机号")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0}至少{2}位", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "两次输入密码不一致.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "角色")]
        public string Roles { get; set; }

        [Required]
        [Display(Name = "部门")]
        public int? DeptCode { get; set; }

        [Required]
        [Display(Name = "证件号")]
        public string CertNo { get; set; }
    }
}
