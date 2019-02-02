using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour {
    public GameObject spawnObject;
    private GameObject generatedObject;
    public Rigidbody2D player;
    public float velocity;
    public float maxX, maxY;
    public float mass, gravityScale;
    private float timeCounter = 0;
    public float peoriod;
    public bool isSmart = false;

    enum Side { left, right };

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	//void Update () {
	//}

	private void Update()
	{
        if(Input.GetKeyDown(KeyCode.O)){
            SceneManager.LoadScene(0);
        }else if (Input.GetKeyDown(KeyCode.Q)){
            Application.Quit();
        }

        float curTime = Time.time;
        float diff = curTime - timeCounter;
        //Debug.Log("Time diff:" + diff);
        if(diff >= peoriod){
            
            timeCounter = curTime;
            //int n = Random.Range(0, 1);
            if (!isSmart)
            {
                generateSillyMeteor(Side.left);
                generateSillyMeteor(Side.right);
            }else
            {
                generateSmartMeteor(Side.left);
                generateSmartMeteor(Side.right);
            }
        }
	}

    void generateSillyMeteor(Side side){
        /*
         Todo: Generate Meteors that does not go toward the player
        */
        float x = Random.Range(0, maxX);
        float y = Random.Range(0, maxY);

        //If meteor is spawned on left, the maxX should be left side which is negative, if it is spawned on the right, velocity should point to left, which is negative
        if (side == Side.left){
            x = -x;
        }else{
            velocity = -velocity;
        }
            
        float spawnX = maxX + x;
        float spawnY = maxY + y;

        generatedObject = Instantiate(spawnObject);
        generatedObject.transform.position = new Vector2(spawnX, spawnY);

        Rigidbody2D rb = generatedObject.AddComponent(typeof(Rigidbody2D)) as Rigidbody2D;

        rb.gravityScale = gravityScale;
        rb.mass = mass;

        //Debug.Log("Asteroid:" + generatedObject.transform.position);
        rb.velocity = new Vector2(velocity, 0);
    }


    void generateSmartMeteor(Side side){
        /*
         Todo: Generate Meteors that goes toward the player
        */
        float playerX = player.transform.position.x;
        float playerY = player.transform.position.y;
        float throwTime = Mathf.Abs(playerX - maxX)/velocity;

        //If meteor is spawned on left, the maxX should be left side which is negative, if it is spawned on the right, velocity should point to left, which is negative
        if(side == Side.left){
            maxX = -maxX;
        }else{
            velocity = -velocity;
        }
            
        float spawnX = maxX;
        float spawnY = playerY + 0.5f * 10 * throwTime;

        generatedObject = Instantiate(spawnObject);
        generatedObject.transform.position = new Vector2(spawnX, spawnY);
        Rigidbody2D rb = generatedObject.AddComponent(typeof(Rigidbody2D)) as Rigidbody2D;

        rb.gravityScale = gravityScale;
        rb.mass = mass;
        //Debug.Log("Asteroid:" + generatedObject.transform.position);
        rb.velocity = new Vector2(velocity, 0);

    }


}
