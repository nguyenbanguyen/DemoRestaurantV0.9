using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace DemoRestaurant.Models
{
    public class AccountModel
    {
       
    }
    //class used for check logon
    public class LogOnModel
    {
        [Required(ErrorMessage = "Tên truy cập không được để trống")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
    //class used to check register
    public class RegisterModel
    {
        [Required(ErrorMessage = "Tên truy cập không được để trống")]
        [Display(Name = "User name")]
        [Remote("doesUserNameExist", "Account", HttpMethod = "POST", ErrorMessage = "Người dùng đã tồn tại. Vui lòng nhập tên người dùng khác.")]
        [RegularExpression("^[0-9a-zA-Z-]*$", ErrorMessage = "Tên người dùng chỉ chứa kí tự 0-9, a-z và dấu gạch ngang")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Email không hợp lệ,phải có dạng abc@xyz.com")]
        [Display(Name = "Email address")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(100, ErrorMessage = "Mật khẩu tối thiểu phải 6 ký tự", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Mật khẩu nhập lại và mật khẩu không trùng nhau.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(11, MinimumLength = 9, ErrorMessage = "Số điện thoại phải từ 9 đến 11 số")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Vui lòng nhập chính xác số điện thoại của bạn")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        public string ShippingAddress { get; set; }
        public string CustomerType { get; set; }
        [DataType(DataType.DateTime)]
        [Required]
        public DateTime Created_at { get; set; }
    }
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Mật khẩu hiện tại không được để trống")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu hiện tại")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Mật khẩu mới không được để trống")]
        [StringLength(100, ErrorMessage = "Mật khẩu tối thiểu là 6 kí tự", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "Mật khẩu mới và mật khẩu nhập lại không đúng")]
        public string ConfirmPassword { get; set; }
    }
}