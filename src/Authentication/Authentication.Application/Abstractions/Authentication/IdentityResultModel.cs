namespace Authentication.Application.Abstractions.Authentication;

public sealed class IdentityResultModel
{
    public bool Succeeded { get; set; }
    public IReadOnlyCollection<string> Errors = Array.Empty<string>();
}