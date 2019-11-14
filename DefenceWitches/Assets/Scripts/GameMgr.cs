using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/// ゲーム管理
public class GameMgr : MonoBehaviour
{
    /// 停止タイマー
    // 2sec停止する
    const float TIMER_WAIT = 2.0f;
    float _tWait = TIMER_WAIT;

    /// 状態
    enum eState
    {
        Wait, // Wave開始前
        Main, // メイン
        Gameover, // ゲームオーバー
    }
    eState _state = eState.Wait;

    /// 選択モード
    public enum eSelMode
    {
        None, // なし
        Buy, // 購入モード
        Upgrade, // アップグレード
    }
    eSelMode _selMode = eSelMode.None;

    /// 選択モード2
    public enum eSelMode2
    {
        None, // なし
        Buy, // 購入モード
        Upgrade, // アップグレード
    }
    eSelMode2 _selMode2 = eSelMode2.None;

    // 選択中のオブジェクト
    GameObject _selObj = null;
    GameObject _selObj2 = null;

    // 選択中のタワー
    Tower _selTower = null;
    Tower2 _selTower2 = null;

    // パス
    public static List<Vec2D> _path;
    public static List<Vec2D> _path2;
    // カーソル
    Cursor _cursor;
    Cursor2 _cursor2;
    // コリジョンレイヤー
    Layer2D _lCollision;
    Layer2D _lCollision2;

    // GUI管理
    Gui _gui;
    Gui2 _gui2;
    /// 敵生成管理
    EnemyGenerator _enemyGenerator;
    EnemyGenerator2 _enemyGenerator2;
    // Wave開始演出
    WaveStart _waveStart;
    // 射程範囲
    CursorRange _cursorRange;

    void Start ()
    {
        // ゲームパラメータ初期化
        Global.Init();

        // 敵管理を生成
        Enemy.parent = new TokenMgr<Enemy>("Enemy", 128);
        //Enemy2.parent = new TokenMgr<Enemy2>("Enemy2", 128);
        // ショット管理を生成
        Shot.parent = new TokenMgr<Shot>("Shot", 128);
        // パーティクル管理を生成
        Particle.parent = new TokenMgr<Particle>("Particle", 256);
        // タワー管理を生成
        Tower.parent = new TokenMgr<Tower>("Tower", 64);
        Tower2.parent = new TokenMgr<Tower2>("Tower2", 64);
        // マップ管理を生成
        // プレハブを取得
        GameObject prefab = null;
        prefab = Util.GetPrefab(prefab, "Field");
        // インスタンス生成
        Field field = Field.CreateInstance2<Field>(prefab, 0, 0);
        // マップ読み込み
        field.Load();
        // パスを取得
        _path = field.Path;
        _path2 = field.Path2;

        // コリジョンレイヤーを取得
        _lCollision = field.lCollision;
        _lCollision2 = field.lCollision2;

        // カーソルを取得
        _cursor = GameObject.Find("Cursor").GetComponent<Cursor>();
        _cursor2 = GameObject.Find("Cursor2").GetComponent<Cursor2>();

        // GUIを生成
        _gui = new Gui();
        _gui2 = new Gui2();

        // 敵生成管理を生成
        _enemyGenerator = new EnemyGenerator(_path);
        _enemyGenerator2 = new EnemyGenerator2(_path2);

        // Wave開始演出を取得
        _waveStart = MyCanvas.Find<WaveStart>("TextWaveStart");

        // 射程範囲カーソルを取得する
        _cursorRange = GameObject.Find("CursorRange").GetComponent<CursorRange>();

        // 初期状態は選択しないモード
        ChangeSelMode(eSelMode.None);
        ChangeSelMode2(eSelMode2.None);

        Tower.Add(-1.12f, -1.6f);
        Tower2.Add(1.12f, -1.6f);
    }

    void Update()
    {
        // GUIを更新
        _gui.Update(_selMode, _selTower);
        _gui2.Update2(_selMode2, _selTower2);

        // カーソルを更新
        _cursor.Proc(_lCollision);
        _cursor2.Proc(_lCollision2);

        // メインの更新
        switch (_state)
        {
            case eState.Wait:
            _tWait -= Time.deltaTime;
            if(_tWait < 0)
            {
                _enemyGenerator.Start(Global.Wave);
                _enemyGenerator2.Start(Global.Wave);

                // Wave開始演出を呼び出す (※ここを追加)
                _waveStart.Begin(Global.Wave);
                // メイン状態に遷移する
                _state = eState.Main;
            }
            break;

            case eState.Main:
            // メインの更新
            UpdateMain();

            // ②ゲームオーバーチェック
            if (Global.Life <= 0 || Global.Life2 <= 0 )
            {
                // ③ライフがなくなったのでゲームオーバー
                _state = eState.Gameover;
                // ④ゲームオーバーのUIを表示する
                MyCanvas.SetActive("TextGameover", true);
                break;
            }

            // Waveクリアチェック
            if (IsWaveClear())
            {
                // Waveをクリアした
                // 次のWaveへ
                Global.NextWave();
                // 停止タイマー設定
                _tWait = TIMER_WAIT;
                _state = eState.Wait;
            }

            break;

            case eState.Gameover:
            if (Input.GetMouseButton(0))
            {
                // ⑤やり直し
                SceneManager.LoadScene("MainScene");
            }

            break;
        }
    }

    // 更新・メイン
    void UpdateMain()
    {
        // 敵生成管理の更新
        _enemyGenerator.Update();
        _enemyGenerator2.Update();
        
        // カーソルの下にあるオブジェクトをチェック
        int mask = 1 << LayerMask.NameToLayer("Tower");
        Collider2D col = Physics2D.OverlapPoint(_cursor.GetPosition(), mask);
        _selObj = null;
        if (col != null)
        {
            // 選択中のオブジェクトを格納
            _selObj = col.gameObject;
        }

        // カーソルの下にあるオブジェクトをチェック
        int mask2 = 1 << LayerMask.NameToLayer("Tower");
        Collider2D col2 = Physics2D.OverlapPoint(_cursor2.GetPosition(), mask2);
        _selObj2 = null;
        if (col2 != null)
        {
            // 選択中のオブジェクトを格納
            _selObj2 = col2.gameObject;
        }

        if (Input.GetButtonDown("Done1p")) {
            if (_selObj != null)
            {
                // 砲台をクリックした
                // 選択した砲台オブジェクトを取得
                _selTower = _selObj.GetComponent<Tower>();

                // アップグレードモードに移行する
                ChangeSelMode(eSelMode.Upgrade);
            }
            else
            {
                switch (_selMode)
                {
                    case eSelMode.Buy:
                        if (_cursor.SelObj == null)
                        {
                            if (_cursor.Placeable)
                            {
                                if (_cursor._Placeable)
                                { 
                                    // 所持金を減らす
                                    int cost = Cost.TowerProduction();
                                    if (Global.Money < cost)
                                    {
                                        // お金が足りないので通常モードに戻る
                                        ChangeSelMode(eSelMode.None);
                                    }
                                    else
                                    {
                                        Global.UseMoney(cost);
                                        // 何もないので砲台を配置
                                        Tower.Add(_cursor.X, _cursor.Y);
                                        // 次のタワーの生産コストを取得する
                                        int cost2 = Cost.TowerProduction();
                                        if (Global.Money < cost)
                                        {
                                            // お金が足りないので通常モードに戻る
                                            ChangeSelMode(eSelMode.None);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case eSelMode.None:
                        ChangeSelMode(eSelMode.Buy);
                        break;
                    case eSelMode.Upgrade:
                        ChangeSelMode(eSelMode.None);
                        break;
                }
            }
        }

        if (Input.GetButtonDown("Done2p"))
        {
            if (_selObj2 != null)
            {
                // 砲台をクリックした
                // 選択した砲台オブジェクトを取得
                _selTower2 = _selObj2.GetComponent<Tower2>();

                // アップグレードモードに移行する
                ChangeSelMode2(eSelMode2.Upgrade);
            }
            else
            {
                switch (_selMode2)
                {
                    case eSelMode2.Buy:
                        if (_cursor2.SelObj == null)
                        {
                            if (_cursor2.Placeable)
                            {
                                if (_cursor2._Placeable)
                                {
                                    // 所持金を減らす
                                    int cost = Cost.TowerProduction();
                                    if (Global.Money2 < cost)
                                    {
                                        // お金が足りないので通常モードに戻る
                                        ChangeSelMode2(eSelMode2.None);
                                    }
                                    else
                                    {
                                        Global.UseMoney2(cost);
                                        // 何もないので砲台を配置
                                        Tower2.Add(_cursor2.X, _cursor2.Y);
                                        // 次のタワーの生産コストを取得する
                                        int cost2 = Cost.TowerProduction();
                                        if (Global.Money2 < cost2)
                                        {
                                            // お金が足りないので通常モードに戻る
                                            ChangeSelMode2(eSelMode2.None);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case eSelMode2.None:
                        ChangeSelMode2(eSelMode2.Buy);
                        break;
                    case eSelMode2.Upgrade:
                        ChangeSelMode2(eSelMode2.None);
                        break;
                }
            }
        }

        if (Input.GetButtonDown("Range1p"))
        {
            OnClickRange();
        }

        if (Input.GetButtonDown("Range2p"))
        {
            OnClickRange2();
        }

        if (Input.GetButtonDown("Firerate1p"))
        {
            OnClickFirerate();
        }

        if (Input.GetButtonDown("Firerate2p"))
        {
            OnClickFirerate2();
        }

        if (Input.GetButtonDown("Power1p"))
        {
            OnClickPower2();
        }

        if (Input.GetButtonDown("Power2p"))
        {
            OnClickPower2();
        }
    }

    /// Waveをクリアしたかどうか
    bool IsWaveClear()
    {
    if (_enemyGenerator.Number > 0 || _enemyGenerator2.Number > 0 )
    {
        // 敵がまだ出現する
        return false;
    }

    if (Enemy.parent.Count() > 0)
    {
        // 敵が存在するのでクリアしていない
        return false;
    }

    // クリアした
    return true;
    }

    /// 購入ボタンをクリックした
    public void OnClickBuy()
    {
        // 購入モードにする
        ChangeSelMode(eSelMode.Buy);
    }

    /// 購入ボタンをクリックした2
    public void OnClickBuy2()
    {
        // 購入モードにする
        ChangeSelMode2(eSelMode2.Buy);
    }

    /// アップグレード・射程範囲をクリックした
    public void OnClickRange()
    {
        ExecUpgrade(Tower.eUpgrade.Range);
    }

    /// アップグレード・射程範囲をクリックした
    public void OnClickRange2()
    {
        ExecUpgrade2(Tower2.eUpgrade.Range);
    }

    /// アップグレード・連射速度をクリックした
    public void OnClickFirerate()
    {
        ExecUpgrade(Tower.eUpgrade.Firerate);
    }

    /// アップグレード・連射速度をクリックした2
    public void OnClickFirerate2()
    {
        ExecUpgrade2(Tower2.eUpgrade.Firerate);
    }

    /// アップグレード・攻撃威力をクリックした
    public void OnClickPower()
    {
        ExecUpgrade(Tower.eUpgrade.Power);
    }

    /// アップグレード・攻撃威力をクリックした2
    public void OnClickPower2()
    {
        ExecUpgrade2(Tower2.eUpgrade.Power);
    }

    /// 選択モードの変更
    void ChangeSelMode(eSelMode mode)
    {
        switch (mode)
        {
            case eSelMode.None:
            // 初期状態に戻す
            // 購入ボタンを表示する
            MyCanvas.SetActive("ButtonBuy", true);
            // タワー情報は非表示
            MyCanvas.SetActive("TextTowerInfo", false);
            // 射程範囲を非表示
            _cursorRange.SetVisible(false, 0);
            // アップグレードボタンを非表示
            SetActiveUpgrade(false);
            break;

            case eSelMode.Buy:
            // 購入モード
            // 購入ボタンを非表示にする
            MyCanvas.SetActive("ButtonBuy", false);
            // タワー情報は非表示
            MyCanvas.SetActive("TextTowerInfo", false);
            // 射程範囲を非表示
            _cursorRange.SetVisible(false, 0);
            // アップグレードボタンを非表示
            SetActiveUpgrade(false);
            break;

            case eSelMode.Upgrade:
            // アップグレードモード
            // 購入ボタンを表示する
            MyCanvas.SetActive("ButtonBuy", true);
            // タワー情報を表示
            MyCanvas.SetActive("TextTowerInfo", true);
            // 射程範囲を表示
            _cursorRange.SetVisible(true, _selTower.LvRange);
            _cursorRange.SetPosition(_cursor);
            // アップグレードボタンを表示
            SetActiveUpgrade(true);
            break;
        }
        _selMode = mode;
    }

    /// 選択モードの変更2
    void ChangeSelMode2(eSelMode2 mode)
    {
        switch (mode)
        {
            case eSelMode2.None:
                // 初期状態に戻す
                // 購入ボタンを表示する
                MyCanvas.SetActive("ButtonBuy2", true);
                // タワー情報は非表示
                MyCanvas.SetActive("TextTowerInfo2", false);
                // 射程範囲を非表示
                _cursorRange.SetVisible(false, 0);
                // アップグレードボタンを非表示
                SetActiveUpgrade2(false);
                break;

            case eSelMode2.Buy:
                // 購入モード
                // 購入ボタンを非表示にする
                MyCanvas.SetActive("ButtonBuy2", false);
                // タワー情報は非表示
                MyCanvas.SetActive("TextTowerInfo2", false);
                // 射程範囲を非表示
                _cursorRange.SetVisible(false, 0);
                // アップグレードボタンを非表示
                SetActiveUpgrade2(false);
                break;

            case eSelMode2.Upgrade:
                // アップグレードモード
                // 購入ボタンを表示する
                MyCanvas.SetActive("ButtonBuy2", true);
                // タワー情報を表示
                MyCanvas.SetActive("TextTowerInfo2", true);
                // 射程範囲を表示
                _cursorRange.SetVisible(true, _selTower2.LvRange);
                _cursorRange.SetPosition(_cursor2);
                // アップグレードボタンを表示
                SetActiveUpgrade2(true);
                break;
        }
        _selMode2 = mode;
    }

    /// アップグレードボタンの表示・非表示を切り替え
    void SetActiveUpgrade(bool b)
    {
    // 各種ボタンの表示制御
    MyCanvas.SetActive("ButtonRange", b);
    MyCanvas.SetActive("ButtonFirerate", b);
    MyCanvas.SetActive("ButtonPower", b);
    }

    /// アップグレードボタン2の表示・非表示を切り替え
    void SetActiveUpgrade2(bool b)
    {
    // 各種ボタンの表示制御
    MyCanvas.SetActive("ButtonRange2", b);
    MyCanvas.SetActive("ButtonFirerate2", b);
    MyCanvas.SetActive("ButtonPower2", b);
    }

    /// アップグレードを実行する
    void ExecUpgrade(Tower.eUpgrade type)
    {
    // コストを取得する
    int cost = _selTower.GetCost(type);
    // 所持金を減らす
    Global.UseMoney(cost);

    // アップグレード実行
    _selTower.Upgrade(type);

    // 射程範囲カーソルの大きさを反映
    _cursorRange.SetVisible(true, _selTower.LvRange);
    }

    /// アップグレードを実行する
    void ExecUpgrade2(Tower2.eUpgrade type)
    {
    // コストを取得する
    int cost = _selTower2.GetCost(type);
    // 所持金を減らす
    Global.UseMoney2(cost);

    // アップグレード実行
    _selTower2.Upgrade(type);

    // 射程範囲カーソルの大きさを反映
    _cursorRange.SetVisible(true, _selTower2.LvRange);
    }
}
