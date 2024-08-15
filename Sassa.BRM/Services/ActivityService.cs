using System.Diagnostics;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Sassa.Brm.Common.Models;
using Sassa.BRM.Models;

namespace Sassa.BRM.Services;

public class ActivityService(IHttpClientFactory _httpClientFactory, IConfiguration config)
{
    string _activityApiUrl = config["Urls:ActivityApi"];
    public void PostActivity(DcActivity activity)
    {
        var client = _httpClientFactory.CreateClient("Brm");
        _ = client.PostAsJsonAsync(_activityApiUrl,activity);
    }
    #region Activity

    public void CreateActivity(string Area, string Activity, string regionId, decimal officeId, string samName, string UniqueFileNo = "")
    {
        DcActivity activity = new DcActivity { ActivityDate = DateTime.Now, RegionId = regionId, OfficeId = officeId, Userid = 0, Username = samName, Area = Area, Activity = Activity, Result = "OK", UnqFileNo = UniqueFileNo };
        PostActivity(activity);
    }

    #endregion

}
