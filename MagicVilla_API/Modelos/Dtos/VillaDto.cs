using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.Modelos.Dtos
{
    public class VillaDto
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage ="Debe ingresar el {0}")]
        [MaxLength(30,ErrorMessage ="No se admiten más de 30 caracteres.")]
        public string? Nombre { get; set; }
        public string? Detalle { get; set; }
        [Required]
        public double Tarifa { get; set; }
        [Required]
        public int Ocupantes { get; set; }
        [Required]
        public int Metros2 { get; set; }
        [Required]
        public string? ImagenUrl { get; set; }
        public string? Amenidad { get; set; }
    }
}
