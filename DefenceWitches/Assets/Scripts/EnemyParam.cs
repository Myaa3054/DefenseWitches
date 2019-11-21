using UnityEngine;
using System.Collections;

/// ①敵のパラメータに関する情報を計算するクラス
public class EnemyParam
{
    /// ②敵のHPを取得する
    public static int Hp()
    {
        return 1 + (Global.Wave / 4);
    }
    /// ②敵のHPを取得する
    public static int Hp2()
    {
        return 1 + (Global.Wave / 2);
    }

    /// ③敵の速度を取得する
    public static float Speed()
    {
        return 3 + (0.3f * Global.Wave);
    }

    /// ③敵の速度を取得する
    public static float Speed2()
    {
        return 2 + (0.05f * Global.Wave);
    }

    /// ④敵の所持金を取得する
    public static int Money()
    {
    if (Global.Wave < 5)
    {
        // Wave4以下は2
        return 4;
    }
        // 5以上は1
        return 2;
    }

    /// ⑤敵の出現数を取得する
    public static int GenerationNumber()
    {
        return 1 + Global.Wave*2 ;
    }

    public static int GenerationNumber2()
    {
        return 1 + Global.Wave*2;
    }

    /// ⑥敵の出現間隔を取得する
    public static float GenerationInterval()
    {
        // 1.5sec.
        return 1.5f;
    }
}
