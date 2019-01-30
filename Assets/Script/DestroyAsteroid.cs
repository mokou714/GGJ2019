using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAsteroid : MonoBehaviour {


    public float lifeTime = 5.0f;

	// Use this for initialization
	void Start () {

        Invoke("Endlife", lifeTime);
	}
	
	// Update is called once per frame
	void Update () {
		
        

	}

    void Endlife()
    {
        Destroy(gameObject);
    }

}
