// Added by Ke on 1/28/2019
// implement object pooling to reuse asteroids
// reference: https://unity3d.com/learn/tutorials/topics/scripting/object-pooling

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MeteorSpawner : MonoBehaviour {
    public GameObject spawnObject;
    private GameObject generatedObject;
    public Rigidbody2D player;
    public float velocity;

    public float initVelocityX;
    public float initVelocityY;

    public float maxX, maxY;
    public float mass, gravityScale;
    private float TimeCounter = 0;
    public float peoriod;
    public bool isSmart = false;

    public int pooledMeteorAmount = 4;
    public bool willGrow = true;
    public List<GameObject> pooledMeteors;



	// Use this for initialization
	void Start () {


        pooledMeteors = new List<GameObject>();

        for(int i=0; i<pooledMeteorAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(spawnObject);
            obj.SetActive(false);
            pooledMeteors.Add(obj);
        }

    }
	
	// Update is called once per frame
	//void Update () {
	//}

	private void Update()
	{
        if(Input.GetKeyDown(KeyCode.O)){
            SceneManager.LoadScene(0);
        }
        else if (Input.GetKeyDown(KeyCode.Q)){
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

    public GameObject GetPooledMeteors()
    {
        for(int i=0; i<pooledMeteorAmount; i++)
        {
            // reuse dead meteors
            if (!pooledMeteors[i].activeInHierarchy)
            {
                return pooledMeteors[i];
            }
        }

        //if (willGrow)
        //{
        //    GameObject obj = (GameObject)Instantiate(spawnObject);
        //    pooledMeteors.Add(obj);
        //    return obj;
        //}

        return null;
    }

    void generateSillyMeteor(bool side){
        
        //generatedObject = Instantiate(spawnObject);
        generatedObject = GetPooledMeteors();
        if (generatedObject == null) return;

        float x = Random.Range(0, maxX);
        float y = Random.Range(0, 5);
        initVelocityX = Random.Range(2, 3);
        initVelocityY = Random.Range(0, 0.5f);

        if (side)
        {
            x = -x;
        }
        else
        {
            initVelocityX = -initVelocityX;
        }

        float spawnX = maxX + x;
        float spawnY = maxY + y;

        generatedObject.transform.position = new Vector2(spawnX, spawnY);

        //Rigidbody2D rb = generatedObject.AddComponent(typeof(Rigidbody2D)) as Rigidbody2D;
        Rigidbody2D rb = generatedObject.GetComponent(typeof(Rigidbody2D)) as Rigidbody2D;

        rb.gravityScale = gravityScale;
        rb.mass = mass;

        //Debug.Log("Asteroid:" + generatedObject.transform.position);
        rb.velocity = new Vector2(initVelocityX, initVelocityY);


         generatedObject.SetActive(true);

    }


    void generateMeteor(bool side){

       
         generatedObject = Instantiate(spawnObject);

        //generatedObject = GetPooledMeteors();

        //if (generatedObject == null) return;



        float playerX = player.transform.position.x;
        float playerY = player.transform.position.y;
        float throwTime = Mathf.Abs(playerX - maxX) / velocity;
        if (!side)
        {
            maxX = -maxX;
        }
        else
        {
            velocity = -velocity;
        }

        float spawnX = maxX;
        float spawnY = playerY + 0.5f * 10 * throwTime;

        generatedObject.transform.position = new Vector2(spawnX, spawnY);


        // Rigidbody2D rb = generatedObject.AddComponent(typeof(Rigidbody2D)) as Rigidbody2D;
        Rigidbody2D rb = generatedObject.GetComponent(typeof(Rigidbody2D)) as Rigidbody2D;
        rb.gravityScale = gravityScale;
        rb.mass = mass;
        //Debug.Log("Asteroid:" + generatedObject.transform.position);
        rb.velocity = new Vector2(velocity, 0);


        //generatedObject.SetActive(true);

    }


}
