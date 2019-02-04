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

        if (planet.GetComponent<Planet>().dustAmount == 0)
            savedPlanetGravityRad = 0f;

	}
	
	// Update is called once per frame
	void Update () {
		if (Application.isEditor) {
			checkSetDustSize();
		}
	}

	private void checkSetDustSize() {
        Planet planetGrav;
        if ((planet == null) || (planetGrav = planet.GetComponent<Planet>()) == null)
			return;
        if (Mathf.Abs(savedPlanetGravityRad - planetGrav.catching_radius) < 1e-3)
			return;
		savedPlanetGravityRad = planetGrav.catching_radius;
		var pShape = particles.shape;
		pShape.radius = savedPlanetGravityRad;
		CircleCollider2D coll;
		if ((coll = GetComponent<CircleCollider2D>()) != null) {
			coll.radius = savedPlanetGravityRad;
		}
		particles.Stop();
		print("Changed");
	}

}
