using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMain : MonoBehaviour
{
    private int Boundary = 50;
    private int speed = 20;

    private int theScreenWidth;
    private int theScreenHeight;
 
    void Start() 
    {
        theScreenWidth = Screen.width;
        theScreenHeight = Screen.height;
    }
 
    void Update() 
    {
        if (Input.mousePosition.x > theScreenWidth - Boundary)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
     
        if (Input.mousePosition.x < 0 + Boundary)
        {
            transform.position -= Vector3.right * speed * Time.deltaTime;
        }
    }
}
