using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BancoApi.Service.ProblemDetail
{
    public class ValidationProblemDetailsError
    {
        public ValidationProblemDetailsError(string errorCode, string description)
            => (ErrorCode, Description) = (errorCode, description);

        [JsonPropertyName("errorCode")]
        public string ErrorCode { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
