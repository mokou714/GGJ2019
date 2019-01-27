using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartParticles : MonoBehaviour {

	[SerializeField]
	private float startInSeconds;

	private ParticleSystem particles;

	// Use this for initialization
	void Start () {
		particles = GetComponent<ParticleSystem>();
		particles.Stop();
		StartCoroutine(waitStartParticles());
	}
	
	private IEnumerator waitStartParticles() {
		float currTime = 0;
		while (currTime < startInSeconds) {
			currTime += Time.deltaTime;
			yield return 0;
		}
		particles.Play();
	}
	
}
