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
    
    //public float catchRadius;
    //public GameObject thePlayerOnPlanet;

    bool startedAbsorb = false;
    bool absorbed = false;

    private int origDustAmount;
    private GameObject origDust;
    private GameObject player;
    public float origSizeRatio;
    private float origPlanetSize;
    private float origDustSize;
    private Transform planetRef;

    private int index_planet = 1;//Find out the index of planet component so that the dust can be correctly resized when respawn

    double _ratio;

    // Use this for initialization
    void Start()
    {
        setup();
        //Save the state of original dust for player respawning
        origDustAmount = dustAmount;

        if(transform.childCount > 0){
            while (!transform.GetChild(index_planet).gameObject.activeSelf && index_planet < transform.childCount)
                index_planet++;
            //Debug.Log("Planet ref," + planetRef);

            if (transform.childCount > 0)
            {
                //Initilization for dust planet, saving the initial values
                planetRef = transform.GetChild(index_planet);
                origPlanetSize = planetRef.localScale.x;

                Debug.Log("About to check size");
                GameObject dust = transform.GetChild(0).gameObject;

                dust.GetComponent<DustParticles>().checkSetDustSize(this);
                origDustSize = transform.GetChild(0).localScale.x;
                origDust = copyDust(transform.GetChild(0).gameObject);
            } 
        }

    }



    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if(thePlayerOnPlanet == null)
            checkCatching();
    }


    public void Recover(){
        /*
        Todo: set up the recover for dust planet when the player is being respawned
        */
        if (!startedAbsorb || origDust == null)
            return;
        dustAmount = origDustAmount;
        float planet_size = planetRef.localScale.x;

        //Resize the new dust based on the ration between the original planet size and the current planet size
        float new_size = (planet_size / origPlanetSize) * origDustSize;

        //We assume all the planets are circle lol x=y=z
        origDust.transform.localScale = new Vector3(new_size, new_size, new_size);
        origDust.SetActive(true);
        startedAbsorb = false;

        if (transform.childCount > 0)
        {
            origDust = copyDust(transform.GetChild(0).gameObject);
        }

    }

    private GameObject copyDust(GameObject dust){
        /* Todo: 
        Clone a new dust before the current dust is absorbed for the next regeneration of the player.
        */
        GameObject new_dust = Instantiate(dust);
        new_dust.transform.lossyScale.Set(dust.transform.lossyScale.x, dust.transform.lossyScale.y, dust.transform.lossyScale.z);
        new_dust.transform.localScale.Set(dust.transform.localScale.x, dust.transform.localScale.y, dust.transform.localScale.z);
        //Debug.Log(new_dust.transform.localScale + ", " + dust.transform.lossyScale);
        new_dust.transform.position = dust.transform.position;

        //DustParticle is not needed for the next cloned dust and it will affect its regeneration
        new_dust.GetComponent<DustParticles>().enabled = false;
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
        dustAmount = 0;
        playerObj.transform.GetChild(0).GetChild(0).GetComponent<TrailRenderer>().time = sc.energy / 100f;

        return;
    }
      

    public override void playLandingSound()
    {
        //Indicate if the sound should be played so that it doesn't play repeatedly
        if (dustAmount > 0)
        {
            //print("plays harp charge");
            AudioManager.instance.PlaySFX("Harp Charge_2");   //Play the audio for absorbing dust
        }
        else
        {
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                AudioManager.instance.PlaySFX("Harp Land_" + AudioManager.sfxNormalLandID.ToString());

                AudioManager.sfxNormalLandID++;
                if (AudioManager.sfxNormalLandID > 4)
                {
                    AudioManager.sfxNormalLandID = 1;
                }
            }
        }

    }
}

