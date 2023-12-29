using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.ModelViews
{
    public class RegisterVM
    {
        [Key]
        public int CustomerId { get; set; }

        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Vui lòng nhập họ và tên")]
        public string AccountName { get; set; }
        
        [MaxLength(150)]
        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [DataType(DataType.EmailAddress)]
        [Remote(action: "ValidateEmail", controller: "Account")]
        public string AccountEmail { get; set; }

        [MaxLength(11)]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Display(Name = "Điện thoại")]
        [DataType(DataType.PhoneNumber)]
        [Remote(action: "ValidatePhone", controller: "Account")]
        public string Phone { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [MinLength(5, ErrorMessage = "Bạn cần đặt mật khẩu tối thiểu 5 ký tự")]
        public string AccountPassword { get; set; }

        [MinLength(5, ErrorMessage = "Bạn cần đặt mật khẩu tối thiểu 5 ký tự")]
        [Display(Name = "Nhập lại mật khẩu")]
        [Compare("AccountPassword", ErrorMessage = "Nhập lại mật khẩu không khớp")]
        public string ConfirmPassword { get; set; }
    }
}
