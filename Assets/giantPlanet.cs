using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giantPlanet : MonoBehaviour {


    public float moveSpeed;
    public Vector3 destinationPos;
    public bool moving;

    public GameObject dustPlanet;


	// Use this for initialization
	void Start () {
        moving = true;
	}
	
	// Update is called once per frame
	void Update () {

        if (moving) {
            Vector3 newPos = transform.position;
            newPos.x -= Time.deltaTime * moveSpeed;

            Vector3 _newPos = dustPlanet.transform.position;
            _newPos.x -= Time.deltaTime * moveSpeed;


            if (newPos.x <= 20f || dustPlanet.GetComponent<dustPlanet>().thePlayerOnPlanet != null) {
                newPos.x = 20f;
                moving = false;
            }
            transform.position = newPos;
            dustPlanet.transform.position = _newPos;

            
        }
    }
}
