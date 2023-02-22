namespace CzechAresClient.Models;

public class Record
{
    public string CompanyId { get; set; } = null!;
    public string? CompanyVatId { get; set; }
    public string CompanyName { get; set;} = null!;

    public string City { get; set; } = null!;
    public string PostCode { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string BuildingNumber { get; set; } = null!;
    public string Country { get; set; } = null!;

    public override string ToString()
    {
        return $"{CompanyId}: {CompanyName}";
    }
}