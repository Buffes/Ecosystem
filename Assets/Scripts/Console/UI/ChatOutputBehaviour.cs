using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ecosystem.Console
{
    public class ChatOutputBehaviour : MonoBehaviour
    {
        [SerializeField] private string messagePrefix = "> ";
        [SerializeField] private TMP_Text outputText = default;
        [SerializeField] private ScrollRect scrollRect = default;

        public void AddMessage(string message, MessageType messageType)
        {
            string prefix = string.Empty;
            if (!string.IsNullOrEmpty(outputText.text)) prefix += "\n";
            if (messageType == MessageType.Chat) prefix += messagePrefix;

            AppendText(prefix + message);
            ScrollToBottom();
        }

        private void AppendText(string text)
        {
            outputText.text += text;
        }

        private void ScrollToBottom()
        {
            Canvas.ForceUpdateCanvases();
            scrollRect.normalizedPosition = new Vector2(0, 0);
        }
    }
}
