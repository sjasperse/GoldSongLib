namespace GoldSongLib.Core.Models;

public class UserModel
{
    public UserModel(Guid id, string username, string givenName, string familyName, string fullName, string[]? tenants = null)
    {
        Id = id;
        Username = username;
        GivenName = givenName;
        FullName = fullName;
        FamilyName = familyName;
        Tenants = tenants ?? new string[] {};
    }

    public Guid Id { get; set; }
    public string Username { get; set; }
    public string GivenName { get; set; }
    public string FullName { get; set; }
    public string FamilyName { get; set; }
    public string[] Tenants { get; set; }
}
