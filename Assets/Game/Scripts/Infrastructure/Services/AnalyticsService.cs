using System.Collections.Generic;
using YG;

namespace Game.Scripts.Infrastructure.Services
{
  public class AnalyticsService : IService
  {
    public void MetricaSend(string eventName)
    {
      YG2.MetricaSend(eventName);
    }
    
    public void MetricaSend(string eventName, Dictionary<string, object> data)
    {
      YG2.MetricaSend(eventName, data);
    }
    
    public void MetricaSend(string eventName, string param, Dictionary<string, object> data)
    {
      YG2.MetricaSend(eventName, param, data);
    }
  }
}