using System.Text;
using UniRx;

namespace Core.Scripts.Loggers
{
    public class LoggerModel
    {
        public readonly ReactiveProperty<string> ConsoleText = new ();
        public readonly ReactiveProperty<bool> IsShowed = new ();
        
        private const int LOGS_ARRAY_LENGTH = 200;
        private const string INFO_FORMAT = "<color=#FFFFFF>{0}</color>";
        private const string WARN_FORMAT = "<color=#FFC800>{0}</color>";
        private const string ERROR_FORMAT = "<color=#FF5851>{0}</color>";
        
        private readonly LimitStack<string> _limitStack;

        public LoggerModel()
        {
            _limitStack = new LimitStack<string>(LOGS_ARRAY_LENGTH);
        }

        public void InfoLog(string text)
        {
            var formatText = string.Format(INFO_FORMAT, text);
            AddLog(formatText);
        }
        
        public void WarningLog(string text)
        {
            var formatText = string.Format(WARN_FORMAT, text);
            AddLog(formatText);
        }
        
        public void ErrorLog(string text)
        {
            var formatText = string.Format(ERROR_FORMAT, text);
            AddLog(formatText);
        }

        public void ClearLogs()
        {
            _limitStack.Clear();
            ConsoleText.Value = string.Empty;
        }

        public void Close()
        {
            SetShowLogPanel(false);
        }

        public void SetShowLogPanel(bool isShow)
        {
            IsShowed.Value = isShow;
            ConsoleWrite();
        }

        private void AddLog(string text)
        {
            _limitStack.Add(text);
            ConsoleWrite();
        }

        private void ConsoleWrite()
        {
            if (!IsShowed.Value)
                return;
        
            var console = new StringBuilder();
            
            foreach (var log in _limitStack)
            {
                if (!string.IsNullOrEmpty(log))
                    console.AppendLine(log);
            }
            
            ConsoleText.Value = console.ToString();
        }
    }
}