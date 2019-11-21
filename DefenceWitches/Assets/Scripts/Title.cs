using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{    
    public static bool Howdo = true;
    void Update()
    {
        if (Input.GetButtonDown("Done1p") || Input.GetButtonDown("Done2p"))
        {    
            SceneManager.LoadScene("Main");
            Howdo = true;
        }

        if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Cancel"))
        {
            SceneManager.LoadScene("Main");
            Howdo = false;
        }
    }        
}

