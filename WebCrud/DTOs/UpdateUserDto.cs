namespace WebCrud.DTOs
{
    public class UpdateUserPassDto
    {
        public required string UserName { get; set; }
        public required string CurrPassword { get; set; }
        public required string NewPassword { get; set; }
    }
    public class UpdateUserEmailDto
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
