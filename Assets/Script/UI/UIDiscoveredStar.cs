using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDiscoveredStar : MonoBehaviour {

    public GameObject unknown;
    public GameObject content;

    private void OnEnable()
    {
        if(unknown == null)
            unknown = transform.GetChild(0).gameObject;
        if (content == null)
            content = transform.GetChild(1).gameObject;

        // using name of the star as the key
        if (PlayerPrefs.HasKey(name))
        {
            int value = PlayerPrefs.GetInt(name);

            // special unlocking animation and sound played for first time
            if(value == 0)
            {
                // To do: sfx and animation
                PlayerPrefs.SetInt(name, 1);

            }
            else if(value == 1)
            {
                // just display the content
                unknown.SetActive(false);
                content.SetActive(true);
            }

        }
        else
        {
            // display the unknown image
            unknown.SetActive(true);
            content.SetActive(false);
        }
    }

}
