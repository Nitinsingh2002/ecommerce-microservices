
namespace Common.Responses;


public sealed record ApiError
(
    string code,
    string message
);