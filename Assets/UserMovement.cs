using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserMovement : MonoBehaviour
{
    private float speed = 3.5f;
    bool crouched = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey("w"))
        {
            pos.x += speed * Time.deltaTime;
        }
        if (Input.GetKey("s"))
        {
            pos.x -= speed * Time.deltaTime;
        }
        if (Input.GetKey("d"))
        {
            pos.z -= speed * Time.deltaTime;
        }
        if (Input.GetKey("a"))
        {
            pos.z += speed * Time.deltaTime;
        }
        if (Input.GetKeyDown("c"))
        {
            /*pos.y = 2.7f;*/
            crouched = !crouched;
            if (crouched)
            {
                pos.y = 2.7f;
            }
            else
            {
                pos.y = 4f;
            }
        }


        transform.position = pos;
    }
}
