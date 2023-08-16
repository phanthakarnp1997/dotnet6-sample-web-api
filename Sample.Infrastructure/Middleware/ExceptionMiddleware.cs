using Sample.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Sample.Infrastructure.Entities.Response;

namespace Sample.Infrastructure.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                // Check for 401 status code in the response and modify it
                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    context.Response.ContentType = "application/json";


                    var errorResponse = new ResponseModel<object>()
                    {
                        Data = null,
                        Error = new Entities.Error.ErrorModel()
                        {
                            Code = "CODE",
                            Details = "Some Details",
                            Message = "Some Message"
                        },
                        Status = "Some Status"
                    };

                    await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
                }
                else if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    context.Response.ContentType = "application/json";

                    var errorResponse = new ResponseModel<object>()
                    {
                        Data = null,
                        Error = new Entities.Error.ErrorModel()
                        {
                            Code = "CODE",
                            Details = "Some Details",
                            Message = "Some Message"
                        },
                        Status = "Some Status"
                    };

                    await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
                }
            }
            catch (DataNotFoundException ex)
            {
                await HandleCustomExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleOtherExceptionsAsync(context, ex);
            }
        }

        private async Task HandleCustomExceptionAsync(HttpContext context, DataNotFoundException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            context.Response.ContentType = "application/json";

            var errorResponse = new ResponseModel<object>()
            {
                Data = null,
                Error = new Entities.Error.ErrorModel()
                {
                    Code = "CODE",
                    Details = "Some Details",
                    Message = "Some Message"
                },
                Status = "Some Status"
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }

        private async Task HandleOtherExceptionsAsync(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            var errorResponse = new ResponseModel<object>()
            {
                Data = null,
                Error = new Entities.Error.ErrorModel()
                {
                    Code = "CODE",
                    Details = "Some Details",
                    Message = "Some Message"
                },
                Status = "Some Status"
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}
