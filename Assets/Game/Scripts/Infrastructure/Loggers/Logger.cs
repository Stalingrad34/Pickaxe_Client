using UnityEngine;

namespace Core.Scripts.Loggers
{
  public static class Logger
  {
    public static void LogInfo(string log)
    {
      Debug.Log(log);
    }
    
    public static void LogWarn(string log)
    {
      Debug.LogWarning(log);
    }
    
    public static void LogError(string log)
    {
      Debug.LogError(log);
    }
  }
}