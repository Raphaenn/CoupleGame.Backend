namespace Domain.Entities;

public class Invite
{
    public Guid Id { get; private set; }
    public Guid QuizId { get; private set; }
    public Guid HostId { get; private set; }
    public string Email { get; private set; }
    public bool Accepted { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Invite(Guid id, Guid quizId, Guid hostId, string email, bool accepted)
    {
        Id = id;
        QuizId = quizId;
        HostId = hostId;
        Email = email;
        Accepted = accepted;
        CreatedAt = DateTime.Now;
    }

    public static Invite CreateInvite(Guid quizId, Guid hostId, string email)
    {
        return new Invite(id: Guid.NewGuid(), quizId, hostId, email, false);
    }

    public static Invite Rehydrate(Guid id, Guid quizId, Guid hostId, string email, bool accepted)
    {
        return new Invite(id, quizId, hostId, email, accepted);
    }

    public void Accept()
    {
        if (Accepted)
        {
            throw new ArgumentException("Invited already accepted");
        }

        this.Accepted = true;
        return;
    }
}