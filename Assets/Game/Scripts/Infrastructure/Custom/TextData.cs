using System;

namespace Game.Scripts.Infrastructure.Custom
{
    public class TextData
    {
        public readonly string Key;
        public readonly string Format;
        public readonly string[] Values;
        public Func<string, string> Formatter;

        public TextData(string key)
        {
            Key = key;
        }

        public TextData(string format, params string[] values)
        {
            Format = format;
            Values = values;
        }
        
        public static implicit operator TextData(string key)
        {
            return new TextData(key);
        }

        public void SetFormatter(Func<string, string> formatter)
        {
            Formatter = formatter;
        }
    }
}