using TMPro;
using UnityEngine;

public class ChatOutputBehaviour : MonoBehaviour
{
    [SerializeField] private string messagePrefix = "> ";
    [SerializeField] private TMP_Text outputText = default;

    public void AddMessage(string message)
    {
        string prefix = string.Empty;
        if (!string.IsNullOrEmpty(outputText.text)) prefix = "\n";

        outputText.text += (prefix + messagePrefix + message);
    }
}
