using Core.Utilities.Messages;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Security;
using static Core.Utilities.Enums.Enums;

namespace Core.Middlewares
{
    public class ExceptionMiddleware
    {
        private RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext) 
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            httpContext.Response.ContentType = "application/json";
            var _statusCode = ResponseCodes.UnknownServerError;
            var _message = "Internal Server Error";
            var _httpstatusCode = (int)HttpStatusCode.InternalServerError;
            
            if (ex is SecurityException)//Token yok
            {
                if (ex.Message == "roleNotAssigned")//Role yetkisi yok
                {
                    _statusCode = ResponseCodes.TokenClaimError;
                    _httpstatusCode = (int)HttpStatusCode.Forbidden;
                    _message = AspectMessages.AuthorizationDenied;
                }
                else
                {
                    _statusCode = ResponseCodes.TokenNotFoundError;
                    _httpstatusCode = (int)HttpStatusCode.Unauthorized;
                    _message = AspectMessages.TokenNotFound;
                }
            }
            else if (ex is SecurityTokenExpiredException)//Token Expire oldu
            {
                _statusCode = ResponseCodes.TokenExpiredError;
                _httpstatusCode = (int)HttpStatusCode.Unauthorized;
                _message = AspectMessages.TokenExpired;
            }
            else if (ex is SecurityTokenSignatureKeyNotFoundException)//
            {
                _statusCode = ResponseCodes.InvalidTokenError;
                _httpstatusCode = (int)HttpStatusCode.Unauthorized;
                _message = AspectMessages.InvalidToken;
            }
            else if (ex is ValidationException)
            {
                _statusCode = ResponseCodes.ValidationError;
                _httpstatusCode = (int)HttpStatusCode.BadRequest;
                _message = ex.Message;
            }

            httpContext.Response.StatusCode = _httpstatusCode;
            return httpContext.Response.WriteAsync(new ErrorDetails
            {
                StatusCode=(int)_statusCode,
                Message=_message
            }.ToString());
        }
    }
}
