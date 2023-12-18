using FluentValidation;

namespace ShortenerUrl.ViewsModel;

public class ShortenerInputValidator : AbstractValidator<UrlDTO>
{

    public ShortenerInputValidator()
    {
        RuleFor(s => s.OriginalUrl).NotEmpty().WithMessage("Url cannot be empty").Must(BeAValidUrl).WithMessage("Invalid URL format");
    }
    
    private bool BeAValidUrl(string? url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return false;
        }

        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}