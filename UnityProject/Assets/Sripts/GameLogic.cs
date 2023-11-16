using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private static GameLogic gamelogic;

    public static GameLogic instance
    {
        get
        {
            return RequestInstance();
        }
    }

    static GameLogic RequestInstance()
    {

        if (gamelogic == null)
        {
            gamelogic = FindObjectOfType<GameLogic>();

            if (gamelogic == null)
            {
                GameObject gamelogicObject = new GameObject("GameLogic");
                gamelogic = gamelogicObject.AddComponent<GameLogic>();
            }
        }
        return gamelogic;
    }

    public PlayerController playerController;
    public Camera[] cameras;
    public MeshRenderer[] renderPlanes;
    public Portal[] portals;
    public PortalCamera[] portalCameras;
    public PortalTextureSetup portalTextureSetup;

    private void Awake()
    {
        if (playerController == null)
        {
            FindObjectOfType<PlayerController>();
        }

        cameras = FindObjectsOfType<Camera>();
        portals = FindObjectsOfType<Portal>();
        portalCameras = FindObjectsOfType<PortalCamera>();
        portalTextureSetup = FindAnyObjectByType<PortalTextureSetup>();
    }
 
}
