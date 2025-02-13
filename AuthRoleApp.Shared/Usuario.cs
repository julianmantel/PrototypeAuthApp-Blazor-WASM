using System;
using System.Collections.Generic;

namespace AuthRoleApp.Shared;

public partial class Usuario
{
    public int Id { get; set; }

    public string? Correo { get; set; }

    public byte[]? PasswordHash { get; set; }

    public byte[]? PasswordSalt { get; set; }

    public string? Token { get; set; }

    public string? Rol { get; set; }

    public string? NombreUsuario { get; set; } 
}
