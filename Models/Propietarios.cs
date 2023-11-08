using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliariaGarroAPI.Models;
public class Propietarios
{
    [Key]
    public int Id {get; set;}
    [ForeignKey("PersonaId")]
    public int PersonaId {get; set;}
    public Personas? Persona {get; set;}
    public string Mail {get; set;}
    public string Password {get; set;}
}