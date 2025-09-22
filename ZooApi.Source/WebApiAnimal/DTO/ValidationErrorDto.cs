namespace WebApiAnimal.DTO
{
    public class ValidationErrorDto
    {
        public object? AttemptedValue { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
