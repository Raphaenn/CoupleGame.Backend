namespace Domain.Entities;

public class Couple
{
    public Guid Id { get; set; }
    public Guid CoupleOne { get; set; }
    public Guid CoupleTwo { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }

    public Couple (Guid id, Guid coupleOne, Guid coupleTwo, string type, string status, DateTime createdAt)
    {
        if (coupleOne == Guid.Empty)
            throw new ArgumentException("Name cannot be empty");
        if (coupleTwo == Guid.Empty)
            throw new ArgumentException("Email cannot be empty");

        this.Id = id;
        this.CoupleOne = coupleOne;
        this.CoupleTwo = coupleTwo;
        this.Type = type;
        this.Status = status;
        this.CreatedAt = createdAt;
    }
}