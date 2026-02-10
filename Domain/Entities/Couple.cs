namespace Domain.Entities;

public enum CoupleTypes
{
    Friends,
    Dating,
    Married,
    Temporary
}

public enum CoupleStatus
{
    Blocked,
    Active,
    Incomplete
}

public class Couple
{
    public Guid Id { get; private set; }
    public Guid CoupleOne { get; private set; }
    public Guid? CoupleTwo { get; private set; }
    public CoupleTypes Type { get; private set; }
    public CoupleStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Couple (Guid id, Guid coupleOne, Guid? coupleTwo, CoupleTypes type, CoupleStatus status, DateTime createdAt)
    {
        this.Id = id;
        this.CoupleOne = coupleOne;
        this.CoupleTwo = coupleTwo;
        this.Type = type;
        this.Status = status;
        this.CreatedAt = createdAt;
    }

    // Factory
    public static Couple CreateCouple(
        Guid coupleOne,
        CoupleTypes type,
        CoupleStatus status)
    {
        if (coupleOne == Guid.Empty)
            throw new ArgumentException("Name cannot be empty");

        return new Couple(id: Guid.NewGuid(), coupleOne, null, type, status, createdAt: DateTime.Now);
    }

    public static Couple Rehydrate(Guid id, Guid coupleOne, Guid? coupleTwo, CoupleTypes type, CoupleStatus status, DateTime createdAt)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty");

        return new Couple(id, coupleOne, coupleTwo, type, status, createdAt);
    }

    public void AddMember(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty");
        
        this.CoupleTwo = userId;
    }
}