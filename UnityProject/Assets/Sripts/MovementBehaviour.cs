using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{

    Rigidbody rb;
    [SerializeField] private float speed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void MoveRB(Vector3 direction)
    {
        Vector3 playerVelocity = new Vector3(direction.x, 0, direction.y) * speed;
        rb.velocity = transform.TransformDirection(playerVelocity);
    }
}
