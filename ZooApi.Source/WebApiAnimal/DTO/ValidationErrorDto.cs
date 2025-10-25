namespace WebApiAnimal.DTO
{
    public sealed class ValidationErrorDto
    {
        public object? AttemptedValue { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
