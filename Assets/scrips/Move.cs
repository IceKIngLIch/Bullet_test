using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float Speed=1;
    public float MouseSense=150;
    public Camera Cam;
    //public GameObject weapon;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void MoveWASD()
    {
        if (Input.GetKey(KeyCode.W) == true)
        {
            transform.position += transform.forward * Speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) == true)
        {
            transform.position += -transform.forward * Speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) == true)
        {
            transform.position += -transform.right * Speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) == true)
        {
            transform.position += transform.right * Speed * Time.deltaTime;
        }
    }
    private void MoveRotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * MouseSense * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * MouseSense * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
        Cam.transform.Rotate(Vector3.left * mouseY);
    }
    // Update is called once per frame
    void Update()
    {
        MoveWASD();
        MoveRotate();

        
        
    }
}
