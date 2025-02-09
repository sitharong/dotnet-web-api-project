using Microsoft.AspNetCore.Identity;

class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
}