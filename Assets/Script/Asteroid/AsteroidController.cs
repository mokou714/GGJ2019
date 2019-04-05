using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour {
    public int readyNum = 0;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {

        if(readyNum == transform.childCount){
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<orbitAsteroid>().movingBack = false;
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<orbitAsteroid>().tell = false;
            }
            readyNum = 0;
        }
	}

    public void recover(){
        for (int i = 0; i < transform.childCount; i ++){
            transform.GetChild(i).GetComponent<orbitAsteroid>().movingBack = true;
        }
    }
}
