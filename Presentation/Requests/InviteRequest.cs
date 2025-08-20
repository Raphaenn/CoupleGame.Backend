namespace Api.Requests;

public class CreateRequest
{
    public string HostId { get; set; }
    public string QuizId { get; set; }
    public string Email { get; set; }
}

public class GetInviteRequest
{
    public string Email { get; set; }
}