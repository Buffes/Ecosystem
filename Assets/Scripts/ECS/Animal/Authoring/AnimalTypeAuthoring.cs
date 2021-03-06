﻿using Ecosystem.ECS.Animal.Needs;
using Ecosystem.ECS.Death;
using Ecosystem.ECS.Movement;
using Ecosystem.ECS.Stats.Base;
using Ecosystem.ECS.Targeting.Sensing;
using Ecosystem.Gameplay;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Ecosystem.ECS.Animal
{
    public class AnimalTypeAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
    {
        public AnimalType animalType = default;
        [SerializeField]
        [Range(0f, 1f)]
        private float startingHunger = 0.75f;
        [SerializeField]
        [Range(0f, 1f)]
        private float startingThirst = 0.75f;
        [SerializeField]
        [Range(0f, 1f)]
        private float startingSexualUrge = 0.75f;

        private Entity entity;
        private EntityManager entityManager;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            this.entity = entity;
            this.entityManager = dstManager;

            AddComp(new AnimalTypeData {
                AnimalTypeId = animalType.GetInstanceID(),
                AnimalName = animalType.Name
            });

            AddComp(new DropOnDeath
            {
                Prefab = conversionSystem.GetPrimaryEntity(animalType.Meat)
            });

            AddCompObject(new AnimalPrefab
            {
                Prefab = animalType.Baby
            });

            DynamicBuffer<PreyTypesElement> preyBuffer = AddBuffer<PreyTypesElement>();
            foreach (AnimalType animalType in animalType.Prey)
            {
                if (animalType.GetInstanceID() == this.animalType.GetInstanceID()) continue;

                preyBuffer.Add(new PreyTypesElement { AnimalTypeId = animalType.GetInstanceID() });
            }

            DynamicBuffer<FoodTypesElement> foodBuffer = AddBuffer<FoodTypesElement>();
            foreach (FoodType foodType in animalType.Food)
            {
                if (foodType.GetInstanceID() == this.animalType.Meat.GetInstanceID()) continue;

                foodBuffer.Add(new FoodTypesElement { FoodTypeId = foodType.GetInstanceID() });
            }

            AddComp(new BaseSpeed
            {
                Value = animalType.MovementSpeed
            });

            AddComp(new BaseHearingRange
            {
                Value = animalType.HearingRange
            });

            AddComp(new BaseVisionRange
            {
                Value = animalType.VisionRange
            });

            AddComp(new MovementSpeed());
            AddComp(new Hearing());
            AddComp(new Vision
            {
                Angle = animalType.VisionAngle,
                Range = 0
            });

            if (animalType.MaxHunger > 0)
            {
                AddComp(new HungerData
                {
                    Hunger = animalType.MaxHunger * startingHunger
                });
            }

            AddComp(new MaxHungerData
            {
                MaxHunger = animalType.MaxHunger
            });

            AddComp(new HungerLimit
            {
                Value = animalType.HungerLimit
            });

            if (animalType.MaxThirst > 0)
            {
                AddComp(new ThirstData
                {
                    Thirst = animalType.MaxThirst * startingThirst
                });
            }

            AddComp(new MaxThirstData
            {
                MaxThirst = animalType.MaxThirst
            });

            AddComp(new ThirstLimit
            {
                Value = animalType.ThirstLimit
            });

            if (animalType.MaxSexualUrge > 0)
            {
                AddComp(new SexualUrgesData
                {
                    Urge = animalType.MaxSexualUrge * startingSexualUrge
                });
            }

            AddComp(new MaxSexualUrgesData
            {
                MaxUrge = animalType.MaxSexualUrge
            });

            AddComp(new MatingLimit
            {
                Value = animalType.MatingLimit
            });

            AddComp(new BraveryData
            {
                Value = animalType.Bravery
            });

            AddComp(new LifespanData
            {
                Value = animalType.Lifespan
            });

            AddComp(new InfertilityData
            {
                InfertilityAge = animalType.Lifespan * animalType.InfertilityAge
            });

            AddComp(new GestationData
            {
                GestationPeriod = animalType.GestationPeriod
            });

            AddComp(new MovementTerrain
            {
                MovesOnLand = animalType.Land,
                MovesOnWater = animalType.Water
            });
        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.Add(animalType.Meat);
        }

        private void AddComp<T>(T componentData) where T : struct, IComponentData
            => entityManager.AddComponentData<T>(entity, componentData);

        private void AddCompObject<T>(T component) where T : class, IComponentData
            => entityManager.AddComponentData<T>(entity, component);

        private DynamicBuffer<T> AddBuffer<T>() where T : struct, IBufferElementData
            => entityManager.AddBuffer<T>(entity);
    }
}
