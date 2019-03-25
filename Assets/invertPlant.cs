using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invertPlant : Planet {


    public bool invBack;

    public override void catchedAction(spacecraft sc)
    {
        base.catchedAction(sc);
        if(!invBack)
        {
            sc.camera.transform.GetChild(0).GetComponent<colorInverter>().inverting = true;
            StartCoroutine(invertActions(sc));
        }
        else{
            sc.camera.transform.GetChild(0).GetComponent<colorInverter>().invertingBack = true;
        }
    }

    IEnumerator invertActions(spacecraft sc)
    {
        yield return new WaitUntil(()=>!sc.camera.transform.GetChild(0).GetComponent<colorInverter>().inverting);
        yield return new WaitForSeconds(0.5f);
        sc.camera.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        yield return new WaitForSeconds(0.1f);
        foreach (GameObject ob in allObjects)
        {
            if (ob != gameObject && ob.tag != "Player" && ob.tag != "end" && ob.tag != "MainCamera" && ob.tag != "background" && ob.transform.parent == null)
            {
                ob.SetActive(false);
            }
        }
        sc.camera.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;


    }
}
