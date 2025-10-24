namespace Domain.Entities;

public class Interactions
{
    public Guid Id { get; private set; }
    public Guid ActorId { get; private set; }
    public Guid TargetId { get; private set; }
    public string Type { get; set; } = String.Empty;

    private Interactions(Guid id, Guid actorId, Guid targetId, string type)
    {
        Id = id;
        ActorId = actorId;
        TargetId = targetId;
        Type = type;
    }
    
    public static Interactions CreateInteractions(Guid actorId, Guid targetId, string type)
    {
        // 🔒 Regras de negócio (invariantes)
        if (actorId == targetId)
            throw new ArgumentException("O ator não pode convidar a si mesmo.");

        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("O tipo de interação é obrigatório.");

        if (type != "invite" && type != "follow" && type != "like")
            throw new ArgumentException("Tipo de interação inválido.");
        return new Interactions(id: Guid.NewGuid(), actorId, targetId, type);
    }

    public static Interactions Rehydrate(Guid id, Guid actorId, Guid targetId, string type)
    {
        return new Interactions(id, actorId, targetId, type);
    }
    
    public void ChangeType(string newType)
    {
        if (string.IsNullOrWhiteSpace(newType))
            throw new ArgumentException("Tipo de interação inválido.");

        Type = newType;
    }
}