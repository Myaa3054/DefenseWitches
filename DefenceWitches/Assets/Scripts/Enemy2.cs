using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// 敵クラス
public class Enemy2 : Enemy
{
    // 管理オブジェクト
    //public static TokenMgr<Enemy2> parent = null;

    // プレハブから敵を生成
    new public static Enemy2 Add(List<Vec2D> path)
    {
        Enemy2 e = parent.Add<Enemy2>(0, 0);
        if (e == null)
        {
            return null;
        }
        e.Init(path);
        return e;
    }

    new public void Init(List<Vec2D> path)
    {
        // 経路をコピー
        _path = path;
        _pathIdx = 0;
        // 移動速度
        _speed = EnemyParam.Speed2();
        _tSpeed = 0;

        // 移動先を更新
        MoveNext();
        // _prevに反映する
        _prev.Copy(_next);
        // 一つ下にずらす
        _prev.y -= Field.GetChipSize();
        // 一度座標を更新しておく
        FixedUpdate();

        // HPを設定する
        _hp = EnemyParam.Hp2();

        // 所持金を設定
        _money = EnemyParam.Money();
    }

    /// 衝突判定
    void OnTriggerEnter2D(Collider2D other)
    {
        // レイヤー名を取得する
        string name = LayerMask.LayerToName(other.gameObject.layer);
        if (name == "Shot")
        {
            // ショットと衝突
            Shot s = other.gameObject.GetComponent<Shot>();
            // ショット消滅
            s.Vanish();

            // ダメージ処理をする
            Damage(s.Power);

            if (Exists == false)
            {
                // 所持金を増やす
                if (X < 0)
                {
                    Global.AddMoney(_money);
                }

                else
                {
                    Global.AddMoney2(_money);
                }
            }
        }
    }

    /// ダメージを受けた
    void Damage(int val)
    {
        // HPを減らす
        _hp -= val;
        if (_hp <= 0)
        {
            // HPがなくなったので死亡
            if (Y > 0.64)
            {
                if (X < 0)
                {
                    if (refcount1 > 0)
                    {
                        Add(GameMgr._path2);
                        refcount1 -= 1;
                    }
                }
                else
                {
                    if (refcount2 > 0)
                    {
                        Add(GameMgr._path);
                        refcount2 -= 1;
                    }
                }
            }
            Vanish();
        }
    }

    /// 消滅
    public override void Vanish()
    {
        // パーティクル生成
        // リングエフェクト生成
        {
            // 生存時間は30フレーム。移動はしない
            Particle p = Particle.Add(Particle.eType.Ring, 30, X, Y, 0, 0);
            if (p)
            {
                // 明るい緑
                p.SetColor(0.7f, 1, 0.7f);
            }
        }

        // ボールエフェクト生成
        float dir = Random.Range(35, 55);
        for (int i = 0; i < 8; i++)
        {
            // 消滅フレーム数
            int timer = Random.Range(20, 40);
            // 移動速度
            float spd = Random.Range(0.5f, 2.5f);
            Particle p = Particle.Add(Particle.eType.Ball, timer, X, Y, dir, spd);
            // 移動方向
            dir += Random.Range(35, 55);
            if (p)
            {
                // 緑色を設定
                p.SetColor(0.0f, 1, 0.0f);
                // 大きさを設定
                p.Scale = 0.8f;
            }
        }

        // 親の消滅処理を呼び出す
        base.Vanish();
    }
}

