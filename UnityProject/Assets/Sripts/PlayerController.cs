using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Vector2 moveInput;
    MovementBehaviour mvb;

    private void Start()
    {
        mvb = GetComponent<MovementBehaviour>();
    }

    private void FixedUpdate()
    {
        if (moveInput != Vector2.zero)
        {
            mvb.MoveRB(moveInput);
        }
    }

    void OnMove(InputValue input)
    {
        moveInput = input.Get<Vector2>();
    }
}
