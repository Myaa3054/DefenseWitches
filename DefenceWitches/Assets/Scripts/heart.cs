using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heart : Token
{
    // アニメーション用のスプライト
    public Sprite spr0;
    public Sprite spr1;
    // アニメーションタイマー
    protected int _tAnim = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        _tAnim++;
        if (_tAnim % 32 < 16)
        {
            SetSprite(spr0);
        }
        else
        {
            SetSprite(spr1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
