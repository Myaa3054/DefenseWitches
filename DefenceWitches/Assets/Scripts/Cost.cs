﻿using UnityEngine;
using System.Collections;

/// コスト計算するクラスメソッドのみのクラス
public class Cost
{
    /// タワーを買うために必要なコストを取得する
    public static int TowerProduction()
    {
        // ①生存数を取得
        int num = Tower.parent.Count();

        // ②タワー生産コスト＝ 10 * (1.5 ^ タワーの存在数)
        int basic = 10;
        float ratio = Mathf.Pow(1.5f, num);

        // ③小数点は切り捨て
        return (int)(basic * ratio);
    }
    public static int TowerProduction2()
    {
        // ①生存数を取得
        int num2 = Tower2.parent.Count();

        // ②タワー生産コスト＝ 8 * (1.3 ^ タワーの存在数)
        int basic = 10;
        float ratio = Mathf.Pow(1.5f, num2);

        // ③小数点は切り捨て
        return (int)(basic * ratio);
    }


    /// アップグレードコストを取得する
    public static int TowerUpgrade(Tower.eUpgrade type, int lv)
    {
        float cost = 0;
        switch (type)
        {
            case Tower.eUpgrade.Range:
            // 射程範囲
            cost = 10 * Mathf.Pow(1.5f, (lv - 1));
            break;

            case Tower.eUpgrade.Firerate:
            // 連射速度
            cost = 15 * Mathf.Pow(1.5f, (lv - 1));
            break;

            case Tower.eUpgrade.Power:
            // 攻撃威力
            cost = 20 * Mathf.Pow(1.5f, (lv - 1));
            break;
        }
        // 小数点以下を切り捨てる
        return (int)cost;
    }

    /// アップグレードコストを取得する
    public static int TowerUpgrade2(Tower2.eUpgrade type, int lv)
    {
        float cost = 0;
        switch (type)
        {
            case Tower2.eUpgrade.Range:
            // 射程範囲
            cost = 10 * Mathf.Pow(1.5f, (lv - 1));
            break;

            case Tower2.eUpgrade.Firerate:
            // 連射速度
            cost = 10 * Mathf.Pow(1.5f, (lv - 1));
            break;

            case Tower2.eUpgrade.Power:
            // 攻撃威力
            cost = 10 * Mathf.Pow(1.5f, (lv - 1));
            break;
        }
        // 小数点以下を切り捨てる
        return (int)cost;
    }
}

