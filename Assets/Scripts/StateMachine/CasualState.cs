using UnityEngine;
using Ecosystem.Attributes;

namespace Ecosystem.StateMachines {
    public class CasualState : IState {

        Animal owner;
        Vector3 nextTarget;
        private float timeSinceLastFrame;
        private readonly float pathfindInterval = 3f;

        public CasualState(Animal owner) 
        { 
            this.owner = owner; 
            timeSinceLastFrame = Random.Range(0f, pathfindInterval); // Offset to make animals update at different points
        }

        public void Enter() 
        {
            owner.GetSensors().LookForRandomTarget(true);
            nextTarget = owner.GetMovement().GetPosition();
        }

        public void Execute() 
        {
            timeSinceLastFrame += Time.deltaTime;
            if (timeSinceLastFrame < pathfindInterval) return;
            timeSinceLastFrame = 0f;

            if (!owner.GetSensors().FoundRandomTarget()) return;
            
            Vector3 currentPos = owner.GetMovement().GetPosition();
            nextTarget = owner.GetSensors().GetFoundRandomTargetInfo();
            
            // Move owner
            owner.Move(nextTarget,0f,200);
        }


        public void Exit() 
        {
            owner.GetSensors().LookForRandomTarget(false);
        }
    }
}