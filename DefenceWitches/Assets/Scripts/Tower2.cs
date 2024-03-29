﻿using UnityEngine;
using System.Collections;

/// タワー
public class Tower2 : Token
{
    // アニメーション用のスプライト
    public Sprite spr0;
    public Sprite spr1;
    int _tAnim = 0;

    /// コスト
    // 射程範囲
    public int CostRange
    {
    get { return Cost.TowerUpgrade2(eUpgrade.Range, _lvRange); }
    }
    // 連射速度
    public int CostFirerate
    {
    get { return Cost.TowerUpgrade2(eUpgrade.Firerate, _lvFirerate); }
    }
    // 攻撃威力
    public int CostPower
    {
    get { return Cost.TowerUpgrade2(eUpgrade.Power, _lvPower); }
    }
    /// アップグレード種別に対応したコストを取得する
    public int GetCost(eUpgrade type)
    {
    switch (type)
    {
        case eUpgrade.Range: return CostRange;
        case eUpgrade.Firerate: return CostFirerate;
        case eUpgrade.Power: return CostPower;
    }
    return 0;
    }

    /// アップグレードの種類
    public enum eUpgrade
    {
    Range,    // 射程範囲
    Firerate, // 連射速度
    Power,    // 攻撃威力
    }

    // タワー管理
    public static TokenMgr<Tower2> parent;
    // タワー生成
    public static Tower2 Add(float px, float py) {
    Tower2 t = parent.Add<Tower2>(px, py);
    if(t == null) {
        return null;
    }
    t.Init();
    return t;
    }

    // ショットの速度
    const float SHOT_SPEED = 5.0f;

    // 射程範囲
    float _range;
    // 連射速度
    float _firerate;
    // 連射速度インターバルタイマー
    float _tFirerate;
    // 攻撃威力
    int _power;

    /// レベル
    // 射程範囲
    int _lvRange;
    public int LvRange
    {
    get { return _lvRange; }
    }
    // 連射速度
    int _lvFirerate;
    public int LvFirerate
    {
    get { return _lvFirerate; }
    }
    // 攻撃威力
    int _lvPower;
    public int LvPower
    {
    get { return _lvPower; }
    }

    /// パラメータ更新
    void UpdateParam()
    {
        // 射程範囲
        _range = TowerParam.Range(_lvRange);
        // 連射速度
        _firerate = TowerParam.Firerate(_lvFirerate);
        // 攻撃威力
        _power = TowerParam.Power(_lvPower);

        // レベルに対する色を設定
        // 平均レベルを計算
        float avg = (_lvRange + _lvFirerate + _lvPower) / 1.0f;
        // 小数点以下を切り上げする
        int avgLv = Mathf.CeilToInt(avg);
        Color c;
        switch (avgLv)
        {
            case 3: c = Color.white; break; // 白色
            case 4: c = Color.cyan; break; // シアン
            case 5: c = Color.green; break; // 緑色
            case 6: c = Color.yellow; break; // 黄色
            default: c = Color.red; break; // 赤色
        }
        // 少し明るくする
        c.r += 0.3f;
        c.g += 0.3f;
        c.b += 0.3f;
        SetColor(c);
    }

    // 初期化
    void Init()
    {
        // 連射速度インターバル初期化
        _tFirerate = 0;

        // レベル初期化
        _lvRange = 1;
        _lvFirerate = 1;
        _lvPower = 1;

        // パラメータ更新
        UpdateParam();
    }

    void Update ()
    {
        //アニメーション
        _tAnim++;
        if (_tAnim % 32 < 16)
        {
            SetSprite(spr0);
        }
        else
        {
            SetSprite(spr1);
        }

        // インターバルタイマー更新 (※①)
        _tFirerate += Time.deltaTime;

        // 一番近い敵を求める(④)
        Enemy e = Enemy.parent.Nearest(this);
        if (e == null)
        {
        // 敵がいないので何もしない
        return;
        }
        // 敵への距離を取得する(⑤)
        float dist = Util.DistanceBetween(this, e);
        float targetAngle;

        // 敵への角度を取得(⑥)
        targetAngle = Util.AngleBetween(this, e);


        if (dist > _range)
        {
            // 射程範囲外
            return;
        }


        // 現在向いている角度との差を求める
        float dAngle = Mathf.DeltaAngle(Angle, targetAngle);

        // インターバルチェック (※②)
        if(_tFirerate < _firerate)
        {
            // インターバル中
            return;
        }

        // ショットを撃つ
        Shot.Add(X, Y, targetAngle, SHOT_SPEED, _power);

        // インターバルを入れる
        _tFirerate = 0;
    }

    /// アップグレードする
    public void Upgrade(eUpgrade type)
    {
        switch (type)
        {
            case eUpgrade.Range:
            // 射程範囲のレベルアップ
            _lvRange++;
            break;

            case eUpgrade.Firerate:
            // 連射速度のレベルアップ
            _lvFirerate++;
            break;

            case eUpgrade.Power:
            // 攻撃威力のレベルアップ
            _lvPower++;
            break;
        }
        // パラメータ更新
        UpdateParam();

        // アップグレードエフェクト生成
        Particle p = Particle.Add(Particle.eType.Ellipse, 20, X, Y, 0, 0);
        if (p)
        {
            p.SetColor(0.2f, 0.2f, 1);
        }
    }
}
