using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Identity;

namespace CodeLab.Models;

public class ApplicationUser : MongoIdentityUser<Guid>
{
    
}