using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    [SerializeField]
    private float moveSpeed = 0.5f;
    
    [SerializeField]
    private float scrollSpeed = 10f;
    
    void Update () {
        // Movement using WASD or arrow keys.
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) {
            transform.position += moveSpeed * new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        }

        // Hold down space and move mouse.
        if (Input.GetAxisRaw("Jump") != 0) {
            transform.position += moveSpeed * new Vector3(-Input.GetAxisRaw("Mouse X"), 0, -Input.GetAxisRaw("Mouse Y"));
        }

        // Scrolling to move vertically
        if (Input.GetAxis("Mouse ScrollWheel") != 0) {
            transform.position += scrollSpeed * new Vector3(0, -Input.GetAxis("Mouse ScrollWheel"), 0);
        }
    }
}