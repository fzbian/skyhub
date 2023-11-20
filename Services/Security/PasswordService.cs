namespace skyhub.Services
{
    public class PasswordService
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool ValidatePassword(string password, string hash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hash);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during password validation: {ex.Message}");
                return false;
            }
        }
    }
}