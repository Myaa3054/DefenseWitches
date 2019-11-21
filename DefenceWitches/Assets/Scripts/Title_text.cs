using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title_text : MonoBehaviour
{
    public Image image;
    public float alpha = 0;
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        
    }

    // Update is called once per frame
    void Update()
    {
        alpha = Mathf.Cos(Time.time*1.5f);
        alpha = Mathf.Abs(alpha);
        image.color = new Color(255, 255, 255, alpha);
    }
}
