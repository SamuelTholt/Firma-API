namespace Firma_API.Dtos
{
    public record ErrorResponse(string Message, IEnumerable<string>? Errors = null);
}
