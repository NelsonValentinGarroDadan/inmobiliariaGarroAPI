using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliariaGarroAPI.Models;
public class LoginView
{
    public string Mail {get; set;}
    public string Password {get; set;}
}