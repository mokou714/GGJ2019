using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Particle = UnityEngine.ParticleSystem.Particle;

public class absorbPlanet : Planet
{
    /*
    This class is attached on planet units, responsible for attracting the player, audio playing when orbiting starts
    */

    public float aborbingSpeed;
    public float rotatingSpeed;
    private bool absorbing;
    private bool startAbsorbing;
    private float playerRotationSpeed;

    Particle[] particles;
    int numAlive;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (thePlayerOnPlanet == null)
            checkCatching();
        else
        {
            checkAbsorbing();
        }
    }

    void checkAbsorbing()
    {

        if (absorbing)
        {

            particles = new Particle[thePlayerOnPlanet.transform.GetChild(1).GetComponent<ParticleSystem>().main.maxParticles];
            numAlive = thePlayerOnPlanet.transform.GetChild(1).GetComponent<ParticleSystem>().GetParticles(particles);

            List<Particle> newParticlesList = new List<Particle>();

            var pshape = thePlayerOnPlanet.transform.GetChild(1).GetComponent<ParticleSystem>().shape;
            pshape.radius = 0.4f;

            for (int i = 0; i < numAlive; i++)
            {
                particles[i].position = Vector3.Lerp(
                    particles[i].position,
                    transform.position - thePlayerOnPlanet.transform.position,
                    0.1f
                );

                //if particle has reach the des, clear it
                if (Vector3.Distance(particles[i].position, transform.position - thePlayerOnPlanet.transform.position) > 0.5f)
                {
                    newParticlesList.Add(particles[i]);
                }
            }
            thePlayerOnPlanet.transform.GetChild(1).GetComponent<ParticleSystem>().Clear();
            thePlayerOnPlanet.transform.GetChild(1).GetComponent<ParticleSystem>().SetParticles(newParticlesList.ToArray(), newParticlesList.Count);

            thePlayerOnPlanet.transform.GetChild(0).GetComponent<spacecraft>().energy -= Time.deltaTime * aborbingSpeed;

        }


    }

    public override void catchedAction(spacecraft sc)
    {
        absorbing = true;
        if(rotatingSpeed != 0){
            playerRotationSpeed = sc.rotating_speed;
            sc.rotating_speed = rotatingSpeed;
        }
    }

    public override void playerLeave()
    {
        thePlayerOnPlanet.transform.GetChild(1).GetComponent<ParticleSystem>().Clear();
        thePlayerOnPlanet.transform.GetChild(0).GetComponent<spacecraft>().rotating_speed = playerRotationSpeed;
        playerRotationSpeed = 0;
        var pshape = thePlayerOnPlanet.transform.GetChild(1).GetComponent<ParticleSystem>().shape;
        pshape.radius = 0.0001f;
        absorbing = false;
        
        base.playerLeave();

    }


}


