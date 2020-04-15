using Unity.Entities;
using Ecosystem.ECS.Targeting.Sensing;

namespace Ecosystem.ECS.Targeting.FindSystems
{
    [UpdateAfter(typeof(SensingSystemGroup))]
    public class FindSystemGroup : ComponentSystemGroup { }
}
