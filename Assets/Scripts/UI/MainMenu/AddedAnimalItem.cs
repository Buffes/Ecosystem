using Ecosystem.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ecosystem.UI
{
    public class AddedAnimalItem : MonoBehaviour
    {
        public AnimalType AnimalType { get; set; }

        public int Amount
        {
            get
            {
                if (int.TryParse(amountInput.text, out int result)) return result;
                return 0;
            }
        }

        public Button Button;
        [SerializeField] private TMP_Text animalName;
        [SerializeField] private TMP_InputField amountInput;

        public void UpdateText()
        {
            animalName.text = AnimalType.Name;
        }
    }
}
