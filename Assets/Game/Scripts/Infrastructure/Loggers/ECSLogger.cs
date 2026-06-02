using System.Collections.Generic;
using System.Text;
using Leopotam.EcsProto;
using UnityEngine;

namespace Core.Scripts.Loggers
{
    public static class ECSLogger
    {
        private const int MAX_LOGS_COUNT = 40;
        private static Queue<List<string>> _logData = new Queue<List<string>>();

        public static void EnqueueLogData(IProtoSystem system, params EcsLogData[] logData)
        {
            var logs = new List<string>();
            foreach (var data in logData)
            {
                logs.Add($"{system.GetType().Name} | {data.Name} : {data.Value}");
            }

            _logData.Enqueue(logs);
            if (_logData.Count > MAX_LOGS_COUNT)
                _logData.Dequeue();
        }

        public static void ShowLogs()
        {
            foreach (var logs in _logData)
            {
                foreach (var log in logs)
                {
                    Debug.Log(log);
                }
            }
        }

        public static string GetLogs()
        {
            var result = new StringBuilder();

            foreach (var logs in _logData)
            {
                foreach (var log in logs)
                {
                    result.AppendLine(log);
                }
            }

            return result.ToString();
        }

        public static void Clear()
        {
            _logData.Clear();
        }
    }
    
    public struct EcsLogData
    {
        public readonly string Name;
        public readonly object Value;

        public EcsLogData(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}