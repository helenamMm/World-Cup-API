using System.Security.Claims;
using WorldCupProjectApi.Models;
namespace WorldCupProjectApi.Services;

public class AuthService
{
    //Guarda los tokens en la memoria y se resetean cuando se caducan 
        private readonly Dictionary<string, (Usuario user, DateTime expiry)> _tokens = new();
        private readonly UsuarioService _usuarioService;

        public AuthService(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public async Task<string?> AuthenticateAsync(string email, string password)
        {
            //solamente para los usuarios administradores
            var usuario = await _usuarioService.ValidateCredentialsAsync(email, password);
            if (usuario == null || usuario.Rol != "admin")
                return null;
            
            var token = GenerateRandomToken();
            var expiry = DateTime.UtcNow.AddHours(24); 

            _tokens[token] = (usuario, expiry);
            return token;
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            // Clean expired tokens first
            CleanExpiredTokens();

            if (_tokens.TryGetValue(token, out var tokenData))
            {
                // Create claims principal from user data
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, tokenData.user.Id),
                    new Claim(ClaimTypes.Email, tokenData.user.Correo),
                    new Claim(ClaimTypes.Role, tokenData.user.Rol),
                    new Claim("Nombre", tokenData.user.Nombre),
                    new Claim("Apellido", tokenData.user.Apellido)
                };

                var identity = new ClaimsIdentity(claims, "AuthService");
                return new ClaimsPrincipal(identity);
            }

            return null;
        }

        public void Logout(string token)
        {
            _tokens.Remove(token);
        }

        private string GenerateRandomToken()
        {
            // Simple token generation using Guid
            return Guid.NewGuid().ToString() + "-" + DateTime.UtcNow.Ticks.ToString("x");
        }

        private void CleanExpiredTokens()
        {
            var now = DateTime.UtcNow;
            var expiredTokens = _tokens.Where(t => t.Value.expiry < now).ToList();
            
            foreach (var token in expiredTokens)
            {
                _tokens.Remove(token.Key);
            }
        }
}