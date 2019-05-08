using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoorScript : MonoBehaviour
{
    public Camera cam;
    public GameObject Door;
    private int count;
    private float initialHeight;
    bool open;

    void Start()
    {
        open = false;
        count = 0;
        initialHeight = Door.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (open == true )
        {
            
            Door.transform.Translate(new Vector3(0f, -3f, 0f) * Time.deltaTime);
        }
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.parent.tag == "agent")
        {
            
            open = true;
           
        }
    }
    
}
