using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.InputSystem.InputAction;

namespace Ecosystem.Console
{
    public class ConsoleBehaviour : MonoBehaviour, ICommandSender
    {
        [Serializable]
        private class MessageEvent : UnityEvent<string> { }

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
            console.CommandExecutor = new CommandManager(commandCollection);
            inputField.onSubmit.AddListener(ProcessInput);
        }

        private void ProcessInput(string input)
        {
            console.SendInput(this, input);
            inputField.text = string.Empty;
            inputField.ActivateInputField();
        }

        public void Toggle(CallbackContext context)
        {
            if (!context.action.triggered) return;

            uiCanvas.SetActive(!uiCanvas.activeSelf);
            inputField.ActivateInputField();
        }

        void ICommandSender.SendMessage(string message)
        {
            messageEvent?.Invoke(message);
        }
    }
}
