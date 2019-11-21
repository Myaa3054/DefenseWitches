using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// カーソル
public class Cursor2 : Token
{
    // x軸方向に１マス移動するときの距離
    Vector3 MOVEX = new Vector3(0.32f, 0, 0);
    // y軸方向に１マス移動するときの距離
    Vector3 MOVEY = new Vector3(0, 0.96f, 0);

    // 移動速度
    float step = 4f;
    // 入力受付時、移動後の位置を算出して保存
    Vector3 target;
    // 何らかの理由で移動できなかった場合、元の位置に戻すため移動前の位置を保存
    Vector3 prevPos;
 
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

    public bool _Placeable = false;
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

    void Start()
    {
        target = transform.position;
    }

    /// 更新
    public void Proc(Layer2D lCollision)
    {
        // ① 移動中かどうかの判定。移動中でなければ入力を受付
        if (transform.position == target)
        {
            SetTargetPosition();
            _Placeable = true;
        }
        else
        {
            _Placeable = false;
        }
        Move();

        Vector2 posWorld = transform.position;

        // スナップ処理
        // チップ座標系
        int i = Field.ToChipX(posWorld.x);
        int j = Field.ToChipY(posWorld.y);

        // 配置可能かチェック
        Placeable = (lCollision.Get(i, j) == 0);

        // 領域外かどうかチェック
        Visible = (lCollision.IsOutOfRange(i, j) == false);

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

    void SetTargetPosition()
    {

        prevPos = target;

        if (Input.GetAxis("Horizontal2p") > 0)
        {
            if (transform.position.x <= 2.24f)
            {
                target = transform.position + MOVEX;
                return;
            }
        }
        if (Input.GetAxis("Horizontal2p") < 0)
        {
            if (transform.position.x >= 1.12f)
            {
                target = transform.position - MOVEX;
                return;
            }
        }
        if (Input.GetAxis("Vertical2p") < 0)
        {
            if (transform.position.y < 2.04f)
            {
                target = transform.position + MOVEY;
                return;
            }
        }
        if (Input.GetAxis("Vertical2p") > 0)
        {
            if (transform.position.y > -1.76f)
            {
                target = transform.position - MOVEY;
                return;
            }
        }
    }

    // ③ 目的地へ移動する
    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, step * Time.deltaTime);
    }
}
