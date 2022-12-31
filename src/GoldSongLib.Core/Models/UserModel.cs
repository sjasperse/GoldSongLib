namespace GoldSongLib.Core.Models;

public class UserModel
{
    public UserModel(Guid id, string username, string givenName, string familyName, string tenantId)
    {
        Id = id;
        Username = username;
        GivenName = givenName;
        FamilyName = familyName;
        TenantId = tenantId;
    }

    public Guid Id { get; set; }
    public string Username { get; set; }
    public string GivenName { get; set; }
    public string FamilyName { get; set; }
    public string TenantId { get; set; }
}
