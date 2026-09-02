namespace AM.DAL.Entities;

public class Endpoint
{
    public Guid Id { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public string Name { get; private set; }
    public string Url { get; private set; }
    public int CheckIntervalSeconds { get; private set; }
    public bool IsActive { get; private set; }
    
    protected Endpoint() {}

    public Endpoint(string name, string url, int checkIntervalSeconds, bool isActive)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTimeOffset.UtcNow;
        Name = name;
        Url = url;
        CheckIntervalSeconds = checkIntervalSeconds;
        IsActive = isActive;
    }
}