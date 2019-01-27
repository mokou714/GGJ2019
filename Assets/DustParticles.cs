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
	}
	
	// Update is called once per frame
	void Update () {
		if (Application.isEditor) {
			checkSetDustSize();
		}
	}

	private void checkSetDustSize() {
		Catcher planetGrav;
		if ((planet == null) || (planetGrav = planet.GetComponent<Catcher>()) == null)
			return;
		if (savedPlanetGravityRad == planetGrav.catching_radius)
			return;
		savedPlanetGravityRad = planetGrav.catching_radius;
		var pShape = particles.shape;
		pShape.radius = savedPlanetGravityRad;
		particles.Stop();
		print("Changed");
	}

}
