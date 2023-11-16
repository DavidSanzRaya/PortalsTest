using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class InCamDetector : MonoBehaviour
{
    [SerializeField] GameObject renderPlane;
    Plane[] cameraFrustum;
    Camera cam;

    private void Awake()
    {
        cam = this.gameObject.GetComponent<Camera>();
    }

    private void Update()
    {
        CheckFrustrumVisibility();
    }

    private void OnDestroy()
    {
        Portal.OnTravelPortal -= CheckFrustrumVisibility;
    }


    private void OnPreCull()
    {
        renderPlane.layer = 6;
    }

    private void OnPostRender()
    {
        renderPlane.layer = 0;
    }

    public void CheckFrustrumVisibility()
    {
        cameraFrustum = GeometryUtility.CalculateFrustumPlanes(cam);

        foreach (MeshRenderer otherRenderPlane in GameLogic.instance.renderPlanes)
        {
            if (GeometryUtility.TestPlanesAABB(cameraFrustum, otherRenderPlane.GetComponent<Collider>().bounds) && (otherRenderPlane.gameObject != renderPlane))
            {
                otherRenderPlane.GetComponent<RenderPlaneRayCaster>().CheckVisibility(cam, renderPlane);
            }
        }

    }
}
