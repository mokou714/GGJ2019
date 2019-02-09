using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Particle = UnityEngine.ParticleSystem.Particle;

public class ParticlesAbsorb : MonoBehaviour {

    private Vector3[] origPos;

    public IEnumerator absorbParticles(ParticleSystem collidedPS) {
		Particle[] particles = 
			new Particle[collidedPS.main.maxParticles];

        origPos = new Vector3[particles.Length];

        int numAlive = collidedPS.GetParticles(particles);
        collidedPS.SetParticles(particles, numAlive);

        for (int i = 0; i < particles.Length; i ++){
            origPos[i] = particles[i].position;
        }

        //foreach(Particle particle in particles){
        //    Debug.Log("Init particles: " + particle.position);
        //}

		float maxTime = 2f;
		float currentTime = 0f;
		while (currentTime < maxTime) {
			//print(numAlive);

			if (currentTime == 0) {
				for (int i = 0; i < numAlive; i++) {
					Vector3 parentPos = collidedPS.transform.position;
					Vector3 pVel = (transform.position - parentPos) - particles[i].position;
					particles[i].velocity = pVel.normalized * 12;
				}
				collidedPS.SetParticles(particles, numAlive);
				yield return new WaitForSeconds(0.5f);
				currentTime += Time.deltaTime;
				continue;
			}
		    for (int i = 0; i < numAlive; i++) {
				Vector3 parentPos = collidedPS.transform.position;
				particles[i].position = Vector3.Lerp( // Particles using local parent space coords
					particles[i].position, 
					transform.position - parentPos, .1f);
			}
			currentTime += Time.deltaTime;
			collidedPS.SetParticles(particles, numAlive);
			yield return 0;
		}

        Destroy(collidedPS.gameObject);
        //collidedPS.gameObject.SetActive(false);
	}

    public void particlesGoBack(ParticleSystem collidedPS){
        Particle[] particles =
            new Particle[collidedPS.main.maxParticles];
        int numAlive = collidedPS.GetParticles(particles);
        collidedPS.SetParticles(particles, numAlive);

        for (int i = 0; i < numAlive; i++)
        {
            particles[i].position = origPos[i];
        }
    }

    public IEnumerator emmitParticles(ParticleSystem collidedPS)
    {
        Particle[] particles =
            new Particle[collidedPS.main.maxParticles];
        int numAlive = collidedPS.GetParticles(particles);

        float maxTime = 2f;
        float currentTime = 0f;
        while (currentTime < maxTime)
        {
            //print(numAlive);

            if (currentTime == 0)
            {
                for (int i = 0; i < numAlive; i++)
                {
                    Vector3 parentPos = collidedPS.transform.position;
                    Vector3 pVel = (transform.position - parentPos) - particles[i].position;
                    particles[i].velocity = pVel.normalized * 12;
                }
                collidedPS.SetParticles(particles, numAlive);
                yield return new WaitForSeconds(0.5f);
                currentTime += Time.deltaTime;
                continue;
            }


            for (int i = 0; i < numAlive; i++)
            {
                Vector3 parentPos = collidedPS.transform.position;
                particles[i].position = Vector3.Lerp( // Particles using local parent space coords
                    particles[i].position, origPos[i], .1f);
            }
            currentTime += Time.deltaTime;
            collidedPS.SetParticles(particles, numAlive);
            yield return 0;
        }
        //Destroy(collidedPS.gameObject);
        //collidedPS.gameObject.SetActive(false);
    }
	
}
