using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accelerator : Planet {

    public float chang_rate;
    private float origSpeed = 0;
    public float maxSpeed;
    public float origTime;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (thePlayerOnPlanet == null)
            checkCatching();
        else{
            if(origSpeed < 1){
                origSpeed = thePlayerOnPlanet.transform.GetChild(0).GetComponent<spacecraft>().rotating_speed;
                origTime = thePlayerOnPlanet.transform.GetChild(0).GetChild(0).GetComponent<TrailRenderer>().time;
                StartCoroutine(Speedup());
            }
        }
	}

    IEnumerator Speedup()
    {
        while (thePlayerOnPlanet != null && thePlayerOnPlanet.transform.GetChild(0).GetComponent<spacecraft>().rotating_speed < maxSpeed)
        {
            thePlayerOnPlanet.transform.GetChild(0).GetComponent<spacecraft>().rotating_speed += chang_rate;

            thePlayerOnPlanet.transform.GetChild(0).GetChild(0).GetComponent<TrailRenderer>().time -= (float)chang_rate * 0.2f;
            //thePlayerOnPlanet.transform.GetChild(0).GetComponent<spacecraft>()
            //Debug.Log(thePlayerOnPlanet.transform.GetChild(0).GetComponent<spacecraft>().rotating_speed);
            thePlayerOnPlanet.transform.GetChild(0).GetComponent<spacecraft>().inwardVel += (float)chang_rate * 0.05f;
                
            yield return new WaitForSeconds(0.5f);
        }

    }

    public override void catchedAction(spacecraft sc){
        origTime = thePlayerOnPlanet.transform.GetChild(0).GetChild(0).GetComponent<TrailRenderer>().time;
        return;
    }

    public override void playerLeave(){
        thePlayerOnPlanet.transform.GetChild(0).GetChild(0).GetComponent<TrailRenderer>().time = origTime;
        origSpeed = 0;
        base.playerLeave();
    }
}
