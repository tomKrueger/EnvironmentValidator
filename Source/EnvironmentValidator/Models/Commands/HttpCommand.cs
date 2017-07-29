using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentValidator.Models.Commands
{
    public class HttpCommand : Command
    {
        public HttpCommand()
            :base("Http")
        {
        }

        public override async Task<CommandResult> ExecuteAsync()
        {
            var result = new CommandResult(this);

            try
            {
                if (string.IsNullOrWhiteSpace(Url)) { throw new ArgumentException("Url not specified.", "Url"); }

                var request = new HttpRequestMessage();
                request.Method = HttpMethod;
                request.RequestUri = new Uri(Url);

                var c = new HttpClient();
                var response = await c.SendAsync(request);

                if (ExpectedResponseCode != null)
                {
                    var actualStatusCode = (int) response.StatusCode;
                    if (actualStatusCode != ExpectedResponseCode)
                    {
                        throw new Exception($"Unexpected Response Status Code.  Expected:{ExpectedResponseCode} Actual: {response.StatusCode}");
                    }
                }
                else
                {
                    response.EnsureSuccessStatusCode();
                }

                result.Status = ResultStatus.Success;
            }
            catch(Exception ex)
            {
                result.Status = ResultStatus.Error;
                result.Exception = ex;
            }

            return result;
        }

        public string Url { get; set; }

        public int? ExpectedResponseCode { get; set; }

        private HttpMethod _httpMethod = HttpMethod.Get;

        public HttpMethod HttpMethod
        {
            get { return _httpMethod; }
            set { _httpMethod = value; }
        }
    }
}
