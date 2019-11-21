using UnityEngine;
using System.Collections;

public class PauseScript : MonoBehaviour
{

    [SerializeField]
    //　ポーズした時に表示するUIのプレハブ
    private GameObject pauseUIPrefab;
    //　ポーズUIのインスタンス
    private GameObject pauseUIInstance;

    void Quit()
    {
       UnityEngine.Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (pauseUIInstance == null)
            {
                pauseUIInstance = GameObject.Instantiate(pauseUIPrefab) as GameObject;
                Time.timeScale = 0f;
            }
            else
            {
                Destroy(pauseUIInstance);
                Time.timeScale = 1f;
            }
            if(pauseUIInstance != null)
            {
                if (Input.GetButtonDown("Done1p") || Input.GetButtonDown("Done2p"))
                {
                    Quit();
                }
            }
        }
    }
}
