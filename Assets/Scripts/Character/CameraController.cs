using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera cam;

    public float cameraMouseSensitivity = 1.5f;
    public float fppUpLimit = -60f;
    public float fppDownLimit = 60f;
    public float tppUpLimit = -30f;
    public float tppDownLimit = 30f;

    private bool firstPerson = true;
    private Vector3 tppVector;
    private Vector3 fppVector;

    float xRotation = 0f;

    void Start()
    {
        cam = Camera.main; //make cam public and set it manually if need be

        //vectors for perspectives, maybe make these public in future
        tppVector = new Vector3(1, 2, -2.5f);
        fppVector = new Vector3(0, 1, 0);
    }


    void Update()
    {
        HandleMouseMovement();

        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchPerspective();
        }
    }

    private void HandleMouseMovement()
    {
        float horRot = Input.GetAxis("Mouse X");
        float verRot = Input.GetAxis("Mouse Y");

        xRotation -= verRot;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //perform transform rotation from mouse movement
        transform.Rotate(0, horRot * cameraMouseSensitivity, 0);
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //cam.transform.Rotate(-verRot * cameraMouseSensitivity, 0, 0);

    }

    private void SwitchPerspective()
    {
        if (firstPerson)
        {
            //switch to thirdperson
            firstPerson = false;
            cam.transform.localPosition = tppVector;
        }
        else
        {
            //switch to first person
            firstPerson = true;
            cam.transform.localPosition = fppVector;
        }
    }
}
