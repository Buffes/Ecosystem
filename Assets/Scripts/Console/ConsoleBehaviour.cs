using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.InputSystem.InputAction;

namespace Ecosystem.Console
{
    public class ConsoleBehaviour : MonoBehaviour, ICommandSender
    {
        [Serializable]
        private class MessageEvent : UnityEvent<string, MessageType> { }

        [SerializeField] private string commandPrefix = "/";
        [SerializeField] private CommandCollection commandCollection = default;

        [Header("UI")]
        [SerializeField] private GameObject uiCanvas = default;
        [SerializeField] private TMP_InputField inputField = default;
        [SerializeField] private MessageEvent messageEvent = default;

        private Console console;

        private void Awake()
        {
            console = new Console(commandPrefix);
        }

        private void Start()
        {
            CommandManager commandManager = new CommandManager(commandCollection);
            console.CommandExecutor = commandManager;
            console.TabCompleter = commandManager;
            inputField.onSubmit.AddListener(ProcessInput);
        }

        private void ProcessInput(string input)
        {
            console.SendInput(this, input);
            console.ResetHistoryNavigation();
            inputField.text = string.Empty;
            inputField.ActivateInputField();
        }

        public void TabComplete(CallbackContext context)
        {
            if (!context.action.triggered) return;

            List<string> results = console.GetAutocompleteAlternatives(inputField.text);
            if (results == null) return;
            if (results.Count == 0) return;

            string[] words = inputField.text.Split(' ');
            string newValue;

            if (words.Length == 1) newValue = console.CommandPrefix + results[0];
            else newValue = string.Join(" ", words.Take(words.Length - 1)) + " " + results[0];

            inputField.text = newValue;
            inputField.caretPosition = inputField.text.Length;
        }

        public void Toggle(CallbackContext context)
        {
            if (!context.action.triggered) return;

            uiCanvas.SetActive(!uiCanvas.activeSelf);
            inputField.ActivateInputField();

            console.ResetHistoryNavigation();
        }

        public void Close(CallbackContext context)
        {
            if (!context.action.triggered) return;

            uiCanvas.SetActive(false);
        }

        public void PreviousMessage(CallbackContext context)
        {
            if (!context.action.triggered) return;

            inputField.text = console.NavigatePreviousMessage();
            inputField.DeactivateInputField();
            inputField.ActivateInputField();
            inputField.caretPosition = inputField.text.Length;
        }

        public void NextMessage(CallbackContext context)
        {
            if (!context.action.triggered) return;

            inputField.text = console.NavigateNextMessage();
        }

        void ICommandSender.SendMessage(string message)
            => SendMessageEvent(message);

        void ICommandSender.SendMessage(string message, MessageType messageType)
            => SendMessageEvent(message, messageType);

        private void SendMessageEvent(string message, MessageType messageType = MessageType.Info)
            => messageEvent?.Invoke(message, messageType);
    }
}
