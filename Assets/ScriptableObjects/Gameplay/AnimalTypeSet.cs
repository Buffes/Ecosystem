using System.Collections.Generic;
using UnityEngine;

namespace Ecosystem.Gameplay
{
    [CreateAssetMenu(menuName = "Gameplay/AnimalTypeSet")]
    public class AnimalTypeSet : ScriptableObject
    {
        public List<AnimalType> values = new List<AnimalType>();
    }
}
