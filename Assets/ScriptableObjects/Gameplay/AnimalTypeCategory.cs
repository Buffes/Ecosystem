using System.Collections.Generic;
using UnityEngine;

namespace Ecosystem.Gameplay
{
    [CreateAssetMenu(menuName = "Gameplay/AnimalCategory")]
    public class AnimalTypeCategory : AnimalTypeComponent
    {
        public List<AnimalTypeComponent> Children = new List<AnimalTypeComponent>();

        public override void AddLeaves(List<AnimalType> list)
        {
            foreach (AnimalTypeComponent component in Children)
            {
                component.AddLeaves(list);
            }
        }
    }
}
