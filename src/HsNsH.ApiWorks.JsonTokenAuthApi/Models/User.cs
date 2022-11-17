namespace HsNsH.ApiWorks.JsonTokenAuthApi.Models;

public class User
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string FamilyName { get; set; }
}