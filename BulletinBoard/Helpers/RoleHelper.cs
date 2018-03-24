namespace BulletinBoard.Helpers
{
    /// <summary>
    /// Contains role names.
    /// </summary>
    public static class RoleHelper
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
