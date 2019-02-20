using System.Collections;
using UnityEngine;

public class StartParticles : MonoBehaviour {

	[SerializeField]
	private float startInSeconds;

	private ParticleSystem particles;

    private CapsuleCollider2D collider;
	// Use this for initialization
	void Start () {
        //Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        //pos.x = Mathf.Clamp(pos.x, 0.1f, 0.9f);
        ////pos.y = Mathf.Clamp01(pos.y);
        //transform.position = Camera.main.ViewportToWorldPoint(pos);
        collider = transform.GetChild(2).GetComponent<CapsuleCollider2D>();
        if (!collider.isTrigger)
            collider.isTrigger = true;
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
