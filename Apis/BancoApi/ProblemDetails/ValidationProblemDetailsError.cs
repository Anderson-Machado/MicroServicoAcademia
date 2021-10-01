using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BancoApi.ProblemDetails
{
    public class ValidationProblemDetailsError
    {
        public ValidationProblemDetailsError(string errorCode, List<string> description)
           => (ErrorCode, Description) = (errorCode, description);

        [JsonPropertyName("errorCode")]
        public string ErrorCode { get; set; }

        [JsonPropertyName("description")]
        public List<string> Description { get; set; }
    }
}