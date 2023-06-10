namespace Model.Users;

public class ChangePasswordRequest
{
    string OldPassword { get; set; }
    string NewPassword { get; set; }
}