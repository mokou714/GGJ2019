using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopParticles : MonoBehaviour {

	[SerializeField]
	private float stopInSeconds;

	[SerializeField]
	private float destroyInAnother;

	private ParticleSystem particles;

	// Use this for initialization
	void Start () {
		particles = GetComponent<ParticleSystem>();
		StartCoroutine(waitStopParticles());
	}
	
	private IEnumerator waitStopParticles() {
		float currTime = 0;
		while (currTime < stopInSeconds) {
			currTime += Time.deltaTime;
			yield return 0;
		}
		particles.Stop();
		Destroy(gameObject, destroyInAnother);
	}
	
}
