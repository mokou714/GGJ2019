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

            // play sfx and switch music if needed
            string sceneName = SceneManager.GetActiveScene().name;
            AudioManager.instance.PlayLevelFinishSFX(sceneName);
            AudioManager.instance.SwitchMusic(sceneName);

            if (collision.gameObject.tag == "Finish"){
                int curLevel = 0;
                string next_level = "";
                if (sceneName == "2-start"){
                    curLevel = 11;
                    next_level = "12";
                }else{
                    curLevel = int.Parse(sceneName);
                    print("cur level:" + curLevel);
                    if (curLevel == -1)
                    {
                        next_level = "1";
                        curLevel = 0;
                        if(GameStates.instance.isLoggedIn){
                            SocialSystem.instance.setAchievement(Achievements.achievement_passed_tutorial);
                            SocialSystem.instance.setAchievement(Achievements.unlock_milkyway);
                        }

                    }else if(curLevel == 20){
                        if (GameStates.instance.isLoggedIn){
                            SocialSystem.instance.setAchievement(Achievements.passing_all_levels);
                        }
                        next_level = "start page";
                    }else if((curLevel > 0 && curLevel < 20) || curLevel < -1){
                        if(curLevel == 9){
                            if ((int)GameStates.instance.getData(Constants.whirpoolStatus, typeof(int)) < 1)
                                next_level = "2-start";
                            else
                                next_level = "10";
                        }else{
                            next_level = (curLevel + 1).ToString();
                        }
                        if(curLevel > 0 && curLevel <= 20){
                            sc.playerModel.energy = sc.energy;
                            saveUserData(curLevel);
                            if (sc.wonAward.Length > 0)
                            {
                                sc.playerModel.wonAward = sc.wonAward;
                                GameStates.instance.saveData(sc.wonAward, 1);
                                print("saved: " + sc.wonAward);
                            }

                        }
                    }
                }
                                

                GameStates.instance.SaveLevel(curLevel + 1);
                if(next_level.Length > 0)
                    StartCoroutine(waitToNext(1f, false, next_level));
            }else if (collision.gameObject.tag == "preTutorial"){
                StartCoroutine(waitToNext(4f, false, "start page"));
                GameStates.instance.SaveTutorialData(1,"pre");
            }else if (collision.gameObject.tag == "compaign"){
                //Debug.Log("load");
                StartCoroutine(waitToNext(1f, true));
            }
        }
        return;
    }

    


    public void saveUserData(int curlevel){
        GameStates.instance.globalContinuousJump += sc.playerModel.continousJump;
        GameStates.instance.globalContinuousJumpMax = Mathf.Max(GameStates.instance.globalContinuousJump, GameStates.instance.globalContinuousJumpMax);

        int curMaxJump = (int)GameStates.instance.getData(Constants.maxConstJumpKey, typeof(int));
        curMaxJump = Mathf.Max(curMaxJump, GameStates.instance.globalContinuousJumpMax);

        GameStates.instance.saveData(Constants.maxConstJumpKey, curMaxJump);

        //print("scores:" + scores);
        //print("Cur level: " + curlevel);

        float cal_score = sc.playerModel.calScore();
        //print(score_list.Length + ", " + scores);
        string level = Constants.bestMilkywayScoreKey;
        string scores_key = Constants.getScoreKeyMilkeyWay;

        if ((11 <= curlevel && curlevel <= 20) || SceneManager.GetActiveScene().name == "2-start")
        {
            level = Constants.bestWhirlpoolScoreKey;
            scores_key = Constants.getScoreKeyWhirlpool;
        }

        string scores = (string)GameStates.instance.getData(scores_key, typeof(string));
        string[] score_list = scores.Split(',');
        string printscore = "";
        curlevel = (curlevel - 1) % 10;
        print("curlevel:" + curlevel);

        if(curlevel < score_list.Length){
            if (score_list[curlevel].Length > 0)
            {
                int res = 0;
                int.TryParse(score_list[curlevel], out res);
                print("parse res:" + res);
                score_list[curlevel] = Mathf.Max(res, cal_score).ToString();
                //print(score_list[curlevel] + "," + cal_score);
            }
            else
            {
                score_list[curlevel] = cal_score.ToString();
            }
        }else{
            scores += ("," + cal_score.ToString());
            score_list = scores.Split(',');
        }

        for (int j = 0; j < score_list.Length; j++)
            printscore += (score_list[j] + ",");

        print("curlevel:" + curlevel + " score:" + printscore);

        float best_score = (float)GameStates.instance.getData(level, typeof(float));


        int total = 0;
        for (int i = 0; i < score_list.Length; i++)
        {
            if(score_list[i].Length > 0){
                total += int.Parse(score_list[i]);
            }

        }

        //if (total > best_score)
        //{
        //    SocialSystem.instance.setLeaderBoard(Constants.bestMilkywayScoreKey, (long)total);
        //}
        GameStates.instance.saveData(Constants.bestMilkywayScoreKey, Mathf.Max(total, best_score));
        GameStates.instance.saveData(scores_key, string.Join(",", score_list));
        GameStates.instance.showContent = Mathf.Max(total, best_score).ToString();

        //Debug.Log("Last continous jump: " + GameStates.instance.globalContinuousJump);
        //Debug.Log("Current max jumps: " + curMaxJump);
    }


    IEnumerator waitToNext(float time, bool load, string id = "")
    {
        /*
        Todo: this coroutine to show instruction after the player lands on the second planet
        */

        yield return new WaitForSeconds(time);
        if(id.Length > 0)
            SceneManager.LoadScene(id);
        else
            GameStates.instance.LoadLevel();
    }

    //private void startNewLevel(int nextLevelID){

    //    }
    //}

    IEnumerator playerBlink() {
        playerTrailRenderer.enabled = false;
        yield return new WaitForSeconds(0.05f);
        playerTrailRenderer.enabled = true;
    }



}
