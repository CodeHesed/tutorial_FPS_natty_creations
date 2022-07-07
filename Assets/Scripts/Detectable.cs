using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Detectable : MonoBehaviour
{
    private GameObject player;
    private GameObject cam;
    private GameObject focus;

    [SerializeField]
    private GameObject focusPrefab;

    private Vector3 lookDirection;
    private Vector3 planetDirection;
    private float angle;
    private Vector3 screenPos;
    private bool visible;
    private bool mapOn;

    void Start()
    {
        player = GameObject.Find("Player");
        cam = GameObject.Find("Main Camera");
        focus = Instantiate(focusPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Map").transform);
        focus.name = this.gameObject.name + "focus";
        focus.GetComponent<Image>().enabled = false;
    }

    void Update()
    {
        mapOn = player.GetComponent<PlayerMap>().mapOn;
        lookDirection = cam.transform.forward;
        planetDirection = transform.position - cam.transform.position;
        angle = Mathf.Acos(Vector3.Dot(Vector3.Normalize(lookDirection), Vector3.Normalize(planetDirection))) * Mathf.Rad2Deg;

        screenPos = cam.GetComponent<Camera>().WorldToScreenPoint(transform.position);
        focus.transform.position = screenPos;

        if (mapOn)
        {
            if (angle <= 30f)
            {
                focus.GetComponent<Image>().enabled = true;
            }
            else
            {
                focus.GetComponent<Image>().enabled = false;
            }
        }
        else
        {
            focus.GetComponent<Image>().enabled = false;
        }
    }
}
