using UnityEngine;
using System.Collections;

public class Gui2
{
    // Wave数テキスト
    TextObj _txtWave;
    // 所持金テキスト
    TextObj _txtMoney2;
    // コストテキスト
    TextObj _txtCost2;
    // 購入ボタン
    ButtonObj _btnBuy2;
    // タワー情報テキスト
    TextObj _txtTowerInfo2;
    /// アップグレードボタン
    // 射程範囲
    ButtonObj _btnRange2;
    // 連射速度
    ButtonObj _btnFirerate2;
    // 攻撃威力
    ButtonObj _btnPower2;

    /// コンストラクタ
    public Gui2()
    {
    // Wave数
    _txtWave = MyCanvas.Find<TextObj>("TextWave2");
    // 所持金テキスト
    _txtMoney2 = MyCanvas.Find<TextObj>("TextMoney2");
    // コストテキスト
    _txtCost2 = MyCanvas.Find<TextObj>("TextCost2");
    _txtCost2.Label = "";
    // 購入ボタン
    _btnBuy2 = MyCanvas.Find<ButtonObj>("ButtonBuy2");
    // タワー情報を取得する
    _txtTowerInfo2 = MyCanvas.Find<TextObj>("TextTowerInfo2");
    // 射程範囲ボタン
    _btnRange2 = MyCanvas.Find<ButtonObj>("ButtonRange2");
    // 連射速度ボタン
    _btnFirerate2 = MyCanvas.Find<ButtonObj>("ButtonFirerate2");
    // 攻撃威力ボタン
    _btnPower2 = MyCanvas.Find<ButtonObj>("ButtonPower2");
    }

    /// 更新
    public void Update2(GameMgr.eSelMode2 selMode2, Tower2 tower2)
    {
        // Wave数を更新
        _txtWave.SetLabelFormat("Wave: {0}", Global.Wave);
        _txtMoney2.SetLabelFormat("MP: {0}", Global.Money2);
        // 生産コストを取得する
        int cost = Cost.TowerProduction();
        _txtCost2.Label = "";
        if (selMode2 == GameMgr.eSelMode2.Buy)
        {
            // 購入モードのみテキストを設定する
            _txtCost2.SetLabelFormat("(消費 {0})", cost);
        }
        // 購入ボタンを押せるかどうかチェック
        _btnBuy2.Enabled = (Global.Money2 >= cost);
        // 購入コストを表示する
        _btnBuy2.FormatLabel("呼び出す ({0})", cost);
        // ライフ表示
        for (int i = 0; i < Global.LIFE_MAX; i++)
        {
            bool b = (Global.Life2 > i);
            MyCanvas.SetActive("ImageLife2" + i, b);
        }

        if (selMode2 == GameMgr.eSelMode2.Upgrade)
        {
            // 選択しているタワーの情報を表示する
            _txtTowerInfo2.SetLabelFormat(
            "<<情報>>\n  範囲: Lv{0}\n  速度: Lv{1}\n  威力: Lv{2}",
            tower2.LvRange,
            tower2.LvFirerate,
            tower2.LvPower
            );

            // アップグレードボタン更新
            int money2 = Global.Money2;
            // 射程範囲
            _btnRange2.Enabled = (money2 >= tower2.CostRange);
            _btnRange2.FormatLabel("範囲 (${0})", tower2.CostRange);
            // 連射速度
            _btnFirerate2.Enabled = (money2 >= tower2.CostFirerate);
            _btnFirerate2.FormatLabel("速度 (${0})", tower2.CostFirerate);
            // 攻撃威力
            _btnPower2.Enabled = (money2 >= tower2.CostPower);
            _btnPower2.FormatLabel("威力 (${0})", tower2.CostPower);
        }
    }
}
