using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVilla_API.Modelos
{
    public class Villa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string? Nombre { get; set; }
        [Required]
        public string? Detalle { get; set; }
        [Required] 
        public double Tarifa { get; set; }
        public int  Ocupantes { get; set; }
        public int Metros2 {  get; set; }
        [Required]
        public string? ImagenUrl { get; set; }
        [Required]
        public string? Amenidad { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}
