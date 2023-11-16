using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelPortal : MonoBehaviour
{
    public Vector3 lastPositionRespectPortal { get; set; }

    public void Teleport(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }
}
