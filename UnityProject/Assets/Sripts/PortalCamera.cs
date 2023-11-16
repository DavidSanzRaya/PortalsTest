using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    private Camera referenceCamera;
    private Transform portal;
    [SerializeField] private Transform otherPortal;

    void Start()
    {
        portal = this.transform.parent;
        transform.forward = portal.forward;
        Portal.OnTravelPortal += MoveCamera;
    }

    private void Update()
    {
        MoveCamera();
    }

    public void MoveCamera()
    {
        referenceCamera = portal.GetComponent<Portal>().ReferenceCam;
        var m = portal.transform.localToWorldMatrix * otherPortal.worldToLocalMatrix * referenceCamera.transform.localToWorldMatrix;
        transform.SetPositionAndRotation(m.GetColumn(3), m.rotation);
    }
}
