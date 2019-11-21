using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{    
    void Update()
    {
        if (Input.GetButtonDown("Done1p") || Input.GetButtonDown("Done2p"))
        {    
                SceneManager.LoadScene("Main");
        }
    }        
}

