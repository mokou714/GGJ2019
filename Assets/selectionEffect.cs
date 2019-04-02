using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.EventSystems;
//ref https://stackoverflow.com/questions/41391708/how-to-detect-click-touch-events-on-ui-and-gameobjects

public class selectionEffect : MonoBehaviour, 
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{


    public float largeScale;
    Vector3 originScale;

    private int bigNumber;

	// Use this for initialization

    void Start()
    {
        originScale = transform.GetChild(0).localScale;
        bigNumber = int.Parse(transform.parent.parent.name);
        // automatically add Physics2DRaycaster to maincamera to make EventSystem work
        addPhysics2DRaycaster();
    }

    void addPhysics2DRaycaster()
    {
        Physics2DRaycaster physicsRaycaster = GameObject.FindObjectOfType<Physics2DRaycaster>();
        if (physicsRaycaster == null)
        {
            Camera.main.gameObject.AddComponent<Physics2DRaycaster>();
        }
    }

    // Update is called once per frame
    void Update () {
		
	}


    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    IEnumerator fadeIn() {
        float currentAlpha = 0f;

        while (currentAlpha < 1f)
        {
            currentAlpha = GetComponent<SpriteRenderer>().color.a;
            float newAlpha = currentAlpha + 0.05f;
            GetComponent<SpriteRenderer>().color = new Color(255f,255f,255f,newAlpha);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator fadeOut()
    {
        float currentAlpha = 1f;

        while (currentAlpha > 0f)
        {
            currentAlpha = GetComponent<SpriteRenderer>().color.a;
            float newAlpha = currentAlpha - 0.05f;
            GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, newAlpha);
            yield return new WaitForSeconds(0.1f);
        }
    }

    // use these for both mouse and touch
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.GetChild(0).localScale = new Vector3(largeScale, largeScale, 0f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.GetChild(0).localScale = originScale;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
        int levelName = int.Parse(gameObject.name);

        int levelID = 10 * (bigNumber - 1) + levelName + 4;
        Debug.Log("Enter a level");
        // load level
        SceneManager.LoadScene(levelID);
        //UIManager.instance.Start();
    }
}
