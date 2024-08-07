namespace Domain.Entities;

public class Couple
{
    public string Id { get; set; }
    public string CoupleOne { get; set; }
    public string CoupleTwo { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }

    public Couple (string id, string coupleOne, string coupleTwo, string type, string status, DateTime createdAt)
    {
        if (string.IsNullOrEmpty(coupleOne))
            throw new ArgumentException("Name cannot be empty");
        if (string.IsNullOrEmpty(coupleTwo))
            throw new ArgumentException("Email cannot be empty");

        this.Id = id;
        this.CoupleOne = coupleOne;
        this.CoupleTwo = coupleTwo;
        this.Type = type;
        this.Status = status;
        this.CreatedAt = createdAt;
    }
}