namespace Access.Api.Responses
{
    public sealed record ApiError(string Code, string Description, string Type);
}
