
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UserDB
{
public class Cliente : Persona 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClienteID { get; set; }
        public string? Contraseña { get; set; }
        public bool Estado { get; set; }
    }
}