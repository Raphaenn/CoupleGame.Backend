namespace Api.Requests;

public record struct CreateTemCoupleReq(string UserId1);

public record struct SearchTemCoupleReq(string UserId1, string UserId2);