using Sassa.BRM.Models;

namespace Sassa.BRM.Services
{
    public class ActivityService
    {
        IHttpClientFactory _httpClientFactory;
        string _activityApiUrl;
        public ActivityService(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _activityApiUrl = config["Urls:ActivityApi"]!;
        }

        public void PostActivity(DcActivity activity)
        {
            var client = _httpClientFactory.CreateClient("Brm");
            _ = client.PostAsJsonAsync(_activityApiUrl, activity);
        }
    }
}
