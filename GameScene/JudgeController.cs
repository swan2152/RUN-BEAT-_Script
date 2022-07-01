using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 判定の表示, 各判定のカウントを行う
public class JudgeController : MonoBehaviour
{
    // 敵の座標を取得({-4.5f, -1.5f, 0, 1.5f, 4.5f})
    GameObject Player;                          // プレイヤー
    GameObject JudgeUI;                         
    GameObject FastorLateUI;                    
    Text UItext;                                // 判定文字
    public GameObject Canvas;
    public Camera GameCamera;                   // ゲーム内のカメラ
    public GameDirector _GameDirector;          // ゲーム全体の管理
    public ComboController _ComboController;    // コンボ管理
    public ScoreController _ScoreController;    // スコア管理
    public AudioSource _AudioSource;

    // 判定の表示に用いる文字
    string[] JudgeSource = {"PERFECT!!", "GREAT!", "PASS!", "MISS...", "DAMAGE!"};
    public string JudgeText;
    public string FastorLateText;

    // 判定文字を表示する箇所
    Vector3[] PosSource = new[]{ 
        new Vector3(-1400f, -600f, 0f), 
        new Vector3(-1100f, -600f, 0f),
        new Vector3(-960f, -600f, 0f),
        new Vector3(-720f, -600f, 0f),
        new Vector3(-490f, -600f, 0f) 
    };
    int PosIndex;

    // ノーツ数, コンボ数, 各判定のカウント
    int TotalNotes = 0;
    int NotesCount = 0;
    int Combo;
    int Score;
    int Perfect = 0;
    int Great = 0;
    int Pass = 0;
    int Damage = 0;
    int Miss = 0;
    bool Flag = false;
     
    
    
    void Start()
    {
        JudgeUI = Resources.Load<GameObject>("Judge");
        FastorLateUI = JudgeUI.transform.Find("FastorLate").gameObject;
        _AudioSource = GameObject.Find("GameMusic").GetComponent<AudioSource>();
    }

    
    void Update()
    {
        // 曲が終わったらリザルトの受け渡しを行う
        if(!_AudioSource.isPlaying && NotesCount > 0 && !Flag){
            SendResult();
        }
    }

    // 総ノーツ数の取得
    public void GetNotesNum(int Notes){
        TotalNotes = Notes;
    }

    // 判定文字の表示
    public void JudgeOutput(float EnemyPos, string Judge){
        // 敵のワールド座標をスクリーン座標に変換
        switch (EnemyPos){
            case -4.5f:
                PosIndex = 0;
                break; 
            case -2.25f:
                PosIndex = 1;
                break; 
            case 0f:
                PosIndex = 2;
                break; 
            case 2.25f:
                PosIndex = 3;
                break; 
            case 4.5f:
                PosIndex = 4;
                break; 
            default:
                break;
        }
        
        // 判定文字の表示
        switch (Judge){
            case "PERFECT":
                JudgeText = JudgeSource[0];
                FastorLateText = "";
                JudgeUI.GetComponent<Text>().color = Color.yellow;
                Perfect++;
                break;
            case "GREAT_FAST":
                JudgeText = JudgeSource[1];
                FastorLateText = "FAST";
                JudgeUI.GetComponent<Text>().color = Color.magenta;
                FastorLateUI.GetComponent<Text>().color = Color.cyan;
                Great++;
                break;
            case "GREAT_LATE":
                JudgeText = JudgeSource[1];
                FastorLateText = "LATE";
                JudgeUI.GetComponent<Text>().color = Color.magenta;
                FastorLateUI.GetComponent<Text>().color = Color.red;
                Great++;
                break;
            case "PASS":
                JudgeText = JudgeSource[2];
                FastorLateText = "";
                JudgeUI.GetComponent<Text>().color = Color.green;
                Pass++;
                break;
            case "MISS":
                JudgeText = JudgeSource[3];
                FastorLateText = "";
                JudgeUI.GetComponent<Text>().color = Color.gray;
                Miss++;
                break;
            case "DAMAGE":
                JudgeText = JudgeSource[4];
                FastorLateText = "";
                JudgeUI.GetComponent<Text>().color = Color.red;
                Damage++;
                break;
            default:
                break;
        }
        JudgeUI.GetComponent<Text>().text = JudgeText.ToString();
        FastorLateUI.GetComponent<Text>().text = FastorLateText.ToString();
        GameObject JudgeUIPrefab = Instantiate(JudgeUI, PosSource[PosIndex], Quaternion.identity);
        JudgeUIPrefab.transform.SetParent(Canvas.transform, false); 
        NotesCount++;
    }

    // 各判定の内訳を取得
    void SendResult(){
        Combo = _ComboController.FinishedCombo();
        Score = _ScoreController.Score;
        _GameDirector.GetResult(Combo, Score, Perfect, Great, Pass, Miss, Damage);
        Flag = true;
    }
}
