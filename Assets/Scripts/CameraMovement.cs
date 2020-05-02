using UnityEngine;
using UnityEngine.EventSystems;

namespace Ecosystem
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
        private float zoomSpeed = 10f;
        [SerializeField]
        [Range(0, 1)]
        [Tooltip("How fast the camera rubber bands toward the target position")]
        private float smoothSpeed = 0.15f;
        [SerializeField]
        [Tooltip("How fast the camera rotates when looking around")]
        private float sensitivity = 1.5f;

        private Vector3 target;
        private float currentMoveSpeed;

        private void Start()
        {
            target = transform.position;
        }

        private void Update()
        {
            LookAround();
            MoveTarget();
            FollowTarget();
        }

        private void LookAround()
        {
            //Look around with Right Mouse
            if (Input.GetMouseButton(1) && !IsPointerOverUI())
            {
                LockCursor(true);
                transform.eulerAngles += new Vector3(sensitivity * -Input.GetAxis("Mouse Y"), sensitivity * Input.GetAxis("Mouse X"));
            }
            else
            {
                LockCursor(false);
            }
        }

        private void MoveTarget()
        {
            currentMoveSpeed = moveSpeed;

            // Move faster with Shift
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentMoveSpeed *= boostFactor;
            }

            // Move using WASD or arrow keys
            Vector3 movement = new Vector3();
            movement += currentMoveSpeed * transform.forward * Input.GetAxis("Vertical") * Time.deltaTime;
            movement += currentMoveSpeed * transform.right * Input.GetAxis("Horizontal") * Time.deltaTime;
            movement.y = 0f;
            target += movement;

            // Zoom in and out with Mouse Wheel
            target += zoomSpeed * transform.forward * Input.GetAxis("Mouse ScrollWheel");

            // Move up with Space
            if (Input.GetKey(KeyCode.Space))
            {
                target.y += currentMoveSpeed * Time.deltaTime;
            }

            // Move down with Control
            if (Input.GetKey(KeyCode.LeftControl))
            {
                target.y -= currentMoveSpeed * Time.deltaTime;
            }
        }

        private void FollowTarget()
        {
            transform.position = Vector3.Lerp(transform.position, target, smoothSpeed);
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
    }
}
