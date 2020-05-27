using Ecosystem.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ecosystem.UI
{
    public class AvailableAnimalItem : MonoBehaviour
    {
        public AnimalType AnimalType { get; set; }

        public Button Button;
        [SerializeField] private TMP_Text animalName;

        public void UpdateText()
        {
            animalName.text = AnimalType.Name;
        }
    }
}
