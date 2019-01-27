using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAbsorb : MonoBehaviour {

	bool startedAbsorb = false;

	private void OnTriggerEnter2D(Collider2D other) {
		ParticlesAbsorb pA;
		if (!startedAbsorb && (pA = other.GetComponent<ParticlesAbsorb>()) != null) {
			StartCoroutine(pA.absorbParticles(GetComponent<ParticleSystem>()));
			startedAbsorb = true;
		}
	}

}
