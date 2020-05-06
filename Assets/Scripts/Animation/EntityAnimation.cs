using Ecosystem.ECS.Hybrid;
using Unity.Mathematics;
using UnityEngine;

namespace Ecosystem.Animation {
    public class EntityAnimation : MonoBehaviour {
        private Animator animator;

        [SerializeField]
        private Movement movement = default;

        void Start() {
            animator = GetComponent<Animator>();
        }

        void Update() {
            if(math.length(movement.GetMovementInput()) != 0) {
                RunAnimation();
            } else {
                IdleAnimation();
            }
        }

        private void IdleAnimation() {
            if(animator != null) {
                animator.SetBool("Run", false);
            }

        }

        private void RunAnimation() {
            if (animator != null) {
                animator.SetBool("Run", true);
            }
        }
    }
}

