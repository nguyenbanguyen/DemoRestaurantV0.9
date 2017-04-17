
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

/// <summary>
/// Account cũ, delete sau khi lập account model/controller mới
/// </summary>
namespace DemoRestaurant.Models
{
    public class ExternalLoginConfirmationViewModel 
    {
        
        [Required(ErrorMessage = "Tên truy cập không được để trống")]
        [Display(Name = "User name")]
        [RegularExpression("^[0-9a-zA-Z-]*$", ErrorMessage = "Tên người dùng chỉ chứa kí tự 0-9, a-z và dấu gạch ngang")]
        public string ExternalUserName { get; set; }


        [Required(ErrorMessage = "Email không được để trống")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Email không hợp lệ,phải có dạng abc@xyz.com")]
        [Display(Name = "Email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(11, MinimumLength = 9, ErrorMessage = "Số điện thoại phải từ 9 đến 11 số")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Vui lòng nhập chính xác số điện thoại của bạn")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        public string ShippingAddress { get; set; }
        public string CustomerType { get; set; }



    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string ShippingAdress { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class 
        
        
        ViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
