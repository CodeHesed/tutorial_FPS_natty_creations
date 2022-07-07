using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMap : MonoBehaviour
{
    private InputManager inputManager;
    public bool mapOn;
    private GameObject cam;
    private GameObject radar;
    private Vector3 centerPos;

    [SerializeField]
    private GameObject radarPrefab;

    [SerializeField]
    private GameObject map;

    void Start()
    {
        cam = GameObject.Find("Main Camera");
        inputManager = GetComponent<InputManager>();
        mapOn = false;

        centerPos.x = cam.GetComponent<Camera>().pixelWidth / 2;
        centerPos.y = cam.GetComponent<Camera>().pixelHeight / 2;
        centerPos.z = 0f;
        radar = Instantiate(radarPrefab, centerPos, Quaternion.identity, map.transform);
        radar.name = "Radar";
        radar.GetComponent<Image>().enabled = false;
    }

    void Update()
    {
        if (inputManager.onFoot.Map.triggered)
        {
            mapOn = !mapOn;
            radar.GetComponent<Image>().enabled = !radar.GetComponent<Image>().enabled;
        }
    }
}
