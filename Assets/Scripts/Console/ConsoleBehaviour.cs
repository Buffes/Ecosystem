using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Ecosystem.Console
{
    public class ConsoleBehaviour : MonoBehaviour, ICommandSender
    {
        [Serializable]
        private class MessageEvent : UnityEvent<string> { }

        [SerializeField]
        private string commandPrefix = "/";
        [SerializeField]
        private CommandCollection commandCollection = default;
        [SerializeField]
        private TMP_InputField inputField = default;
        [SerializeField]
        private MessageEvent messageEvent = default;

        private Console console;

        private void Awake()
        {
            console = new Console(commandPrefix);
        }

        private void Start()
        {
            console.CommandExecutor = new CommandManager(commandCollection);
            inputField.onSubmit.AddListener((string input) => console.SendInput(this, input));
        }

        void ICommandSender.SendMessage(string message)
        {
            messageEvent?.Invoke(message);
        }
    }
}
