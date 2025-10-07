using Assignment3.Models;
using System.Linq;
using System.Text.Json;

namespace Assignment3.Services
{
    public class RequestValidator
    {
        public Response ValidateRequest(Request request)
        {
            if (request == null)
                return new Response { Status = "request is null", Reason = "" };

            if (string.IsNullOrEmpty(request.Date))
                return new Response { Status = "missing date", Reason = "" };

            if (!long.TryParse(request.Date, out _))
                return new Response { Status = "illegal date", Reason = "" };

            if (string.IsNullOrEmpty(request.Method))
                return new Response { Status = "missing method", Reason = "" };

            var allowedMethods = new[] { "read", "create", "update", "delete", "echo" };
            if (!allowedMethods.Contains(request.Method))
                return new Response { Status = "illegal method", Reason = "" };

            // Echo method doesn't require a path
            if (request.Method != "echo" && string.IsNullOrEmpty(request.Path))
                return new Response { Status = "missing path", Reason = "" };

            if ((request.Method == "create" || request.Method == "update" || request.Method == "echo")
                && string.IsNullOrEmpty(request.Body))
            {
                return new Response { Status = "missing body", Reason = "" };
            }

            // Only validate JSON body for create and update methods (not echo)
            if ((request.Method == "create" || request.Method == "update") 
                && !string.IsNullOrEmpty(request.Body))
            {
                try
                {
                    System.Text.Json.JsonDocument.Parse(request.Body);
                }
                catch
                {
                    return new Response { Status = "illegal body", Reason = "", Body = null };
                }
            }

            return new Response { Status = "1 Ok", Reason = "", Body = null };
        }
    }
}