using System;

namespace Game.Scripts.Infrastructure.Services
{
  public class TimeProvider : IService
  {
    private static readonly DateTime UnixEpoch = new DateTime(1970,1,1,0,0,0,0,DateTimeKind.Utc);
    
    public long GetCurrentTimeStamp()
    {
      return (long)DateTime.Now.ToUniversalTime().Subtract(UnixEpoch).TotalSeconds;
    }
  }
}