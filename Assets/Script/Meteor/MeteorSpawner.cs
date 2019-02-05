using UnityEngine.SceneManagement;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour {
    /*
    This class controls the dynamic spawning of meteors
    */
    public GameObject spawnObject;
    private GameObject generatedMeteor;
    public Rigidbody2D player;
    public float velocity;
    public float mass, gravityScale;
    private float timeCounter = 0;
    public float peoriod;
    //public bool isSmart = false;

    enum Side { left, right };

	// Use this for initialization
	void Start () {
        
	}
	
	void Update() {
        
        if(Input.GetKeyDown(KeyCode.O)){
            //Reload the scene
            SceneManager.LoadScene(0);
        }else if (Input.GetKeyDown(KeyCode.Q)){
            //Quit the game
            Application.Quit();
        }

        float curTime = Time.time;
        float diff = curTime - timeCounter;
        //Debug.Log("Time diff:" + diff);
        if(diff >= peoriod){
            timeCounter = curTime;

            //Randomly select side on which a meteor is generated
            int random_side = Random.Range(0, 2);
            //Randomly determine if the meteor is smart
            int is_smart = Random.Range(0, 2);
            if (random_side == 1)
                genMeteor(Side.right, is_smart);
            else
                genMeteor(Side.left, is_smart);

        }
	}

    private void genMeteor(Side side, int isSmart){
        /*
         Todo: Generate Meteors that goes toward the player
        */
        float max_x = Constants.maxX;
        float local_velocity = velocity;
        //If meteor is spawned on left, the maxX should be left side which is negative, if it is spawned on the right, velocity should point to left, which is negative
        if(side == Side.left){
            max_x = -max_x;
        }else{
            local_velocity = -local_velocity;
        }

        float spawnX, spawnY;

        //Determine if the meteor is smart
        if(isSmart == 1){
            if (player == null)
                return;
            //Make the meteor be thrown toward player's position
            float playerX = player.transform.position.x;
            float playerY = player.transform.position.y;
            float throwTime = Mathf.Abs(playerX - max_x) / velocity;
            spawnX = max_x;
            spawnY = playerY + 0.5f * 5 * Mathf.Pow(throwTime, 2);
            //Debug.Log(side + " Spawned Position:" + spawnX + "," + spawnY + "," + local_velocity);

        }else{
            //Make the meteor be spawned at random position
            float x;
            if(max_x > 0)
                x = Random.Range(0, max_x);
            else
                x = Random.Range(max_x, 0);
            float y = Random.Range(Constants.maxY, Constants.maxY + 20);
            spawnX = Constants.maxX + x;
            spawnY = Constants.maxY + y;
            //Debug.Log(side + " Spawned Position:" + spawnX + "," + spawnY + "," + velocity);
        }

        shootMeteor(spawnX, spawnY, local_velocity);
    }

    private void shootMeteor(float spawnX, float spawnY, float v){
        
        //Generate a new instance of meteor and set up its properties
        generatedMeteor = Instantiate(spawnObject);
        generatedMeteor.transform.position = new Vector2(spawnX, spawnY);
        Rigidbody2D rb = generatedMeteor.AddComponent(typeof(Rigidbody2D)) as Rigidbody2D;

        rb.gravityScale = gravityScale;
        rb.mass = mass;
        //Debug.Log("Asteroid:" + generatedObject.transform.position);
        rb.velocity = new Vector2(v, 0);
    }


}
