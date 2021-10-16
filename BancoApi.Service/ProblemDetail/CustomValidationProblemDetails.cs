using System.Collections.Generic;

namespace BancoApi.Service.ProblemDetail
{
    public class CustomValidationProblemDetails
    {
        public string Type { get; set; }

        public string Title { get; set; }

        public int Status { get; set; }

        public string TraceId { get; set; }

        public IDictionary<string, List<ValidationProblemDetailsError>> Errors { get; set; }
    }
}
