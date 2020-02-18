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
        if (Input.GetKey(KeyCode.LeftArrow) && hasMoved == false){
            Vector3 position = this.transform.position;
            for (int i = 0; i < 20; i++){
                position.x -= 0.1f;
                this.transform.position = position;
            }
            hasMoved = true;
        } else if (Input.GetKeyUp(KeyCode.LeftArrow)){
            hasMoved = false;
        }
        
        if (Input.GetKey(KeyCode.RightArrow) && hasMoved == false){
            Vector3 position = this.transform.position;
            for (int i = 0; i < 20; i++){
                position.x += 0.1f;
                this.transform.position = position;
            }
            hasMoved = true;
        } else if (Input.GetKeyUp(KeyCode.RightArrow)){
            hasMoved = false;
        }
    }

    public void movement(){

    }
}
 