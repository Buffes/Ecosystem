using UnityEngine;

namespace Ecosystem.Samples
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField]
        private Transform target = default;
        [SerializeField]
        [Range(0, 1)]
        private float lerpSpeed = 0.3f;
        [SerializeField]
        private float scrollSpeed = 10f;

        private Vector3 offsetDirection;
        private float offsetAmount;

        private void Start()
        {
            Vector3 offset = transform.position - target.position;
            offsetDirection = Vector3.Normalize(offset);
            offsetAmount = Vector3.Magnitude(offset);
        }

        private void Update()
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                offsetAmount += scrollSpeed * -Input.GetAxis("Mouse ScrollWheel");
            }

            transform.position = Vector3.Lerp(
                transform.position,
                target.position + offsetDirection * offsetAmount,
                lerpSpeed);
            /*Vector3 cameraPosition = transform.position;
            cameraPosition = Vector3.Lerp(cameraPosition, targetPosition, lerpSpeed);
            //cameraPosition.x += 0.2f;
            transform.position = cameraPosition;*/
        }
    }
}
