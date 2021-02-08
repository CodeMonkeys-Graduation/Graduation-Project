using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListCastTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics.Linecast(transform.position, transform.position + transform.up * 10f))
        {
            Debug.Log("Hit");
        }
        Debug.DrawLine(transform.position, transform.position + transform.up * 10f, Color.blue);
    }
}
