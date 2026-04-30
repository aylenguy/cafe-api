// DTOs/TokenResponseDto.cs
public class TokenResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expira { get; set; }
    public string TipoToken { get; set; } = "Bearer";
}