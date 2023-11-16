using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal exit;
    public MeshRenderer screen;
    private Camera referenceCam;
    private Camera portalCam;
    [SerializeField]
    private GameObject player;

    public Camera ReferenceCam { get { return referenceCam; } set{ referenceCam = value; }}

    [SerializeField]
    private float nearClipLimit = 0.2f;

    [SerializeField]
    private float nearClipOffset = 0.05f;

    [SerializeField]
    private int num;

    //Lista de objetos que estaninteractuando con el portal a la vez
    private List<TravelPortal> travellers = new List<TravelPortal>();

    public delegate void OnTravelPortalDelegate();
    public static event OnTravelPortalDelegate OnTravelPortal;

    void Awake()
    {
        referenceCam = Camera.main;
        portalCam = GetComponentInChildren<Camera>();
        Camera.onPreCull += OnPrecullPortal;
    }

    private void Update()
    {
        ProcessTravellers();
    }

    //Update que se procesa despues del update normal
    void LateUpdate()
    {
        ProtectScreenFromClipping(referenceCam.transform.position);
        //ChangeNearClipPlane();
    }

    private void OnPrecullPortal(Camera cam)
    {
        if (cam == portalCam)
        {
            ChangeNearClipPlane();
        }

    }

    private void OnDestroy()
    {
        Camera.onPreCull -= OnPrecullPortal;
    }

    //public void Render()
    //{
    //    exit.ProtectScreenFromClipping(portalCam.transform.position);
    //    ChangeNearClipPlane();
    //    Debug.Log("hola");
    //}

    //Comprueva si el objeto ha atravesado el portal
    private void ProcessTravellers()
    {
        for (int i = 0; i < travellers.Count; i++)
        {
            TravelPortal traveller = travellers[i];
            var m = exit.transform.localToWorldMatrix * transform.worldToLocalMatrix * traveller.transform.localToWorldMatrix;
            Vector3 distanceFromPortal = traveller.transform.position - transform.position;

            //Comprueva en que lado del portal esta el objeto y en que lado estaba antes
            int portalSide = System.Math.Sign(Vector3.Dot(distanceFromPortal, transform.forward));
            int previousPortalSide = System.Math.Sign(Vector3.Dot(traveller.lastPositionRespectPortal, transform.forward));

            if (portalSide != previousPortalSide && portalSide != 0)
            {
                traveller.Teleport(m.GetColumn(3), m.rotation);
                OnTravelPortal?.Invoke();

                // No puedes usar OnTriggerEnter/Exit ya que depende del FixedUpdate y aqui se usa el LastUpdate
                exit.NewTraveller(traveller);
                travellers.RemoveAt(i);
                i--;
            }
            else
            {
                traveller.lastPositionRespectPortal = distanceFromPortal;
            }
        }
    }

    // Evita que se vea la parte de atras del portal porque la camara atraviesa el portal
    public void ProtectScreenFromClipping(Vector3 viewPoint)
    {
        //Calcula lo gruesa que tiene que ser la camara del portal y la posicion de esta dependiendo del valor near clipping planes de la camara
        float halfHeight = referenceCam.nearClipPlane * Mathf.Tan(referenceCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float halfWidth = halfHeight * referenceCam.aspect;
        float dstToNearClipPlaneCorner = new Vector3(halfWidth, halfHeight, referenceCam.nearClipPlane).magnitude;
        float screenThickness = dstToNearClipPlaneCorner;

        Transform screenT = screen.transform;
        bool camFacingSameDirAsPortal = Vector3.Dot(transform.forward, transform.position - viewPoint) > 0;
        screenT.localScale = new Vector3(screenT.localScale.x, screenThickness , screenT.localScale.z);
        screenT.localPosition = Vector3.forward * screenThickness * ((camFacingSameDirAsPortal) ? 0.5f : -0.5f);
    }

    // Cambia el near clip plane para que los objetos que estan entre el portal y la camara no aparezcan
    private void ChangeNearClipPlane()
    {
        Transform clipPlane = transform;
        int dot = System.Math.Sign (Vector3.Dot (clipPlane.forward, transform.position - portalCam.transform.position));

        Vector3 camSpacePos = portalCam.worldToCameraMatrix.MultiplyPoint (clipPlane.position);
        Vector3 camSpaceNormal = portalCam.worldToCameraMatrix.MultiplyVector (clipPlane.forward) * dot;
        float camSpaceDst = -Vector3.Dot (camSpacePos, camSpaceNormal) + nearClipOffset;

        // Impide usar oblique clip plane quando esta cerca del portal ya que causa problemas
        if (Mathf.Abs (camSpaceDst) > nearClipLimit) {
            Vector4 clipPlaneCameraSpace = new Vector4 (camSpaceNormal.x, camSpaceNormal.y, camSpaceNormal.z, camSpaceDst);

            // Canvia el near clip plane y lo calcula con la camara de referencia para usar los setings de la camara de este
            portalCam.projectionMatrix = referenceCam.CalculateObliqueMatrix (clipPlaneCameraSpace);
        } else {
            portalCam.projectionMatrix = referenceCam.projectionMatrix;
        }
    }

    //Añade los objetos que acaban de tocar el portal
    void NewTraveller(TravelPortal traveller)
    {
        if (!travellers.Contains(traveller))
        {
            traveller.lastPositionRespectPortal = traveller.transform.position - transform.position;
            travellers.Add(traveller);
        }
    }

    

    void OnTriggerEnter(Collider other)
    {
        var traveller = other.GetComponent<TravelPortal>();
        if (traveller)
        {
            NewTraveller(traveller);
        }
    }

    void OnTriggerExit(Collider other)
    {
        var traveller = other.GetComponent<TravelPortal>();
        if (traveller && travellers.Contains(traveller))
        {
            travellers.Remove(traveller);
        }
    }
}
 