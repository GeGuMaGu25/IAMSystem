namespace IAM.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    
    // Constructor sin parámetros requerido por Entity Framework Core
    protected User() { }

    public User(string email, string passwordHash)
    {
        if  (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("El email no puede estar vacio", nameof(email));
        
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("El hash de la contraseña es requerido",  nameof(passwordHash));
        
        Id = Guid.NewGuid();
        Email = email;
        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow;
    }
    
    // Comportamiento de dominio
    public bool VerifyPassword(string inputHash)
    {
        return PasswordHash == inputHash;
    }
}