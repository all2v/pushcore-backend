namespace Astrum.CodeRev.Application.UserService.Services
{
    public static class GuidParser
    {
        public static Guid TryParse(string id)
        {
            if (string.IsNullOrEmpty(id))
                return Guid.Empty;
            var result = Guid.TryParse(id, out var guid);
            return result ? guid : Guid.Empty;
        }
    }
}