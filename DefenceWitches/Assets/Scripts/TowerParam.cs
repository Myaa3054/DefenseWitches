using UnityEngine;
using System.Collections;

/// タワーのパラメータ計算クラス
public class TowerParam
{
  /// 射程範囲
  public static float Range(int lv)
  {
    float size = Field.GetChipSize();
    return size + (1.0f * size * lv);
  }

  /// 連射速度
  public static float Firerate(int lv)
  {
    return 4.0f * (0.5f * lv);
  }

  /// 攻撃威力
  public static int Power(int lv)
  {
    return 1 * lv;
  }
}
