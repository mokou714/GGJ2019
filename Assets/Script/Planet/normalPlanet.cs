using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class normalPlanet : Planet {
    //public float catch_radius;
	// Use this for initialization
	void Start () {
        setup();
	}
	
	// Update is called once per frame
	void Update () {

	}

    void FixedUpdate()
    {
        if(thePlayerOnPlanet == null)
            checkCatching();
    }

    public override void catchedAction(spacecraft sc)
	{
        return;
	}

}
