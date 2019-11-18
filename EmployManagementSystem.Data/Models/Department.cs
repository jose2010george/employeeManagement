using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmployManagementSystem.Data.Models
{
    public partial class Department : BaseEntity
    {
        // [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Department()
        {
            Employees = new HashSet<Employee>();
        }


        [Required]
        [StringLength(100)]
        [Display(Name="Department Name")]
        public string Name { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }

}
