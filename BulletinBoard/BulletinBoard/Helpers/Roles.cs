namespace BulletinBoard.Helpers
{
    public static class Roles
    {
        public const string Administrator = "Administrator";
        public const string Moderator = "Moderator";
        public const string User = "User";

        public static string Normalize(string roleName)
        {
            return roleName.ToUpper();
        }
    }
}
