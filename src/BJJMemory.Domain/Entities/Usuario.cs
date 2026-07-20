namespace BJJMemory.Domain.Entities;

public class Usuario
{
    public Guid Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string HashedPassword { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    private Usuario(){}

    private Usuario(Guid id, string username, string email, string hashedPassword, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        Username = username;
        Email = email;
        HashedPassword = hashedPassword;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Usuario Create(string username, string email, string hashedPassword)
    {
        var today = DateTime.UtcNow;

        return new Usuario(Guid.NewGuid(), username, email, hashedPassword, today, today);
    }

    public void Update(string username, string email, string hashedPassword)
    {
        Username = username;
        Email = email;
        HashedPassword = hashedPassword;
        UpdatedAt = DateTime.UtcNow;
    }
}
