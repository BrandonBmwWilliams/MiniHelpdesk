namespace MiniHelpdesk.Domain;

public sealed class Customer
{
    public Guid Id { get; }
    public string DisplayName { get; private set; }
    public string Email { get; private set; }

    public Customer(string displayName, string email)
    {
        Id = Guid.NewGuid();

        DisplayName = string.IsNullOrWhiteSpace(displayName)
            ? throw new ArgumentException("Display name is required.", nameof(displayName))
            : displayName.Trim();

        Email = string.IsNullOrWhiteSpace(email)
            ? throw new ArgumentException("Email is required.", nameof(email))
            : email.Trim();
    }

    public void UpdateContact(string displayName, string email)
    {
        DisplayName = string.IsNullOrWhiteSpace(displayName)
            ? throw new ArgumentException("Display name is required.", nameof(displayName))
            : displayName.Trim();

        Email = string.IsNullOrWhiteSpace(email)
            ? throw new ArgumentException("Email is required.", nameof(email))
            : email.Trim();
    }
}
