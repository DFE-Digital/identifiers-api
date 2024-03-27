using Dfe.Identifiers.Api.Models;
using FluentAssertions;
using Dfe.Identifiers.Api.Models.Identifiers;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Identifiers.Api.Test.Extensions;

public static class ActionResultExtension 
{
    public static Models.Identifiers.Identifiers? GetIdentifiers(this ActionResult<Models.Identifiers.Identifiers> actionResult)
    {
        return (Models.Identifiers.Identifiers?)((OkObjectResult?)actionResult.Result)?.Value;
    }
    
    public static int? GetStatusCode<T>(this ActionResult<T> actionResult)
    {
        return ((ObjectResult?)actionResult.Result)?.StatusCode;
    }
}