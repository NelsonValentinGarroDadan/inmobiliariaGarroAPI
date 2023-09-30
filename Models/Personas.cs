using System.ComponentModel.DataAnnotations;

namespace inmobiliariaGarroAPI.Models;

public class Personas
{
    [Key]
    public int Id {get; set;}
    public int DNI {get; set;}
    public string Nombre {get; set;}
    public string Apellido {get; set;}
    public int Telefono {get; set;}
}