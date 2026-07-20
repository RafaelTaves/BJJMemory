namespace BJJMemory.Communication.Usuarios.Requests;

public class RequestRegisterUsuario
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
