using Ecosystem.Gameplay;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ecosystem.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private SceneField simulationScene = default;

        [SerializeField] private SimulationSettings simulationSettings = default;
        [SerializeField] private AnimalTypeSet animalTypeSet = default;

        [SerializeField] private AvailableAnimalItem availableAnimalItemTemplate = default;
        [SerializeField] private GameObject availableAnimalsContent = default;

        [SerializeField] private AddedAnimalItem addedAnimalItemTemplate = default;
        [SerializeField] private GameObject addedAnimalsContent = default;

        private List<AddedAnimalItem> addedAnimalItems = new List<AddedAnimalItem>();

        private void Start()
        {
            PopulateAvailableAnimalsList();
        }

        private void PopulateAvailableAnimalsList()
        {
            foreach (AnimalType animalType in animalTypeSet.values)
            {
                PopulateAvailableAnimalsList(animalType);
            }
        }

        private void PopulateAvailableAnimalsList(AnimalType animalType)
        {
            var availableAnimalitem = Instantiate(availableAnimalItemTemplate, availableAnimalsContent.transform);
            availableAnimalitem.AnimalType = animalType;
            availableAnimalitem.UpdateText();
            availableAnimalitem.Button.onClick.AddListener(() => AddAnimal(availableAnimalitem));
        }

        private void AddAnimal(AvailableAnimalItem availableAnimalItem)
        {
            Destroy(availableAnimalItem.gameObject);
            var item = Instantiate(addedAnimalItemTemplate, addedAnimalsContent.transform);
            item.AnimalType = availableAnimalItem.AnimalType;
            item.UpdateText();
            item.Button.onClick.AddListener(() => RemoveAnimal(item));

            addedAnimalItems.Add(item);
        }

        private void RemoveAnimal(AddedAnimalItem item)
        {
            Destroy(item.gameObject);
            PopulateAvailableAnimalsList(item.AnimalType);

            addedAnimalItems.Remove(item);
        }

        public void StartSimulation()
        {
            simulationSettings.Clear();
            foreach (AddedAnimalItem item in addedAnimalItems)
            {
                simulationSettings.AddAnimalPopulation(item.AnimalType.Baby, item.Amount);
            }

            SceneManager.LoadScene(simulationScene);
        }
    }
}
