using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Collision : MonoBehaviour
{

    public float collide_strengh;
    public bool collided;
    public float asteroidDamage;
    public UIEffect UIeffect;

    public ParticleSystem energyLossOnCollide;
    public ParticleSystem burstEnergy;
    public TrailRenderer playerTrailRenderer;
    private spacecraft sc;

    // Use this for initialization
    void Start()
    {
        playerTrailRenderer = transform.GetChild(0).GetChild(0).GetComponent<TrailRenderer>();
        sc = transform.GetChild(0).GetComponent<spacecraft>();
    }

    // Update is called once per frame
    void Update()
    {


    }

    private void LateUpdate()
    {
        if (collided)
        {
            gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0;
            collided = false;

            if (energyLossOnCollide)
                energyLossOnCollide.Play();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        float damage = 0;
        string hit_obj = col.gameObject.tag;

        switch (hit_obj)
        {
            case "asteroid":
                damage = col.gameObject.GetComponent<Asteroid>().damage;
                break;
            case "orbasteroid":
                damage = col.gameObject.GetComponent<orbitAsteroid>().damage;
                break;
            default:
                return;
        }

        if(col.transform.childCount > 1 && col.transform.GetChild(col.transform.childCount - 1).name == "secret"){
            if(BadgeManager.instance != null)
                BadgeManager.instance.showHiddenPlanet(col.transform.GetChild(col.transform.childCount - 1).tag);
        }

        col.gameObject.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity * collide_strengh;
        float left_health = sc.energy - damage;
        if (left_health <= 0)
        {
            //playerTrailRenderer.enabled = false;
            burstEnergy.Play();
        }

        sc.playerModel.hit += 1;
        sc.energy = left_health;
        playerTrailRenderer.time = sc.energy / 100f; 
        collided = true;
        AudioManager.instance.PlaySFX("being hit");
    }

    IEnumerator stopBurst(){
        yield return new WaitForSeconds(0.2f);
        burstEnergy.Stop();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Tag: " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Finish" || collision.gameObject.tag == "preTutorial" || collision.gameObject.tag == "compaign")
        {
            if (sc.dead)
                return;
            sc.won = true;
            AudioManager.instance.PlaySFX("Next Level");
            sc.requiredSleep = true;
            sc.requiredFreeze = true;
            playerTrailRenderer.time = 0.5f;
            GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
            sc.requiredSleep = true;

            if (collision.gameObject.tag == "Finish")
            {
                if (SceneManager.GetActiveScene().buildIndex == 4)
                {
                    SocialSystem.instance.setAchievement(Achievements.achievement_passed_tutorial);
                    SocialSystem.instance.setAchievement(Achievements.unlock_whirlpool);
                }else if (SceneManager.GetActiveScene().buildIndex == 14)
                {
                    SocialSystem.instance.setAchievement(Achievements.unlock_whirlpool);
                }

                if(sc.wonAward.Length > 0){
                    sc.playerModel.wonAward = sc.wonAward;
                    GameStates.instance.saveData(sc.wonAward, 1);
                }

                sc.playerModel.energy = sc.energy;
                //Debug.Log("SC won: " + sc.wonAward);

                //Debug.Log("Total score: " + sc.playerModel.calScore());
                //Save progress in case user kill the app in the background
                saveUserData();
                GameStates.instance.SaveLevel();

                if (SceneManager.GetActiveScene().name == "2-start"){
                    StartCoroutine(waitToNext(1f, false, id: SceneManager.GetActiveScene().buildIndex + 2));
                }else{
                    StartCoroutine(waitToNext(1f, false));
                }
            }else if (collision.gameObject.tag == "preTutorial"){
                StartCoroutine(waitToNext(4f, false));
                GameStates.instance.SaveTutorialData(1,"pre");
            }else if (collision.gameObject.tag == "compaign"){
                Debug.Log("load");
                StartCoroutine(waitToNext(1f, true));
            }

        }
        return;
    }


    private void saveUserData(){
        GameStates.instance.globalContinuousJump += sc.playerModel.continousJump;
        GameStates.instance.globalContinuousJumpMax = Mathf.Max(GameStates.instance.globalContinuousJump, GameStates.instance.globalContinuousJumpMax);

        int curMaxJump = (int)GameStates.instance.getData(Constants.maxConstJumpKey, typeof(int));
        curMaxJump = Mathf.Max(curMaxJump, GameStates.instance.globalContinuousJumpMax);

        GameStates.instance.saveData(Constants.maxConstJumpKey, curMaxJump);
        Debug.Log("Last continous jump: " + GameStates.instance.globalContinuousJump);
        Debug.Log("Current max jumps: " + curMaxJump);
    }


    IEnumerator waitToNext(float time, bool load, int id = -1)
    {
        /*
        Todo: this coroutine to show instruction after the player lands on the second planet
        */

        yield return new WaitForSeconds(time);
        if(id > 0)
            startNewLevel(id);
        else if (!load)
            startNewLevel(SceneManager.GetActiveScene().buildIndex + 1);
        else
            GameStates.instance.LoadLevel();
    }

    private void startNewLevel(int nextLevelID){
        int cur_scene = SceneManager.GetActiveScene().buildIndex;
        if (cur_scene == SceneManager.sceneCount - 2)
            SceneManager.LoadScene("end stage");
        else{
            //int nextLevelID = SceneManager.GetActiveScene().buildIndex + 1;
            // load next level
            SceneManager.LoadScene(nextLevelID);
        }
    }

    IEnumerator playerBlink() {
        playerTrailRenderer.enabled = false;
        yield return new WaitForSeconds(0.05f);
        playerTrailRenderer.enabled = true;
    }




}
