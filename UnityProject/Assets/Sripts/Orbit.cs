using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public float gravity;

    public bool fixedDirection;

    public bool cilinder;

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<GravityController>().gravity = this.GetComponent<Orbit>();
    }
}
