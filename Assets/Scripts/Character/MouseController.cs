using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //also sets the cursor to be invisible
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            SwitchCursorLockSate();
        }
    }

    private void SwitchCursorLockSate()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None; //also sets cursor to be visible
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
