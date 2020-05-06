using Ecosystem.ECS.Hybrid;
using Unity.Mathematics;
using UnityEngine;

namespace Ecosystem.Animation {
    [RequireComponent(typeof(Animator))]
    public class EntityAnimation : MonoBehaviour {
        private Animator animator;

        [SerializeField]
        private Movement movement = default;

        void Awake() {
            animator = GetComponent<Animator>();
        }

        void Update() {
            if (math.length(movement.GetMovementInput()) != 0) {
                RunAnimation();
            } else {
                IdleAnimation();
            }
        }

        private void IdleAnimation() {
            animator.SetBool("Run", false);
        }

        private void RunAnimation() {
            animator.SetBool("Run", true);
        }
    }
}

