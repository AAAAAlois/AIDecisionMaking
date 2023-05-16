using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public float moveSpeed = 10f;  

    void Update()
    {
        
        float horizontal = -Input.GetAxis("Vertical");
        float vertical = Input.GetAxis("Horizontal");

       
        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
        float distance = moveSpeed * Time.deltaTime;

        
        transform.position += moveDirection * distance;
    }
}
