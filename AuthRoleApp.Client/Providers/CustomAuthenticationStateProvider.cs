using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace AuthRoleApp.Client
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;

        public CustomAuthenticationStateProvider(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //Dios quiera que esto funcione, porque la otra solucion era parsear el token y obtener los claims a mano
            try
            {
                string token = await _jsRuntime.InvokeAsync<String>("localStorage.getItem", "Token");
                var identity = new ClaimsIdentity();

                if (!string.IsNullOrEmpty(token) && ValidateToken(token))
                {
                    var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                    identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
                }

                var user = new ClaimsPrincipal(identity);

                var state = new AuthenticationState(user);
                NotifyAuthenticationStateChanged(Task.FromResult(state));
                return state;
            } catch (Exception ex) {
                Console.Error.WriteLine($"Error al obtener el estado de autentificacion: {ex.Message}");
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
           
        }

        //Chequeo que el token se pueda leer y que no haya expirado
        private bool ValidateToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(token)) 
                return false;

            var jwtToken = handler.ReadToken(token);
            if(jwtToken.ValidTo < DateTime.UtcNow) 
                return false;


            return true;
        }


    }
}
