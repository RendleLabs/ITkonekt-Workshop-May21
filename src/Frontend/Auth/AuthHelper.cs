using System.Net.Http;
using System.Threading.Tasks;

namespace Frontend.Auth
{
    public class AuthHelper
    {
        private readonly HttpClient _client;

        private string _token;

        public AuthHelper(HttpClient client)
        {
            _client = client;
        }

        public ValueTask<string> GetTokenAsync()
        {
            if (_token is not null) return new ValueTask<string>(_token);
            return new ValueTask<string>(GetTokenAsyncImpl());
        }

        private async Task<string> GetTokenAsyncImpl()
        {
            _token = await _client.GetStringAsync("/generateJwtToken?name=frontend");
            return _token;
        }
    }
}