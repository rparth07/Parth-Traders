﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Parth_Traders.Service.Filter
{
    public class UserFriendlyExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var BadRequestException = context.Exception as BadRequestException;
            if (BadRequestException != null)
            {
                context.Result = new BadRequestObjectResult(BadRequestException.Message);
                base.OnException(context);
                return;
            }

            var notFoundException = context.Exception as NotFoundException;
            if (notFoundException != null)
            {
                context.Result = new NotFoundObjectResult(notFoundException.Message);
                base.OnException(context);
                return;
            }

            context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            base.OnException(context);
        }
    }
}
