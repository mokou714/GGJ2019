using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camController : MonoBehaviour {

    //from player
    public Vector3 zoomPos;
    public GameObject player;

    //preset parameters
    public float zoomSpeed;

    //zoom switches for debug
    public bool startZoomIn;
    public bool endZoomIn;


    public GameObject smallLevels;
    public bool zoomEnd;



    private float transTime;
    private float sizeChangeVelocity;
    private bool calculated;
    private float originFOV;
    private GameObject levelPlanet;
    



	// Use this for initialization
	void Start () {
        originFOV = GetComponent<Camera>().fieldOfView;

    }
	
	// Update is called once per frame
	void Update () {

        if (startZoomIn) {
            ZoomIn();
        }
        else if (endZoomIn) {
            ZoomOut();
        }

        //trigger zoomIN after entering the planet
        if (player.transform.GetChild(0).GetComponent<spacecraft>().rotatingPlanet != null
            && player.transform.GetChild(0).GetComponent<spacecraft>().rotatingPlanet.tag == "levelPlanet"
            && zoomPos == new Vector3(0,0,0)
            ) {

            levelPlanet = player.transform.GetChild(0).GetComponent<spacecraft>().rotatingPlanet;
            startZoomIn = true;
            zoomPos = levelPlanet.transform.position;
        }
        //trigger zoomOut after leaving the planet
        else if(player.transform.GetChild(0).GetComponent<spacecraft>().rotatingPlanet == null
            && zoomPos != new Vector3(0, 0, 0)) {
            endZoomIn = true;
         }

    }


    void ZoomIn() {
        if (GetComponent<Camera>().fieldOfView > levelPlanet.GetComponent<levelPlanet>().minZoomInFOV)
        {
        GetComponent<Camera>().fieldOfView -= zoomSpeed * Time.deltaTime;

        Vector3 currentPos = transform.position;
        Vector3 transVelocity = (zoomPos - currentPos).normalized * zoomSpeed * 2.5f;
        Vector3 newPos = new Vector3(currentPos.x + transVelocity.x * Time.deltaTime,
        currentPos.y + transVelocity.y * Time.deltaTime,
            -20f);

        transform.position = newPos;

        }
        else {
            //zooming finished
            startZoomIn = false;
            //show levels
            levelPlanet.GetComponent<levelPlanet>().showingSelection = true;
        }
        //Debug.Log("Zoom!!!!!");
    }

    void ZoomOut() {
        if (GetComponent<Camera>().fieldOfView < originFOV)
        {
            GetComponent<Camera>().fieldOfView += zoomSpeed * Time.deltaTime;

            Vector3 currentPos = transform.position;
            Vector3 transVelocity =  -currentPos.normalized * zoomSpeed * 2.5f;
            Vector3 newPos = new Vector3(currentPos.x + transVelocity.x * Time.deltaTime,
            currentPos.y + transVelocity.y * Time.deltaTime,
                -20f);

            transform.position = newPos;

        }
        else
        {
            //zooming finished
            endZoomIn = false;
            zoomPos = new Vector3(0, 0, 0);

            //hind selection
            levelPlanet.GetComponent<levelPlanet>().hidingSelection = true;
            levelPlanet = null;
        }
    }
}
