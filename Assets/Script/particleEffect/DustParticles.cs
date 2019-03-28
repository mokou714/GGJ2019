using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DustParticles : MonoBehaviour {

	private GameObject planet;
	private float savedPlanetGravityRad;

	private ParticleSystem particles;

    private bool checkedSize = false;
	// Use this for initialization
	void Start () {

		planet = transform.parent.gameObject;
        Debug.Log(planet);

		particles = GetComponent<ParticleSystem>();
		savedPlanetGravityRad = particles.shape.radius;



        //Debug.Log("savedgravityRad: " + transform.parent.gameObject + ", " + savedPlanetGravityRad);
        if (planet.GetComponent<dustPlanet>().dustAmount == 0)
            savedPlanetGravityRad = 0f;

        //if (Application.isEditor)
        //{
        //    checkSetDustSize();
        //}
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void checkSetDustSize(Planet planetGrav) {
        if ((planet == null) || (planetGrav = planet.GetComponent<dustPlanet>()) == null)
			return;
   //     if (Mathf.Abs(savedPlanetGravityRad - planetGrav.catchRadius) < 1e-3)
			//return;
        //Debug.Log(transform.parent.name + " Checking size: " + planetGrav.catchRadius);

        savedPlanetGravityRad = planetGrav.catchRadius;
        float x = Mathf.Log(1.6f + savedPlanetGravityRad);
        Debug.Log(planetGrav.name + " Checking size: " + planetGrav.catchRadius + "," + x);

		var pShape = particles.shape;
        pShape.radius = x;

        transform.localScale = new Vector3(x, x, x);

        //Debug.Log(transform.localScale);
		CircleCollider2D coll;
		if ((coll = GetComponent<CircleCollider2D>()) != null) {
			coll.radius = savedPlanetGravityRad;
		}

        checkedSize = true;
		//particles.Stop();
		//print("Changed");
	}

}
