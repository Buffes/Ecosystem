using Unity.Entities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Ecosystem.CameraControl
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("How fast the camera moves around with the keyboard controls")]
        private float moveSpeed = 10f;
        [SerializeField]
        [Tooltip("How much the speed is increased when boosting")]
        private float boostFactor = 2f;
        [SerializeField]
        [Tooltip("How fast the camera moves in and out with Mouse Wheel")]
        private float zoomSpeed = 1f;
        [SerializeField]
        [Tooltip("How fast the camera rubber bands toward the target position")]
        private float smoothSpeed = 3f;
        [SerializeField]
        [Tooltip("How fast the camera rotates when looking around")]
        private float sensitivity = 1f;

        private Vector3 target;
        private float currentMoveSpeed;

        private Vector2 inputDirection;
        private float inputElevation;
        private Vector2 inputRotation;
        private bool inputBoost;
        private bool inputActivateLookAround;

        private FollowedEntitySystem followedEntitySystem;

        private void Awake()
        {
            followedEntitySystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<FollowedEntitySystem>();
        }

        private void Start()
        {
            target = transform.position;
        }

        private void Update()
        {
            LookAround();
            if (followedEntitySystem.FollowingEntity) FollowEntity();
            else MoveTarget();
            FollowTarget();
        }

        private void LookAround()
        {
            //Look around with Right Mouse
            if (inputActivateLookAround && !IsPointerOverUI())
            {
                LockCursor(true);

                var eulerAngles = transform.eulerAngles;
                eulerAngles += sensitivity / 10 * new Vector3(-inputRotation.y, inputRotation.x);
                if (eulerAngles.x > 180f && eulerAngles.x < 270.1f) eulerAngles.x = 270.1f;
                if (eulerAngles.x < 180f && eulerAngles.x > 89.9f) eulerAngles.x = 89.9f;
                transform.eulerAngles = eulerAngles;
            }
            else
            {
                LockCursor(false);
            }
        }

        private void FollowEntity()
        {
            Vector3 entityPosition = followedEntitySystem.Position;
            float distance = Vector3.Distance(transform.position, entityPosition);
            float targetDistance = Vector3.Distance(target, entityPosition);

            transform.position = entityPosition + -transform.forward * distance;
            target = entityPosition + -transform.forward * targetDistance;
        }

        private void MoveTarget()
        {
            currentMoveSpeed = moveSpeed;

            // Move faster with Shift
            if (inputBoost)
            {
                currentMoveSpeed *= boostFactor;
            }

            // Move
            if (inputDirection.sqrMagnitude != 0)
            {
                Vector3 localDirection = transform.TransformDirection(new Vector3(inputDirection.x, 0, inputDirection.y));
                localDirection.y = 0;
                if (localDirection.sqrMagnitude == 0)
                {
                    localDirection = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w) * Vector3.forward;
                }
                target += currentMoveSpeed * localDirection.normalized * Time.unscaledDeltaTime;
            }

            // Elevation
            target.y += currentMoveSpeed * inputElevation * Time.unscaledDeltaTime;
        }

        private void FollowTarget()
        {
            transform.position = Vector3.Lerp(transform.position, target, smoothSpeed * Time.deltaTime);
        }

        private void LockCursor(bool locked)
        {
            Cursor.visible = !locked;
            Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        }

        private bool IsPointerOverUI()
        {
            EventSystem eventSystem = EventSystem.current;
            return eventSystem != null && eventSystem.IsPointerOverGameObject();
        }

        public void OnMovement(InputValue value) => inputDirection = value.Get<Vector2>();
        public void OnElevation(InputValue value) => inputElevation = value.Get<float>();
        public void OnRotation(InputValue value) => inputRotation = value.Get<Vector2>();
        public void OnZoom(InputValue value) => target += zoomSpeed / 100 * transform.forward * value.Get<float>();
        public void OnBoost(InputValue value) => inputBoost = value.Get<float>() > 0;
        public void OnActivateLookAround(InputValue value) => inputActivateLookAround = value.Get<float>() > 0;
        public void OnToggleFollowTarget() => followedEntitySystem.ToggleFollow();
    }
}
