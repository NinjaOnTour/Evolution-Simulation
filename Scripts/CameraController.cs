using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float MoveSpeed = 10f;
    public float ZoomSpeed = 150f;

    private Camera cam;
    private float horizontal;
    private float vertical;
    private float zoom;

    private void Start()
    {
        cam= Camera.main;
    }

    private void FixedUpdate()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        zoom = -Input.GetAxis("Mouse ScrollWheel");

        transform.position += new Vector3(horizontal, vertical, 0f) * MoveSpeed * Time.fixedDeltaTime;
        cam.orthographicSize += zoom * ZoomSpeed * Time.fixedDeltaTime; 
    }
}