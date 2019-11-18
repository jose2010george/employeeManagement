using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace EmployManagementSystem.Data.Models
{
    public partial class Employee : BaseEntity
    {
        public Employee()
        {
        }

        [Required]
        [StringLength(50,MinimumLength =2)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(150)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string EmailAdress { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Phone")]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Please enter valid phone no.")]
        public string PhoneNumber { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(100)]
        public string State { get; set; }

        [StringLength(20)]
        [Display(Name = "Zipcode")]
        [RegularExpression(@"(^\d{5}(-\d{4})?$)|(^[ABCEGHJKLMNPRSTVXYabceghjklmnprstvxy]{1}\d{1}[ABCEGHJKLMNPRSTVWXYZabceghjklmnprstv‌​xy]{1} *\d{1}[ABCEGHJKLMNPRSTVWXYZabceghjklmnprstvxy]{1}\d{1}$)", ErrorMessage = "The zipcode is not a valid US or Canadian postal code.")]
        public string ZipCode { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [StringLength(250, MinimumLength = 6)]
        public string Password { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Joining Date")]
        public DateTime JoiningDate { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Exit Date")]
        [DataType(DataType.Date)]
        public DateTime? ExitDate { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }

        [Display(Name = "Department")]
        public long Department_id { get; set; }

        public virtual Department Department { get; set; }
    }

}
