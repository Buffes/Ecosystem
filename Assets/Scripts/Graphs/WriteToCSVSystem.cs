
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Ecosystem.ECS.Animal;
using Ecosystem.ECS.Stats.Base;


public class WriteToCSVSystem : SystemBase
{

    public static List<(int, float, float, float, Entity)> attributeList = new List<(int, float,float, float, Entity)>();

    protected override void OnUpdate()
    {
        Entities.WithoutBurst().
            ForEach((Entity entity, in AnimalTypeData animalTypeData, in BaseSpeed baseSpeed, in BaseHearingRange baseHearingRange, in BaseVisionRange baseVisionRange
           ) =>
           {

               if (!attributeList.Contains((animalTypeData.AnimalTypeId, baseSpeed.Value, baseHearingRange.Value, baseVisionRange.Value, entity)))
               {
                   attributeList.Add((animalTypeData.AnimalTypeId, baseSpeed.Value, baseHearingRange.Value, baseVisionRange.Value, entity));
               }    
                   
           }).Run();
    }
}


