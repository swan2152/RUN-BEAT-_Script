using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
    // シーン間の遷移、ゲームオーバー処理など
    public GameObject GameoverView;
    public GameObject GameoverString;
    public GameObject Retry;
    public GameObject Select;
    public GameObject Player;
    public GameObject Camera;
    public GameObject MissLine;
    public AudioSource _AudioSource;
    public JudgeLineController _JudgeLineController;

    // 判定の内訳, スコア, コンボの格納
    public static int[] Result = new int[7];


    void Start()
    {
        Player = GameObject.Find("Player");
        Camera = GameObject.Find("Main Camera");
        MissLine = GameObject.Find("MissLine");
        _AudioSource = GameObject.Find("GameMusic").GetComponent<AudioSource>();
        if(SceneManager.GetActiveScene().name == "GameScene"){
            GameoverView = GameObject.Find("GameoverView");
            GameoverString = GameObject.Find("GameoverString");
            Retry = GameObject.Find("RETRY");
            Select = GameObject.Find("SELECT");
            GameoverView.SetActive(false);
            GameoverString.SetActive(false);
            Retry.SetActive(false);
            Select.SetActive(false);
        }
    }

    // HPが0になったらプレイヤーの動きを止める(リトライか曲選択かを表示)
    public void Gameover(){
        GameoverView.SetActive(true);
        GameoverString.SetActive(true);
        Retry.SetActive(true);
        Select.SetActive(true);
        _JudgeLineController.StopLine();
        MissLine.GetComponent<MissLineController>().StopLine();
        Player.GetComponent<CharaMove>().Stop();
        Camera.GetComponent<FollowCamera>().Stop();
        _AudioSource.Stop();
    }

    // ゲームクリア処理, フルコンボ、パーフェクトの表示も
    public void GetResult(int Combo, int Score, int Perfect, int Great, int Pass, int Miss, int Damage){
        Result[0] = Combo;
        Result[1] = Score;
        Result[2] = Perfect;
        Result[3] = Great;
        Result[4] = Pass;
        Result[5] = Miss;
        Result[6] = Damage;
        for(int i = 0; i < 7; i++) Debug.Log(Result[i]);
    }

    // リザルト画面へ遷移
    public void LoadResult(){
        // SceneManager.LoadScene("Result");
        FadeController.Instance.FadeOutScene("Result");
    }

    // リザルトの内訳の取得
    public static int GetValue(int i){
        return Result[i];
    }

    // ゲームオーバー時の選択処理
    public void onClick(int Num){
        switch (Num){
            // リトライ
            case 0:
                // SceneManager.LoadScene("GameScene");
                SystemSEController.Instance.PlaySystemSE("Retry");
                FadeController.Instance.FadeOutScene("GameScene");
                break;
            
            // 曲選択
            case 1:
                // SceneManager.LoadScene("Select");
                SystemSEController.Instance.PlaySystemSE("Select");
                FadeController.Instance.FadeOutScene("Select");
                break;
            default:
                break;
        }
    }
}
