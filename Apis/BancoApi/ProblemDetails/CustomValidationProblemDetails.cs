using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BancoApi.ProblemDetails
{
    public class CustomValidationProblemDetails
    {
        public string Type { get; set; }

        public string Title { get; set; }

        public int Status { get; set; }

        public string TraceId { get; set; }

        public IDictionary<string, ValidationProblemDetailsError> Errors { get; set; }
    }
}
