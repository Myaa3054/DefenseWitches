using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// 敵クラス
public class Enemy : Token
{
    //アニメーション用のスプライト 
    public Sprite spr0;
    public Sprite spr1;

    // アニメーション タイマー 
    int tAnim = 0;

    //速度パラメータ 
    float speed = 0;// 速度 
    float tSpeed = 0;// 補完値( 0.0〜 100.0)

    //経路座標のリスト
    List<Vec2D> path;

    //経路の現在の番号 
    int pathIdx;
    //チップ座標 
    Vec2D prev; // 1つ前 
    Vec2D next; // 1つ先

    //初期化
    public void Init(List<Vec2D> path)
    {
        //経路をコピー 
        path = path;
        pathIdx = 0;
        //移動速度 
        speed = 2.0f;
        tSpeed = 0;

        //移動先を更新 
        MoveNext();
        //prevに反映 する 
        prev.Copy(next);
        //一つ左にずらす 
        prev.x -= Field.GetChipSize();
        //一度座標を更新しておく
        FixedUpdate();
    }

    // アニメーション更新 
    void FixedUpdate()
    {
        tAnim++;
        if ( tAnim % 32 < 16)
        {
            SetSprite(spr0);
        }
        else
        {
            SetSprite(spr1);
        }

        // 速度タイマー更新 
        tSpeed += speed;
        if (tSpeed >= 100.0f)
        {
            // 移動先を次に進める 
            tSpeed -= 100.0f;
            MoveNext();
        }

        //速度タイマーに対応する位置に線形補間で移動する 
        X = Mathf.Lerp(prev.x, next.x, tSpeed / 100.0f);
        Y = Mathf.Lerp(prev.y, next.y, tSpeed / 100.0f);
    }

    //次の移動先に進める 
    void MoveNext()
    {
        if( pathIdx >= path.Count)
        {
            // ゴールにたどりついた 
            tSpeed = 100.0f;
            return;
        }
        //移動先を移動元にコピーする
        prev.Copy( next);

        //チップ座標を取り出す 
        Vec2D v = path[pathIdx];
        next.x = Field.ToWorldX(v.X);
        next.y = Field.ToWorldY(v.Y);
        //パス番号を進める 
        pathIdx++;
    }
}