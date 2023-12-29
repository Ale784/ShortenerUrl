using FluentValidation;
using ShortenerUrl.Entities;
using ShortenerUrl.Extensions;
using ShortenerUrl.ViewsModel;

namespace ShortenerUrl
{
    
public static class ShortenerUrl
{

    public static RouteGroupBuilder ApiEndPoints(this RouteGroupBuilder groups)
    {

        groups.MapGet("/{id}", GetOriginalUrl);
        groups.MapPost("/", CreateShortUrl);

        return groups;
    }


    public static async Task<IResult> GetOriginalUrl(string id, ShortenerDb db)
    {   
        return await db.Urls.FindAsync(id) is UrlInput urlInput
        ? TypedResults.Redirect(urlInput.RedirectUrl)
        : TypedResults.NotFound();
    }


    public static async Task<IResult> CreateShortUrl(UrlDTO urlDTO, ShortenerDb db, IValidator<UrlDTO> ShortenerInputValidator, HttpContext httpContext){
        
        var validation = ShortenerInputValidator.Validate(urlDTO);

        if(!validation.IsValid)
        {

            return TypedResults.ValidationProblem(validation.ToDictionary());

        }

        var shortKey = ServiceExtension.GenerateRandomKey();
        var shortUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/shortener/{shortKey}";


        var urlShorter = new UrlInput()
        {
            Id = shortKey,
            RedirectUrl = urlDTO.OriginalUrl,
            ShortUrl = shortUrl,
            CreatedAt = DateTimeOffset.UtcNow,
            ExpiratesAt = DateTimeOffset.UtcNow.AddDays(1),
            LastVisited = DateTimeOffset.Now
        };

        db.Urls.Add(urlShorter);
        await db.SaveChangesAsync();

        return TypedResults.Created($"/shortener/{shortKey}",urlShorter);
    }
    
}
}