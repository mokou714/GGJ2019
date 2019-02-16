using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorInverter : MonoBehaviour {

    public float invertingSpeed;
    public bool inverting;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (inverting)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0f, 0f, 0f), Time.deltaTime * invertingSpeed);
            if (transform.position == new Vector3(0f, 0f, 0f))
                inverting = false;
        }

    }
}
