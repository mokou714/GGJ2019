﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorInverter : MonoBehaviour {

    public float invertingSpeed;
    public bool inverting;
    public bool invertingBack;

	// Use this for initialization
	void Start () {

        //Debug.Log(Screen.width + "," + Screen.height + "," + (float)Screen.width/Screen.height);
        //transform.parent.gameObject.GetComponent<Camera>().orthographicSize =(int)((float)Screen.height / Screen.width * Constants.screenRatio) + 1;
	}
	
	// Update is called once per frame
	void Update () {
        if (inverting)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0f, 0f, transform.position.z), Time.deltaTime * invertingSpeed);
            if (transform.position == new Vector3(0f, 0f, transform.position.z))
                inverting = false;
        }
        else if(invertingBack)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(-45f, 45f, transform.position.z), Time.deltaTime * invertingSpeed);
            if (transform.position == new Vector3(-45f, 45f, transform.position.z))
                invertingBack = false;
        }

    }
}
