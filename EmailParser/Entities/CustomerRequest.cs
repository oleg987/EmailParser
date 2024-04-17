namespace EmailParser.Entities;

public class CustomerRequest
{
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public bool Viber { get; set; }
    public bool Telegram { get; set; }
    public string? Email { get; set; }
    public string? AdditionalInfo { get; set; }
    public string? ChildName { get; set; }
    public int? Age { get; set; }
    public string? WhereFindUs { get; set; }
    public string? Course { get; set; }
    public string Source { get; set; } = null!;
}