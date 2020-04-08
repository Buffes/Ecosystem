
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Ecosystem.ECS.Animal;
using Unity.Collections;
using Ecosystem.ECS.Targeting.Sensors;
using Ecosystem.ECS.Targeting.Targets;
using Ecosystem.ECS.Death;
using Ecosystem.Gameplay;


public class WriteToCSVSystem : SystemBase
{

    public static List<(int, Entity)> list = new List<(int, Entity)>();

    protected override void OnUpdate()
    {
        //list.Clear();
        Entities.WithoutBurst().
            ForEach((Entity entity, in AnimalTypeData animalTypeData
           ) =>
           {

               if (!list.Contains((animalTypeData.AnimalTypeId, entity)))
                   list.Add((animalTypeData.AnimalTypeId, entity));

           }).Run();

    }
        
   
}


