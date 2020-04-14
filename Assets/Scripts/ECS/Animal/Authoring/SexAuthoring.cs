using Ecosystem.ECS.Animal;
using Unity.Entities;
using UnityEngine;

public class SexAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public enum SexSetting
    {
        Random,
        Male,
        Female
    }

    [SerializeField]
    private SexSetting sexSetting = default;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        Sex sex;
        if (sexSetting == SexSetting.Male) sex = Sex.Male;
        else if (sexSetting == SexSetting.Female) sex = Sex.Female;
        else sex = Random.value < 0.5f ? Sex.Male : Sex.Female;

        dstManager.AddComponentData(entity, new SexData { Sex = sex });
    }
}
