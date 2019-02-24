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
    public Vector3 origSizeRatio;

    private int index_planet = 1;//Find out the index of planet component so that the dust can be correctly resized when respawn

    // Use this for initialization
    void Start()
    {
        //Save the state of original dust for player respawning
        origDustAmount = dustAmount;
        if(transform.childCount > 0){
            while (!transform.GetChild(index_planet).gameObject.activeSelf && index_planet < transform.childCount)
                index_planet++;
            //Debug.Log("Planet ref," + planetRef);

            if (transform.childCount > 0)
            {
                Vector3 planet_scale = transform.GetChild(index_planet).localScale;
                GameObject dust = transform.GetChild(0).gameObject;
                origSizeRatio = new Vector3(dust.transform.localScale.x / planet_scale.x, dust.transform.localScale.y / planet_scale.y, dust.transform.localScale.z / planet_scale.z);
                origDust = copyDust(transform.GetChild(0).gameObject);
                Debug.Log(name + " dust scale: " + origDust.transform.localScale + ", planet scale" + transform.GetChild(index_planet).localScale);

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
        if (!startedAbsorb)
            return;
        

        //Debug.Log("Recover, " + startedAbsorb);
        dustAmount = origDustAmount;
        Debug.Log(name + " dust scale ratio: " + origSizeRatio);
        Debug.Log(name + " dust scale: " + origDust.transform.localScale + ", planet scale" + transform.GetChild(index_planet).localScale);
        origDust.SetActive(true);
        origDust.transform.localScale = new Vector3(origSizeRatio.x * transform.GetChild(index_planet).localScale.x, 
                                                    origSizeRatio.y * transform.GetChild(index_planet).localScale.y, 
                                                    origSizeRatio.z * transform.GetChild(index_planet).localScale.z);
        startedAbsorb = false;

        if (GetComponent<pulse>() != null && GetComponent<pulse>().isActiveAndEnabled)
        {
            GetComponent<pulse>().time = 1f;
            GetComponent<pulse>().scaleFactor = 1f;
            StartCoroutine(pulseBackSpeed());
        }


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
        Debug.Log("Orig Size Ratio: " + origSizeRatio);
        new_dust.transform.SetParent(transform);
        //new_dust.SetActive(false);
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

    IEnumerator pulseBackSpeed(){
        yield return new WaitForSeconds(1f);
        GetComponent<pulse>().time = 0.01f;
    }

}

