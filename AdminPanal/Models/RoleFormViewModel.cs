using System.ComponentModel.DataAnnotations;

namespace AdminPanal.Models
{
    public class RoleFormViewModel
    {
        [Required(ErrorMessage ="Name Is Required")]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
