using AuthRoleApp.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthRoleApp.Data;
using AuthRoleApp.Services;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;


namespace AuthRoleApp.Controllers
{
    [Authorize] //proteccion de los endpoints, los que tienen [AllowAnonymous] funcionan para cualquier que acceda
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly AutentificacionContext _context;
        private AuthService _authService;
        public UsuarioController(AutentificacionContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpGet("GetDatos")]
        public async Task<ActionResult<List<Usuario>>> GetCuenta()
        {
            var lista = await _context.Usuarios.ToListAsync();
            return Ok(lista);
        }

        [HttpPost("Registrar")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> CreateCuenta(UsuarioDTO obj)
        {
            try
            {
                var usuario = new Usuario();

                _authService.CreatePasswordHash(obj.password, out byte[] passwordHash, out byte[] passwordSalt);
                usuario.NombreUsuario = obj.nombre_usuario;
                usuario.Correo = obj.correo;
                usuario.PasswordHash = passwordHash;
                usuario.PasswordSalt = passwordSalt;

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                return Ok("Registrado con exito");
            }
            catch (Exception ex)
            {
                return BadRequest("Error durante el registro" + " " + ex.Message);
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> IniciarSesion(UsuarioDTO obj)
        {
            
            var cuenta = await _context.Usuarios.Where(x => x.Correo == obj.correo).FirstOrDefaultAsync();
            bool valido = _authService.VerifyPasswordHash(obj.password, cuenta.PasswordHash, cuenta.PasswordSalt);

            if (cuenta == null || !valido)
            {
                return BadRequest("Usuario y/o contraseña incorrectos");
            }

            string token = _authService.CreateToken(cuenta);
            return Ok(token);
        }

    }
}
