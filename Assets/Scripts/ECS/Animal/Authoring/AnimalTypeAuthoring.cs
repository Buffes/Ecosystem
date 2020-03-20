﻿using Ecosystem.ECS.Death;
using Ecosystem.Gameplay;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Ecosystem.ECS.Animal
{
    public class AnimalTypeAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
    {
        [SerializeField]
        private AnimalType animalType = default;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new AnimalTypeData
            {
                AnimalTypeId = animalType.GetInstanceID()
            });

            dstManager.AddComponentData(entity, new DropOnDeath
            {
                Prefab = conversionSystem.GetPrimaryEntity(animalType.Meat)
            });

            DynamicBuffer<PreyTypesElement> preyBuffer = dstManager.AddBuffer<PreyTypesElement>(entity);
            foreach (AnimalType animalType in animalType.Prey)
            {
                if (animalType.GetInstanceID() == this.animalType.GetInstanceID()) continue;

                preyBuffer.Add(new PreyTypesElement { AnimalTypeId = animalType.GetInstanceID() });
            }

            DynamicBuffer<FoodTypesElement> foodBuffer = dstManager.AddBuffer<FoodTypesElement>(entity);
            foreach (FoodType foodType in animalType.Food)
            {
                if (foodType.GetInstanceID() == this.animalType.Meat.GetInstanceID()) continue;

                foodBuffer.Add(new FoodTypesElement { FoodTypeId = foodType.GetInstanceID() });
            }
        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.Add(animalType.Meat);
        }
    }
}
