using UnityEngine;
using Unity.Entities;
using Ecosystem.Attributes;
using Ecosystem.ECS.Grid;
using Ecosystem.ParticleSystems;

namespace Ecosystem.StateMachines
{
    public class HuntState : IState
    {

        Animal owner;
        private float timeSinceLastFrame = 0f;
        private float pathfindInterval = 1f;

        public HuntState(Animal owner) { this.owner = owner; }

        public void Enter()
        {
            // Starts sprint
            owner.GetMovement().Sprint(true);
            owner.GetMovement().Fly(false);
        }

        public void Execute()
        {
            timeSinceLastFrame += Time.deltaTime;
            if (timeSinceLastFrame < pathfindInterval) return;
            timeSinceLastFrame = 0f;
            var preyInfo = owner.GetSensors().GetFoundPreyInfo();
            Vector3 preyPos = preyInfo.Position;
            Vector3 currentPos = owner.GetMovement().GetPosition();
            float diffLength = Vector3.Distance(preyPos, currentPos);

            if (diffLength <= 1f)
            {
                Entity prey = owner.GetSensors().GetFoundPreyInfo().Entity;
                ParticleMono.InstantiateParticles(ParticleMono.kill, currentPos, 2f);
                owner.GetInteraction().Kill(prey);
            }
            Vector3 predictedPosition = preyInfo.PredictedPosition;
            owner.Move(predictedPosition, 0f, 200);
        }

        public void Exit()
        {
            // End sprint
            owner.GetMovement().Sprint(false);
            owner.GetSensors().LookForPrey(false);
        }
    }
}