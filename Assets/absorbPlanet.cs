using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Particle = UnityEngine.ParticleSystem.Particle;

public class absorbPlanet : MonoBehaviour
{
    /*
    This class is attached on planet units, responsible for attracting the player, audio playing when orbiting starts
    */


    public float catchRadius;
    public float aborbingSpeed;
    private bool absorbing = false;
    private GameObject player;
    private bool startAbsorbing = false;

    Particle[] particles;
    int numAlive;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        checkCatching();
        checkAbsorbing();
    }

    void checkCatching()
    {
        //Keep scanning around itself to find if player is around
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, catchRadius);
        int i = 0;

        while (i < hitColliders.Length)
        {
            GameObject ob = hitColliders[i].gameObject;
            //player catched
            if (ob != gameObject && ob.tag == "Player")
            {
                spacecraft sc = ob.transform.GetChild(0).GetComponent<spacecraft>();

                //Debug.Log("rotating planet: " + sc.rotating_planet);

                //check if spacecraft is not orbiting the same planet after launch
                if (sc.rotating_planet == null || sc.rotating_planet != gameObject)
                {
                    //store player reference
                    player = ob;
                    absorbing = true;
                    startAbsorbing = true;

                    //rotate
                    sc.rotating_planet = gameObject;
                    sc.rotation_center = transform.position;
                    sc.rotate_on = true;
                    sc.moving = false;
                    sc.movingStart = false;

                    Vector2 v1 = new Vector2(transform.position.x - sc.transform.position.x,
                                             transform.position.y - sc.transform.position.y);
                    //v1.Normalize();
                    Vector2 v2 = sc.transform.parent.GetComponent<Rigidbody2D>().velocity;
                    //v2.Normalize();

                    float angle = Vector2.SignedAngle(v1, v2);

                    if (angle < 0f && angle < 90f)
                        sc.rotating_dir = -1; //counterclockwise rotation
                    else
                        sc.rotating_dir = 1; //clockwise rotation 
                       
                }

            }
            ++i;

        }
    }

    void checkAbsorbing()
    {

        if (absorbing) {

            particles = new Particle[player.transform.GetChild(1).GetComponent<ParticleSystem>().main.maxParticles];
            numAlive = player.transform.GetChild(1).GetComponent<ParticleSystem>().GetParticles(particles);

            List<Particle> newParticlesList = new List<Particle>();

            Debug.Log("absorbing");


            var pshape = player.transform.GetChild(1).GetComponent<ParticleSystem>().shape;
            pshape.radius = 0.4f;



            for (int i = 0; i < numAlive; i++) {
                particles[i].position = Vector3.Lerp(
                    particles[i].position,
                    transform.position - player.transform.position,
                    0.1f
                );

                //if particle has reach the des, clear it
                if (Vector3.Distance(particles[i].position, transform.position - player.transform.position) > 0.5f)
                {
                    newParticlesList.Add(particles[i]);
                }
            }


            player.transform.GetChild(1).GetComponent<ParticleSystem>().Clear();
            player.transform.GetChild(1).GetComponent<ParticleSystem>().SetParticles(newParticlesList.ToArray(), newParticlesList.Count);

            //stop absorbing when player leaves
            if(player.transform.GetChild(0).GetComponent<spacecraft>().moving == true) {
                absorbing = false;
                player.transform.GetChild(1).GetComponent<ParticleSystem>().Clear();
                pshape.radius = 0.0001f;

                player = null;
            }

            //player.transform.GetChild(0).GetComponent<spacecraft>().energy -= Time.deltaTime * aborbingSpeed;

        }


    }



    public void Recover()
    {
        //transform.GetChild(0)
    }

}


