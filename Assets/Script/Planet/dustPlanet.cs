using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class dustPlanet : Planet
{
    /*
    This class is attached on planet units, responsible for attracting the player, audio playing when orbiting starts
    */

    public int dustAmount;
    
    public float catchRadius;
    //public GameObject thePlayerOnPlanet;

    bool startedAbsorb = false;
    bool absorbed = false;

    private int origDustAmount;
    private GameObject origDust;
    private GameObject player;
    //public bool canPlaySound = true;

    // Use this for initialization
    void Start()
    {
        //Debug.Log(tag);
        //Save the state of original dust for player respawning
        origDustAmount = dustAmount;
        //Debug.Log(transform.GetChild(0).lossyScale);
        if (transform.childCount > 0)
        {
            origDust = copyDust(transform.GetChild(0).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        checkCatching(catchRadius, dustAmount);

    }


    public void Recover(){
        /*
        Todo: set up the recover for dust planet when the player is being respawned
        */
        if (!startedAbsorb)
            return;
        //Debug.Log("Recover, " + startedAbsorb);
        dustAmount = origDustAmount;
        origDust.SetActive(true);
        startedAbsorb = false;
        if (transform.childCount > 0)
        {
            //origDust = transform.GetChild(0);
            origDust = copyDust(transform.GetChild(0).gameObject);
        }
    }

    private GameObject copyDust(GameObject dust){
        /* Todo: 
        Clone a new dust before the current dust is absorbed for the next regeneration of the player.
        */
        //Debug.Log(dust + "," + dust.transform.lossyScale);

        GameObject new_dust = Instantiate(dust);
        new_dust.transform.position = dust.transform.position;
        //Debug.Log(new_dust.transform.localScale + ", " + dust.transform.lossyScale);
        new_dust.transform.localScale = dust.transform.lossyScale;
        new_dust.transform.SetParent(transform);
        new_dust.SetActive(false);
        return new_dust;
    }

    public override void catchedAction(spacecraft sc){

            //enegy
        sc.energy += dustAmount;
        //Debug.Log("absorbing");

        //dust absorbed
        ParticlesAbsorb pA;

        if (!startedAbsorb && (pA = playerObj.GetComponent<ParticlesAbsorb>()) != null && (tag == "dustPlanet") ){
            StartCoroutine(pA.absorbParticles(transform.GetChild(0).GetComponent<ParticleSystem>()));

            transform.GetChild(transform.childCount - 1).SetSiblingIndex(0);
            startedAbsorb = true;
        }
        // change
        if (sc.energy > 100f)
            sc.energy = 100f;
        dustAmount = 0;playerObj.transform.GetChild(0).GetChild(0).GetComponent<TrailRenderer>().time = sc.energy / 100f;

        return;
    }

}

