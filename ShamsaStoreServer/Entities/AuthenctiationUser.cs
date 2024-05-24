using Microsoft.AspNetCore.Identity;

namespace ShamsaStoreServer.Entities;

public class AuthenctiationUser : IdentityUser
{
    public string FullName { get; set; }
}
