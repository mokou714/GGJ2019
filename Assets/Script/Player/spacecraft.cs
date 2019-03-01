using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class spacecraft : MonoBehaviour {
    /*
    This is the script executed in the player, responsible for player control and basic behaviors
    */

    // postion when player launch from a planet, used an initial to calculate energy loss
    Vector2 launchPos;
    float maxDistance;
    float launchEnergy; 

    public int rotation_sensitivity;
    public float launch_speed;
    public float energy;

    public GameObject camera;
    Vector2 screenSize;

    public bool launched;

    public float start_velocity;
    public GameObject end;


    public float rotating_speed;
    public bool rotate_on;
    public Vector3 rotation_center;
    public int rotating_dir;
      
    public GameObject rotatingPlanet;
    public GameObject preRotatingPlanet;

    private float movingTime = 0;
    public bool movingStart = false;

    private float originalWidth;

    public ParticleSystem energyLoss;
    private Vector2 energyLossLocalOffset;
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
    private float origSpeed;

    public float energy2dis;
    public float checkRotatingTime = 0;

    public bool requiredStop = false;
    public bool requiredSleep = false;
    public float inwardVel;

	// Use this for initialization
	void Start () {
        InitPlayer(false);//Init player with reinit parameter being false

    }

    private void InitPlayer(bool reinit = true){
        /*
        Todo: this function initialize parameters of player when starting/restarting the current level
        */
        if(reinit)
            parentRigidBody.velocity = Vector3.zero;//If it's not the first time init, stop the rigidbody speed for restarting
        else{
            //If it is the first time to initialize, store some the initial values.
            parentRigidBody = transform.parent.GetComponent<Rigidbody2D>();
            energyLoss = transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>();
            energyLossLocalOffset = energyLoss.transform.localPosition;
            originalWidth = transform.GetChild(0).gameObject.GetComponent<TrailRenderer>().widthMultiplier;
            spawnPoint = transform.position;
            origSpeed = rotating_speed;
        }

        energy = 100f;
        transform.GetChild(0).gameObject.GetComponent<TrailRenderer>().time = energy / 100f;
        transform.GetChild(0).gameObject.GetComponent<TrailRenderer>().widthMultiplier = originalWidth;
        movingStart = true;

        Vector3 dir = new Vector3(1, 0, 0);
        parentRigidBody.velocity = dir * start_velocity;
        launched = false;
        movingTime = 0;
        energyLoss = transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>();
        rotatingPlanet = preRotatingPlanet = null;
    }
	
    private void ReinitScene(){
        /*
        Todo: this function reinitialize the scene's other objects, pulling some of them back to their original positions
        */
        object[] scene_obj = FindObjectsOfType(typeof(GameObject));

        foreach (object obj in scene_obj){
            //Iterate through all the gameobject in this scene to execute their reinitializations
            GameObject single_obj = (GameObject)obj;
            //Debug.Log("Dust planet detected " + single_obj.GetComponent<Planet>());

            //Execute basic recovering code for all kinds of planet
            if(single_obj.GetComponent<Planet>() != null){
                single_obj.GetComponent<Planet>().Recover();
            }

            string obj_tag = single_obj.tag;
            switch(obj_tag){
                case "begin":
                    //Add one more check to prevent the mis-tagging causing errors, same as following ones
                    if (single_obj.gameObject.GetComponent<StopParticles>() == null)
                        break;
                    //Begin point redo the showup
                    single_obj.gameObject.GetComponent<StopParticles>().ParticleReStart();
                    break;
                case "asteroid":
                    if (single_obj.gameObject.GetComponent<Asteroid>() == null)
                        break;
                    //Asteroids move back to original positions
                    single_obj.gameObject.GetComponent<Asteroid>().Recover();
                    break;
                case "orbasteroid":
                    if (single_obj.gameObject.GetComponent<orbitAsteroid>() == null)
                        break;
                    //For orbit asteroid, if the asteroid is already out of its orbit, it should go back when the scene restarts
                    single_obj.gameObject.GetComponent<orbitAsteroid>().Recover();
                    break;
                case "dustPlanet":
                    if (single_obj.gameObject.GetComponent<dustPlanet>() == null)
                        break;
                    //Determine if it actually has dust
                    single_obj.gameObject.GetComponent<dustPlanet>().Recover();
                    break;
            }


        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount == 1))
        {
            //Derail when the input is detected and player is in an orbit
            if (!requiredStop)
            { //requiredStop is for pause request out of the player object such as from tutorial manager
                if (rotate_on)
                    Launch();
            }
        }
        else if (Input.GetKeyDown(KeyCode.R) || (Input.touchCount == 2))
        {
            SceneManager.LoadScene(Application.loadedLevel);
        }
    }

    void FixedUpdate () {
        if (requiredSleep)
            return;
        //Set up the original width of player
        transform.GetChild(0).gameObject.GetComponent<TrailRenderer>().widthMultiplier = originalWidth * energy / 100f;


        if(rotate_on){
            //If no valid input, keep rotating around the current planet
            Rotate();
        }
        //Indicating wether the player is in orbit or flying straight
        if(launched){
            curMovingTime += Time.deltaTime;
            //Detecting if the previous moment is orbiting
            if (!movingStart)
            {
                //Debug.Log("Energy starts Losing");
                movingStart = true;
                movingTime = curMovingTime;
                //Debug.Log("start recording :" + movingTime);
                Vector2 playerVelocity = transform.parent.GetComponent<Rigidbody2D>().velocity;
                Vector2 newEnergyLossLocalPos = energyLossLocalOffset.magnitude * playerVelocity.normalized;
                float angleBetween = Mathf.Atan2(newEnergyLossLocalPos.y, newEnergyLossLocalPos.x) * Mathf.Rad2Deg;
                //print("ANgle: " + angleBetween);
                Vector3 energyLossRotation = new Vector3(
                    0, 
                    0, 
                    angleBetween - 90
                );
                energyLoss.transform.rotation = Quaternion.Euler(energyLossRotation);
                energyLoss.transform.localPosition = newEnergyLossLocalPos;
                energyLoss.Play();
            }else{
                if (parentRigidBody.velocity.magnitude < speedThreshold)
                {
                    float dec = (curMovingTime - movingTime) / (parentRigidBody.velocity.magnitude + 0.1f);
                    energy -= dec; // fast decrease
                }
                else
                {
                    // scale energy with distance
                    float distance = Vector2.Distance((Vector2)transform.parent.position, launchPos);

                    energy = launchEnergy * (1 - distance / maxDistance);
                }
                
                transform.GetChild(0).gameObject.GetComponent<TrailRenderer>().time = energy / 100f;

            }
        }else{
            //If the player is in orbit, stop lossing energy particles
            if (energyLoss.isEmitting)
                energyLoss.Stop();
        }

        // Death detection, when the player is out of the camera view
        Vector2 viewportPos = camera.GetComponent<Camera>().WorldToViewportPoint(transform.position);
        if (energy <= 5f ||
            viewportPos.x < -0.2f ||
            viewportPos.x > 1.2f||
            viewportPos.y < -0.2f ||
             viewportPos.y > 1.2){
            energyLoss.Stop();

            //This is for the condition when the player hits the end point but the death is detected at the same time, then we do not restart
            if (requiredStop)
                return;
            ReinitScene();
            StartCoroutine(waitInHiding());
        }


    }

    void Rotate(){
        /*
        Todo: this function is for keeping the player rotating around a planet when it is around one
        */

        Vector2 pos1 = new Vector2(transform.position.x, transform.position.y);//Player's position
        Vector2 pos2 = new Vector2(rotation_center.x, rotation_center.y);//Planet's center position
     
        Vector2 currentVelocity = transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity;

        float angle = Vector2.Angle(pos2 - pos1, currentVelocity);
        Vector2 v1 = Vector2.Dot(currentVelocity,(pos2-pos1).normalized) *(pos2-pos1).normalized ;
        Vector2 newV = (currentVelocity - v1).normalized * rotating_speed;//Get the tangent line 

        //Set up a proper inward velocity depending on the planet's catchRadius to prevent the player rotating outward
        transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity = newV + (pos2-pos1).normalized * inwardVel;
    }

    public void Launch(){
        /*
        Todo: This function makes the player derail when it is in an orbit 
        */
        //Debug.Log("Shoot");
        rotate_on = false;

        // update launch position
        launchPos = transform.parent.position;

        // update maximum distance that player can travel with the current energy
        maxDistance = energy * energy2dis;

        // record launchEnergy for later scaling player's energy with distance
        launchEnergy = energy;

        if (Mathf.Abs(rotating_speed - origSpeed) > 0.1)
            rotating_speed = origSpeed;

        //float x_speed = Mathf.Abs(transform.parent.GetComponent<Rigidbody2D>().velocity.x);
        //float y_speed = Mathf.Abs(transform.parent.GetComponent<Rigidbody2D>().velocity.y);
        Vector3 orig_vel = transform.parent.GetComponent<Rigidbody2D>().velocity.normalized * rotating_speed;
        float x_speed = orig_vel.x;
        float y_speed = orig_vel.y;
        float origin_speed = Mathf.Sqrt(Mathf.Abs(x_speed * x_speed) + Mathf.Abs(y_speed * y_speed));

        if (origin_speed < 0.1f)
            origin_speed = 1f;
        transform.parent.GetComponent<Rigidbody2D>().velocity = orig_vel * origin_speed * launch_speed;
        launched = true;

        checkRotatingTime = Time.time;

        //player left, update player&planet references
        rotatingPlanet.GetComponent<Planet>().playerLeave();
        preRotatingPlanet = rotatingPlanet;
        rotatingPlanet = null;
    }

    IEnumerator waitInHiding() {
        /*
        Todo: this coroutine is used to hide player for a tiny moment after death
        */
        transform.GetChild(0).GetComponent<TrailRenderer>().Clear();
        transform.GetChild(0).GetComponent<TrailRenderer>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        transform.GetChild(0).GetComponent<TrailRenderer>().enabled = true;
        transform.parent.transform.position = spawnPoint;
        InitPlayer();

    }

    public void landOn(){
        inwardVel = 1 / (10 * rotatingPlanet.GetComponent<Planet>().catchRadius);
    }
}
