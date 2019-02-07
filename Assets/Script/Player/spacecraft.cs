using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class spacecraft : MonoBehaviour {
    /*
    This is the script executed in the player, responsible for player control and basic behaviors
    */

    public float direction;
    public int rotation_sensitivity;
    public float launch_speed;
    public float energy;

    public bool moving;

    public float start_velocity;
    public GameObject end;


    public float rotating_speed;
    public bool rotate_on;
    public Vector3 rotation_center;
    public int rotating_dir;

    public float egdecSpeed;
    public GameObject rotating_planet;

    private float movingTime = 0;
    public bool movingStart;

    public float inwardVel;
    private float originalWidth;

    private ParticleSystem energyLoss;
    private Vector3 spawnPoint;

    public GameObject Player;

    private float currTime = 0;
    private float startInSeconds = 1.5f;

    private bool hide = false;
    private bool startHide = false;
    private bool changedBack = true;
    private Rigidbody2D parentRigidBody;

    private float speedThreshold = 3.5f;

    private float curMovingTime = 0;

	// Use this for initialization
	void Start () {
        energy = 100f;
        spawnPoint = transform.position;
        Vector3 dir = new Vector3(1, 0, 0);
        transform.parent.GetComponent<Rigidbody2D>().velocity = dir * start_velocity;
        originalWidth = transform.GetChild(0).gameObject.GetComponent<TrailRenderer>().widthMultiplier;
        energyLoss = transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>();
        parentRigidBody = transform.parent.GetComponent<Rigidbody2D>();
    }

    private void ReinitPlayer(){
        /*
        Todo: this function reinitialize parameters of player when restarting the current level
        */

        transform.gameObject.SetActive(true);
        transform.parent.GetComponent<BoxCollider2D>().enabled = true;
        energy = 100f;
        transform.GetChild(0).gameObject.GetComponent<TrailRenderer>().time = energy / 100f;
        transform.GetChild(0).gameObject.GetComponent<TrailRenderer>().widthMultiplier = originalWidth;
        movingStart = true;
        Vector3 dir = new Vector3(1, 0, 0);
        transform.parent.GetComponent<Rigidbody2D>().velocity = dir * start_velocity;
        moving = false;
        movingTime = 0;
        energyLoss = transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>();
        rotating_planet = null;
        //Debug.Log("Reinitialized player: " + transform.position);
    }
	
    private void ReinitScene(){
        /*
        Todo: this function reinitialize the scene's other objects, pulling some of them back to their original positions
        */
        object[] scene_obj = FindObjectsOfType(typeof(GameObject));
        foreach (object obj in scene_obj){
            //Iterate through all the gameobject in this scene
            GameObject single_obj = (GameObject)obj;
            string obj_tag = single_obj.tag;

            switch(obj_tag){
                case "begin":
                    single_obj.gameObject.GetComponent<StopParticles>().ParticleReStart();
                    break;
                case "asteroid":
                    single_obj.gameObject.GetComponent<Asteroid>().movingBack =  true;
                    break;
                case "orbasteroid":
                    //For orbit asteroid, if the asteroid is already out of its orbit, it should go back when the scene restarts
                    orbitAsteroid orb_obj = single_obj.gameObject.GetComponent<orbitAsteroid>();
                    if(Vector3.Distance(single_obj.transform.position, orb_obj.orbitCenter.position) > (orb_obj.radius + 1f)){
                        single_obj.gameObject.GetComponent<orbitAsteroid>().movingBack = true;
                    }
                    break;
            }


        }
    }

	// Update is called once per frame
	void Update () {
        
        //Check if the player should be visible or not
        //modVisibility();

        //Set up the original width of player
        transform.GetChild(0).gameObject.GetComponent<TrailRenderer>().widthMultiplier = originalWidth * energy / 100f;

        if (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount == 1))
        {
            //Derail when the input is detected and player is in an orbit
            if (rotate_on)
                Launch();
        }else if(Input.GetKeyDown(KeyCode.R)|| (Input.touchCount == 2)){
            Application.LoadLevel(Application.loadedLevel);
        }else if(rotate_on){
            //If no valid input, keep rotating around the current planet
            Rotate();
        }
        //Indicating wether the player is in orbit or flying straight
        if(moving){
            curMovingTime += Time.deltaTime;
            //Detecting if the previous moment is orbiting
            if (!movingStart)
            {
                //Debug.Log("Energy starts Losing");
                movingStart = true;
                movingTime = curMovingTime;
                //Debug.Log("start recording :" + movingTime);
                energyLoss.Play();
            }else{
                float timeDuration = curMovingTime - movingTime;
                //Debug.Log("Velocity: " + parentRigidBody.velocity.magnitude + ", " + curMovingTime + ", " + movingTime);
                //if(parentRigidBody.velocity.magnitude < speedThreshold && timeDuration > 0.001f){
                //    egdecSpeed = ((curTime - movingTime) / parentRigidBody.velocity.magnitude) * 2500;
                //}
                float offset = ((curMovingTime - movingTime) * transform.parent.GetComponent<Rigidbody2D>().velocity.magnitude) * egdecSpeed;
                //Debug.Log("Distance:" + offset);
                //Debug.Log("Energy keeps Losing offset:" + offset);
                energy -= offset;
                transform.GetChild(0).gameObject.GetComponent<TrailRenderer>().time = energy / 100f;
                //movingTime = curMovingTime;
                //Debug.Log("Energy:" + enegy);
            }

        }else{
            //If the player is in orbit, stop lossing energy
            if (energyLoss.isEmitting)
                energyLoss.Stop();
        }

        // Death detection
        if (energy <= 1 ||
            transform.position.x < -Constants.maxX - 10 ||
            transform.position.x > Constants.maxX ||
            transform.position.y < -Constants.maxY - 10 ||
            transform.position.y > Constants.maxY
           ){
            //Application.LoadLevel(Application.loadedLevel);
            energyLoss.Stop();
            transform.parent.GetComponent<BoxCollider2D>().enabled = false;
            //transform.GetChild(0).GetComponent<TrailRenderer>().enabled = false;
            RespawnPlayer();
            Debug.Log("hide!!!!!!!");
            StartCoroutine(waitInHiding());
        }


    }

    void Rotate(){
        /*
        Todo: this function is for keeping the player rotating around a planet when it is around one
        */

        Vector2 pos1 = new Vector2(transform.position.x, transform.position.y);
        Vector2 pos2 = new Vector2(rotation_center.x, rotation_center.y);
        float dis = Vector2.Distance(pos1, pos2);

        float sin = (pos1.y - pos2.y) / dis;
        float cos = (pos1.x - pos2.x) / dis;

        Vector2 new_v = new Vector2(sin, -cos) * rotating_dir * rotating_speed;

        Vector2 offset = pos2 - pos1;
        offset.Normalize();

        transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity = new_v + offset * inwardVel;
        Vector2 v_dir = transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity;
        v_dir.Normalize();
        transform.up = new Vector3(v_dir.x, v_dir.y, 0);
    }

    void Launch(){
        /*
        Todo: This function makes the player derail when it is in an orbit 
        */
        rotate_on = false;

        float x_speed = Mathf.Abs(transform.parent.GetComponent<Rigidbody2D>().velocity.x);
        float y_speed = Mathf.Abs(transform.parent.GetComponent<Rigidbody2D>().velocity.y);
        float origin_speed = Mathf.Sqrt(x_speed * x_speed + y_speed * y_speed);

        if (origin_speed < 0.1f)
            origin_speed = 1f;
        transform.parent.GetComponent<Rigidbody2D>().velocity = transform.up * origin_speed * launch_speed;
        moving = true;

    }

    void modVisibility(){
        /*
        Todo:
        This function is responsible for hiding the player for the moment of death.
        */
        //if (startHide){
        //    transform.GetChild(0).GetComponent<TrailRenderer>().enabled = false;
        //    startHide = false;
        //    changedBack = false;
        //    //spawnPoint = transform.parent.transform.position;
        //}
        //if (hide){
        //    if (currTime < startInSeconds){
        //        //Debug.Log(currTime);
        //        currTime += Time.deltaTime;
        //    }else{
        //        hide = false;
        //        currTime = 0;
        //        ReinitPlayer();
        //        transform.parent.transform.position = spawnPoint;
        //    }
        //}else{
        //    if (!changedBack){
        //        Debug.Log("Changed back");
        //        transform.GetChild(0).GetComponent<TrailRenderer>().enabled = true;
        //        changedBack = true;
        //    }
        //}
    }

    void RespawnPlayer(){
        /*
        Todo: this function triggers the reinitialization process when restarting game
        */
        ReinitScene();
        hide = true;
        startHide = true;
    }

    IEnumerator waitInHiding() {
        /*
        Todo: this coroutine is used to hide player for a tiny moment after death
        */
        transform.GetChild(0).GetComponent<TrailRenderer>().Clear();
        transform.GetChild(0).GetComponent<TrailRenderer>().enabled = false;
        ReinitPlayer();
        yield return new WaitForSeconds(0.1f);
        Debug.Log("show!!!!!!!");
        transform.GetChild(0).GetComponent<TrailRenderer>().enabled = true;
        transform.parent.transform.position = spawnPoint;

    }


}
