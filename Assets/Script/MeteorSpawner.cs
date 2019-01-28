using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour {
    public GameObject spawnObject;
    private GameObject generatedObject;
    public Rigidbody2D player;
    public float velocity;
    public float maxX, maxY;
    public float mass, gravityScale;
    private float TimeCounter = 0;
    public float peoriod;
    public bool isSmart = false;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	//void Update () {
	//}

	private void Update()
	{
        if(Input.GetKeyDown(KeyCode.O)){
            Application.LoadLevel(0);
        }else if (Input.GetKeyDown(KeyCode.Q)){
            Application.Quit();
        }

        float curTime = Time.time;
        float diff = curTime - TimeCounter;
        //Debug.Log("Time diff:" + diff);
        if(diff >= peoriod){
            
            TimeCounter = curTime;
            //int n = Random.Range(0, 1);
            if (!isSmart)
            {
                generateSillyMeteor(true);
                generateSillyMeteor(false);
            }else
            {
                generateMeteor(true);
                generateMeteor(false);
            }
        }
	}

    void generateSillyMeteor(bool side){
        float x = Random.Range(0, maxX);
        float y = Random.Range(0, 5);
        if (side){
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


	void generateMeteor(bool side){

        float playerX = player.transform.position.x;
        float playerY = player.transform.position.y;
        float throwTime = Mathf.Abs(playerX - maxX)/velocity;
        if(!side){
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
