using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliariaGarroAPI.Models;
public class Propietarios
{
    [Key]
    public int Id {get; set;}
    public int PersonaId {get; set;}

    [ForeignKey("PersonaId")]
    public Personas Persona {get; set;}
    public string Mail {get; set;}
    public string Password {get; set;}
}