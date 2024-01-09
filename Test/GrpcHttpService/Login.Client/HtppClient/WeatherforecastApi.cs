using RestSharpComponent;

namespace Login.Client.HtppClient
{

    public class WeatherforecastApi
    {
        public interface IWeatherForecastApiSync : IApiAccessor
        {
            ApiResponse<List<WeatherForecast>> ListWeatherForecastSync();
        }
        public interface IWeatherForecastApiAsync : IApiAccessor
        {
            Task<ApiResponse<List<WeatherForecast>>> ListWeatherForecastAsync();
        }
        public interface IWeatherForecastApi : IWeatherForecastApiSync, IWeatherForecastApiAsync
        {
        }

        public partial class WeatherForecastApi : IWeatherForecastApi
        {
            private ExceptionFactory _exceptionFactory = (methodName, response) => null;
            public WeatherForecastApi(string basePath)
            {
                this.Configuration = RestSharpComponent.Configuration.MergeConfigurations(GlobalConfiguration.Instance, new Configuration() { BasePath = basePath });
                this.SynchronousClient = new ApiClient(this.Configuration.BasePath);
                this.AsynchronousClient = new ApiClient(this.Configuration.BasePath);
                this.ExceptionFactory = RestSharpComponent.Configuration.DefaultExceptionFactory;
            }
            public ExceptionFactory ExceptionFactory { get { if (_exceptionFactory != null && _exceptionFactory.GetInvocationList().Length > 1) throw new InvalidOperationException("unsupported. "); return _exceptionFactory; } set { _exceptionFactory = value; } }
            public IAsynchronousClient AsynchronousClient { get; set; }
            public ISynchronousClient SynchronousClient { get; set; }
            public string GetBasePath() => this.Configuration.BasePath;
            public IReadableConfiguration Configuration { get; set; }

            public async Task<ApiResponse<List<WeatherForecast>>> ListWeatherForecastAsync()
            {
                RequestOptions localVarRequestOptions = new RequestOptions();
                string[] _contentTypes = new string[] { };
                string[] _accepts = new string[]
                {
                "application/json"
                };

                foreach (var _contentType in _contentTypes)
                    localVarRequestOptions.HeaderParameters.Add("Content-Type", _contentType);

                foreach (var _accept in _accepts)
                    localVarRequestOptions.HeaderParameters.Add("Accept", _accept);

                //localVarRequestOptions.PathParameters.Add("settle", ClientUtils.ParameterToString(settle)); // path parameter


                // make the HTTP request

                var localVarResponse = await this.AsynchronousClient.GetAsync<List<WeatherForecast>>("/WeatherForecast", localVarRequestOptions, this.Configuration);

                if (this.ExceptionFactory != null)
                {
                    Exception _exception = this.ExceptionFactory("ListWeatherForecast", localVarResponse);
                    if (_exception != null) throw _exception;
                }

                return localVarResponse;
            }

            public ApiResponse<List<WeatherForecast>> ListWeatherForecastSync()
            {
                return ListWeatherForecastAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }
    }
}
