using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class delockmouse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private bool locked;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            locked = !locked;

            if (locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;

            }
            
        }
    }
}
