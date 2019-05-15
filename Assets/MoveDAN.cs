using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDAN : MonoBehaviour
{
    public float rotationSpeed = 80;
    public float gravity = 100;
    float rotationY;

    Vector3 movedir = Vector3.zero;

    public float speed = 2.5f;
    public float runspeed = 3;
    public Rigidbody rb;
    // public CapsuleCollider cc;
    public float jumpForce = 7;

    CharacterController controller;
    Animator anim;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        // cc = GetComponent<CapsuleCollider>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        anim.SetFloat("Speed", 0);
        anim.SetInteger("Direction", 31);

        if (Input.GetKey(KeyCode.W))
            {
                 anim.SetFloat("Speed", 1);
                 anim.SetInteger("Direction", 0);
           

        }
        


        rotationY += Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed;
        transform.rotation = Quaternion.Euler(0, rotationY, 0);
    }
}