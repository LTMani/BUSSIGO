using System;
using System.Collections.Generic;

namespace Bussigo.Game.Debug
{
    public class DeveloperDebugConsole
    {
        public bool IsConsoleOpen { get; set; } = false;
        public List<string> LogHistory { get; } = new List<string>();

        public event Action<string, string[]> OnCommandExecuted;

        public void ExecuteCommand(string inputLine)
        {
            if (string.IsNullOrWhiteSpace(inputLine)) return;
            LogHistory.Add($"> {inputLine}");

            string[] parts = inputLine.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string command = parts[0].ToLowerInvariant();
            string[] args = parts.Length > 1 ? parts[1..] : Array.Empty<string>();

            OnCommandExecuted?.Invoke(command, args);
        }
    }
}
