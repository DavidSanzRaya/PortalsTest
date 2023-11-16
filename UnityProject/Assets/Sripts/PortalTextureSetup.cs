using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class PortalTextureSetup : MonoBehaviour
{
    [SerializeField] private GameObject[] portals;

    private void Awake()
    {
        CreateRenderTexture();
    }

    public void CreateRenderTexture()
    {
        foreach (var portal in portals)
        {
            Camera portalCamera = portal.GetComponentInChildren<Camera>();

            if (portalCamera.targetTexture != null)
            {
                portalCamera.targetTexture.Release();
            }

            Portal portalBehaviour = portal.GetComponent<Portal>();

            //Creamos una rendertexture con el tamaño adecuado de la camara/pantalla
            portalCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 0);
            portalBehaviour.exit.screen.material.mainTexture = portalCamera.targetTexture;
        }
    }
}
