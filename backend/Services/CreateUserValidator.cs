using jornal.Models;

namespace jornal.Services
{
    public class CreateUserValidator
    {
        public async Task<IResult> IsValid(UserCreateDto user)
        {
            if (user == null) return Results.BadRequest("Invalid user");
            if (user.Name == null) return Results.BadRequest("Invalid user name");
            if (user.Email == null) return Results.BadRequest("Invalid user email");
            if (user.Password == null) return Results.BadRequest("invalid user Password");

            return null;
        }
    }
}
