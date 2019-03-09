using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DustParticles : MonoBehaviour {

	private GameObject planet;
	private float savedPlanetGravityRad;

	private ParticleSystem particles;

	// Use this for initialization
	void Start () {
		if (!transform.parent)
			return;
		planet = transform.parent.gameObject;

		particles = GetComponent<ParticleSystem>();
		savedPlanetGravityRad = particles.shape.radius;

        //Debug.Log("savedgravityRad: " + transform.parent.gameObject + ", " + savedPlanetGravityRad);
        if (planet.GetComponent<dustPlanet>().dustAmount == 0)
            savedPlanetGravityRad = 0f;
        
        if (Application.isEditor)
        {
            checkSetDustSize();
        }
	}
	
	// Update is called once per frame
	void Update () {

	}

	private void checkSetDustSize() {
        dustPlanet planetGrav;
        if ((planet == null) || (planetGrav = planet.GetComponent<dustPlanet>()) == null)
			return;
        if (Mathf.Abs(savedPlanetGravityRad - planetGrav.catchRadius) < 1e-3)
			return;
        savedPlanetGravityRad = planetGrav.catchRadius;
		var pShape = particles.shape;
		pShape.radius = savedPlanetGravityRad;

		CircleCollider2D coll;
		if ((coll = GetComponent<CircleCollider2D>()) != null) {
			coll.radius = savedPlanetGravityRad;
		}
		//particles.Stop();
		//print("Changed");
	}

}
