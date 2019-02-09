using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class dustPlanet : MonoBehaviour
{
    /*
    This class is attached on planet units, responsible for attracting the player, audio playing when orbiting starts
    */

    public int dustAmount;
    
    public float catchRadius;
    public GameObject thePlayerOnPlanet;

    bool startedAbsorb = false;

    private int origDustAmount;
    private GameObject origDust;
    private GameObject player;
    private bool absorbed = false;
    // Use this for initialization
    void Start()
    {
        origDustAmount = dustAmount;

        if (transform.childCount > 0)
        {
            //origDust = transform.GetChild(0);
            origDust = copyDust(transform.GetChild(0).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        checkCatching();
    }

    void checkCatching()
    {
        //Keep scanning around itself to find if player is around
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, catchRadius);
        int i = 0;

        //if player leaves the planet, delete the reference
        if(thePlayerOnPlanet != null) {
            if (thePlayerOnPlanet.transform.GetChild(0).GetComponent<spacecraft>().moving == true)
                thePlayerOnPlanet = null;
        }

        while (i < hitColliders.Length)
        {
            GameObject ob = hitColliders[i].gameObject;
            //player catched
            if (ob != gameObject && ob.tag == "Player")
            {
                //if (player == null){
                //    player = ob;
                //    Debug.Log("Catch player " + player);
                //}
                //store player reference
                thePlayerOnPlanet = ob;

                spacecraft sc = ob.transform.GetChild(0).GetComponent<spacecraft>();

                //Debug.Log("rotating planet: " + sc.rotating_planet);

                //check if spacecraft is not orbiting the same planet after launch
                if(sc.rotating_planet == null || sc.rotating_planet != gameObject)
                {
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

                    //enegy
                    sc.energy += dustAmount;

                    //dust absorbed
                    ParticlesAbsorb pA;
                    if (!startedAbsorb && (pA = ob.GetComponent<ParticlesAbsorb>()) != null && tag == "dustPlanet")
                    {
                        StartCoroutine(pA.absorbParticles(transform.GetChild(0).GetComponent<ParticleSystem>()));

                        transform.GetChild(transform.childCount - 1).SetSiblingIndex(0);
                        startedAbsorb = true;
                    }
                                                           
                    //landing sound  //comment for debug

                    //if (dustAmount > 0)
                    //{
                    //    AudioManager.instance.PlaySFX("Harp Charge_2");   //Play the audio for absorbing dust
                    //}
                    //else
                    //{
                    //    if (SceneManager.GetActiveScene().buildIndex != 0)
                    //    {
                    //        //print("sfxNormalLand id: " + AudioManager.sfxNormalLandID);
                    //        AudioManager.instance.PlaySFX("Harp Land_" + AudioManager.sfxNormalLandID.ToString());

                    //        AudioManager.sfxNormalLandID++;
                    //        if (AudioManager.sfxNormalLandID > 4)
                    //        {
                    //            AudioManager.sfxNormalLandID = 1;
                    //        }
                    //    }
                    //}

                    // change
                    if (sc.energy > 100f)
                        sc.energy = 100f;
                    dustAmount = 0;
                    ob.transform.GetChild(0).GetChild(0).GetComponent<TrailRenderer>().time = sc.energy / 100f;
                }
                                                             
            }
            ++i;

        }
    }

    public void Recover(){
        if (!startedAbsorb)
            return;
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
        GameObject new_dust = Instantiate(dust);
        new_dust.transform.position = dust.transform.position;
        //Debug.Log(new_dust.transform.localScale + ", " + dust.transform.lossyScale);
        new_dust.transform.localScale = dust.transform.lossyScale;
        new_dust.transform.SetParent(transform);
        new_dust.SetActive(false);
        return new_dust;
    }

    private void emmitDust(){
        
    }

}

