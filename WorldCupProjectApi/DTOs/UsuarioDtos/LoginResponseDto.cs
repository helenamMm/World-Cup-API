namespace WorldCupProjectApi.DTOs;

public class LoginResponseDto
{
    public UsuarioDto Usuario { get; set; }
    public string? Token { get; set; } 
    public string Message { get; set; }
    public string Rol { get; set; }
}