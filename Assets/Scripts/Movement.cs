using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Vector2 movementInput;
    private Vector3 direction;
    private bool hasMoved;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow)){
            Vector3 position = this.transform.position;
            position.x -= 0.01f;
            this.transform.position = position;
        } 
        
        if (Input.GetKey(KeyCode.RightArrow)){
            Vector3 position = this.transform.position;
            position.x += 0.01f;
            this.transform.position = position;
        } 

        if (Input.GetKey(KeyCode.UpArrow)) {
            Vector3 position = this.transform.position;
            position.z += 0.01f;
            this.transform.position = position;
        } 

        if (Input.GetKey(KeyCode.DownArrow)) {
            Vector3 position = this.transform.position;
            position.z -= 0.01f;
            this.transform.position = position;
        } 
    }

    public void movement(){

    }
}
 