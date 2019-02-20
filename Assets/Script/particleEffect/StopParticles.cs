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
        //Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        //pos.x = Mathf.Clamp(pos.x, 0.1f, 0.9f);
        ////pos.y = Mathf.Clamp(pos.y, 0.1f, 0.9f);
        //transform.position = Camera.main.ViewportToWorldPoint(pos);

        //Debug.Log(Camera.main.ViewportToWorldPoint(pos));

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
        //Debug.Log("Start beginning");
        particles.Play();
        StartCoroutine(waitStopParticles());
    }
	
}
