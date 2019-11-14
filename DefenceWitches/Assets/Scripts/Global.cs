using UnityEngine;
using System.Collections;

/// グローバル情報
public class Global
{
    /// 初期化
    public static void Init()
    {
        _wave = 1;
        _money = MONEY_INIT;
        _money2 = MONEY_INIT;
        // ライフ初期化
        _life = LIFE_INIT;
        _life2 = LIFE_INIT;
    }

    /// 所持金
    // 初期値
    const int MONEY_INIT = 30;
    static int _money;
    static int _money2;
    public static int Money
    {
    get { return _money; }
    }
    public static int Money2
    {
    get { return _money2; }
    }
    // 所持金を増やす
    public static void AddMoney(int v)
    {
    _money += v;
    }
    // 所持金を増やす2
    public static void AddMoney2(int v)
    {
    _money2 += v;
    }
    // お金を使う
    public static void UseMoney(int v)
    {
    _money -= v;
    if (_money < 0)
    {
        _money = 0;
    }
    }
    // お金を使う2
    public static void UseMoney2(int v)
    {
    _money2 -= v;
    if (_money2 < 0)
    {
        _money2 = 0;
    }
    }

    /// ライフ
    // 初期値
    const int LIFE_INIT = 3;
    // 最大値
    public const int LIFE_MAX = 3;
    static int _life;
    static int _life2;
    public static int Life
    {
    get { return _life; }
    }
    public static int Life2
    {
        get { return _life2; }
    }
    public static void Damage()
    {
    // ライフを1つ減らす
    _life--;
    if (_life < 0)
    {
        _life = 0;
    }
    }
    public static void Damage2()
    {
        // ライフを1つ減らす
        _life2--;
        if (_life2 < 0)
        {
            _life2 = 0;
        }
    }

    /// Wave数
    static int _wave = 1;
    public static int Wave
    {
    get { return _wave; }
    }
    /// Wave数を次に進める
    public static void NextWave()
    {
    _wave++;
        Enemy.refcount1 = _wave * 2;
        Enemy.refcount2 = _wave * 2;
    }
}
