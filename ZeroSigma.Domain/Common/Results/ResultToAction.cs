using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroSigma.Domain.Common.Results
{
    public static class ResultToAction
    {
        public static ActionResult FromResult<T>(this ControllerBase controller, Result<T> result)
        {
            switch (result.ResultType)
            {
                case ResultType.Ok:
                    return controller.Ok(result.Data);
                case ResultType.NotFound:
                    return controller.NotFound(new
                    {
                        result.CustomProblemDetails.Title,
                        Status = 404,
                        result.CustomProblemDetails.Type,
                        result.CustomProblemDetails.Detail,
                        result.CustomProblemDetails.Code,
                        result.CustomProblemDetails.Errors
                    });
                case ResultType.Invalid:
                    return controller.BadRequest(new
                    {
                        result.CustomProblemDetails.Title,
                        Status = 400,
                        result.CustomProblemDetails.Type,
                        result.CustomProblemDetails.Detail,
                        result.CustomProblemDetails.Code,
                        result.CustomProblemDetails.Errors
                    });
                case ResultType.Unexpected:
                    return controller.BadRequest(new
                    {
                        result.CustomProblemDetails.Title,
                        Status = 400,
                        result.CustomProblemDetails.Type,
                        result.CustomProblemDetails.Detail,
                        result.CustomProblemDetails.Code,
                        result.CustomProblemDetails.Errors
                    });
                case ResultType.Unauthorized:
                    return controller.Unauthorized(new
                    {
                        result.CustomProblemDetails.Title,
                        Status = 401,
                        result.CustomProblemDetails.Type,
                        result.CustomProblemDetails.Detail,
                        result.CustomProblemDetails.Code,
                        result.CustomProblemDetails.Errors
                    });
                default:
                    throw new Exception("An unhandled exception has occured");
            }
        }
    }
}
