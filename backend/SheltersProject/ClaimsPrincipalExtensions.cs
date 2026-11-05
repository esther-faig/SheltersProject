using System.Security.Claims;


namespace SheltersProject
{
    public static class ClaimsPrincipalExtensions
    {

        public static int GetUserId(this ClaimsPrincipal user)
        {
            var value = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(value, out var id))
                throw new InvalidOperationException("User id claim is missing.");
            return id;
        }

        // לחיפוש: אורחת בלי טוקן מקבלת null
        public static int? GetUserIdOrNull(this ClaimsPrincipal user)
        {
            return int.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out var id)
                ? id
                : null;
        }


    }
}
