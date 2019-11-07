﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// カーソル
public class Cursor : Token
{
    // 速度
    public Vector2 SPEED = new Vector2(0.1f, 0.1f);
    /// 表示スプライト
    // 四角
    public Sprite sprRect;
    // バッテン
    public Sprite sprCross;

    // カーソルにあるオブジェクト
    GameObject _selObj = null;
    public GameObject SelObj
    {
    get { return _selObj; }
    }


    // 配置可能かどうか
    bool _bPlaceable = true;
    public bool Placeable
    {
    get { return _bPlaceable; }
    set {
        if(value)
        {
        // 配置できるので四角
        SetSprite(sprRect);
        }
        else
        {
        // 配置できないのでバッテン
        SetSprite(sprCross);
        }
        _bPlaceable = value;
    }
    }

    /// 更新
    public void Proc(Layer2D lCollision)
    {
        // 座標を取得
        Vector2 pos = transform.position;

        // スティックを右に倒した時の処理
        if (Input.GetAxis("Horizontal2-1p") > 0)
        {
            // 右に移動する
            pos.x += SPEED.x;
        }

        // スティックを左に倒した時の処理
        if (Input.GetAxis("Horizontal2-1p") < 0)
        {
            //左に移動する
            pos.x -= SPEED.x;
        }

        // スティックを下に倒した時の処理
        if (Input.GetAxis("Vertical2-1p") < 0)
        {
            // 下に移動する
            pos.y += SPEED.y;
        }

        // スティックを上に倒した時の処理
        if (Input.GetAxis("Vertical2-1p") > 0)
        {
            // 上に移動する
            pos.y -= SPEED.y;
        }
        transform.position = pos;
        // スナップ処理
        // チップ座標系
        int i = Field.ToChipX(pos.x);
        int j = Field.ToChipY(pos.y);
        // ワールド座標系に再変換
        X = Field.ToWorldX(i);
        Y = Field.ToWorldY(j);

        // 配置可能かチェック
        //Placeable = (lCollision.Get(i, j) == 0);

        // 領域外かどうかチェック
        //Visible = (lCollision.IsOutOfRange(i, j) == false);

        // 選択しているオブジェクトを設定
        SetSelObj();
    }

    /// カーソルの下にあるオブジェクトを設定する
    void SetSelObj()
    {
    // カーソルの下にあるオブジェクトをチェック
    int mask = 1 << LayerMask.NameToLayer("Tower");
    Collider2D col = Physics2D.OverlapPoint(GetPosition(), mask);
    _selObj = null;
    if(col != null)
    {
        // 選択中のオブジェクトを格納
        _selObj = col.gameObject;
    }
    }
}
