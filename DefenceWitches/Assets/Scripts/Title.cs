using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Title : MonoBehaviour
{
    private int scenestate = 0;
    public int maxpage = 2;
    public GameObject illust; //ここにプレイ方法の書いたオブジェクトを入れる
    // Start is called before the first frame update
   
    void Update()
    {
        if (Input.GetButtonDown("Done1p") || Input.GetButtonDown("Done2p"))
        {
            scenestate++;
            if (scenestate == 1)
            {
                illust.SetActive(true);
            }
            if (scenestate >= maxpage)
            {
                SceneManager.LoadScene("Main");
            }
        }
    }
    // Update is called once per frame
    
}

