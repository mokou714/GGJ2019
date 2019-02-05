using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopParticles : MonoBehaviour {

	[SerializeField]
	private float stopInSeconds;

	[SerializeField]
	private float destroyInAnother;

	private ParticleSystem particles;

    [SerializeField]
    private float startInSeconds;

	// Use this for initialization
	void Start () {
        startInSeconds = 0f;
        particles = GetComponent<ParticleSystem>();
        StartCoroutine(waitStopParticles());
	}
	

    public void ParticleReStart(){
        if(particles.isStopped)
            StartCoroutine(waitStartParticles());
    }

	private IEnumerator waitStopParticles() {
		float currTime = 0;
		while (currTime < stopInSeconds) {
			currTime += Time.deltaTime;
			yield return 0;
		}
		particles.Stop();
		//Destroy(gameObject, destroyInAnother);
	}

    private IEnumerator waitStartParticles()
    {
        float currTime = 0;
        while (currTime < startInSeconds)
        {
            currTime += Time.deltaTime;
            yield return 0;
        }
        Debug.Log("Start beginning");
        particles.Play();
        StartCoroutine(waitStopParticles());
    }
	
}
