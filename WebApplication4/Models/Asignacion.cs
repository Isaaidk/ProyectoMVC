using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Models
{
    public class Asignacion
    {
        [Key]
        public int IdAsignacion { get; set; }

        [MaxLength(200)]
        [Required]
        public string Nombre { get; set; }



    }
}
