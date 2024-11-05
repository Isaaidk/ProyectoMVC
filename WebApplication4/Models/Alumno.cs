using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Models
{

    public class Alumno
    {
        [Key]
        public String IdBanner { get; set; }

        [MaxLength(200)]
        [Required]
        public string nombre { get; set; }

        [EmailAddress]
        public string Correo { get; set; }

        [Required]
        [MaxLength(200)]
        public string Modalidad { get; set; }

        [Required]
        public Boolean Asistencia { get; set; }  


        public Asignacion? Asignacion { get; set; }

        [ForeignKey("Asignacion")]
        public int IdAsignacion { get; set; }


    }
}
