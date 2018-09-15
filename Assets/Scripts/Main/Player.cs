using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vox;

[RequireComponent(typeof(Rigidbody))]
public class Player : Actor
{
    private InputManager _inputManager;
    private Rigidbody _rigidBody;

    #region Initialization
    // Use this for initialization
    void Start ()
    {
        _inputManager = new InputManager();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        InitRigidBody();
    }

    private void InitRigidBody()
    {
        _rigidBody = GetComponent<Rigidbody>();

        _rigidBody.useGravity = false;        
    }

    #endregion

    // Update is called once per frame
    void Update ()
    {
        CheckKeyboardInputs();
    }

    private void CheckKeyboardInputs()
    {
        Vector3 __moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            __moveDirection += transform.forward;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            __moveDirection -= transform.forward;
        }

        if (Input.GetKey(KeyCode.A))
        {
            __moveDirection -= transform.right;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            __moveDirection += transform.right;
        }

        Move(__moveDirection);
    }

    private void Move(Vector3 p_direction)
    {
        _rigidBody.velocity = p_direction * Velocity * Time.deltaTime * 100f;
    }
}
