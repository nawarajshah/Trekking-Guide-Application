using Microsoft.AspNetCore.Identity;

namespace TrekkingGuideApp
{
    public class CustomIdientityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
        {
            // return an error with an empty description so it won't be displayed.
            return new IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = string.Empty
            };
        }
    }
}
