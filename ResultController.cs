using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// リザルトの表示
public class ResultController : MonoBehaviour
{
    // リザルトUI
    public GameObject ScoreUI;
    public GameObject ComboUI;
    public GameObject PerfectUI;
    public GameObject GreatUI;
    public GameObject MissUI;
    public GameObject DamageUI;
    public GameObject RankUI;
    public GameObject AchievementUI;
    public GameObject MusicNameUI;
    public GameObject MusicComposerUI;
    public GameObject ClearLampUI;

    AudioClip Audience;
    AudioClip ClearVoice;
    AudioSource ResultBGM;
    AudioSource _AudioSource;

    // 楽曲情報を保存するCSVのパスの取得
    string[] MusicListPass = new string[]{
        "MusicListBASIC.x",
        "MusicListADVANCED.x",
        "MusicListEXPERT.x"
    };

    // クリアランプの種類
    string[] ClearLampSource = new string[]{
        "NOTCLEAR",
        "CLEAR",
        "FULLCOMBO!",
        "PERFECT!!"
    };

    // 保存時に曲名, 作曲者名, スコア, ランク, レベル, パス名を格納する配列
    public string[] SaveMusicName = new string[1000];
    public string[] SaveComposer = new string[1000];
    public int[] SaveScore = new int[1000];
    public string[] SaveRank = new string[1000];
    public int[] SaveLevel = new int[1000];
    public string[] SavePass = new string[1000];
    public float[] SaveAchieve = new float[1000];
    public string[] SaveClearLamp = new string[1000];

    int Combo = 0;
    int Score = 0;
    int Perfect = 0;
    int Great = 0;
    int Miss = 0;
    int Damage = 0;
    float Achievement;
    string Rank;
    string ClearLamp;
    int ClearLampIndex;

    void Start()
    {
        ScoreUI = GameObject.Find("SCORE");
        ComboUI = GameObject.Find("COMBO");
        PerfectUI = GameObject.Find("PERFECT");
        GreatUI = GameObject.Find("GREAT");
        MissUI = GameObject.Find("MISS");
        DamageUI = GameObject.Find("DAMAGE");
        RankUI = GameObject.Find("RANK");
        AchievementUI = GameObject.Find("ACHIEVEMENT");
        MusicNameUI = GameObject.Find("MusicName");
        MusicComposerUI = GameObject.Find("MusicComposer");
        ClearLampUI = GameObject.Find("ClearLamp");
        Audience = GameObject.Find("AudienceVoice").GetComponent<AudioSource>().clip;
        ClearVoice = GameObject.Find("ClearVoice").GetComponent<AudioSource>().clip;
        ResultBGM = GameObject.Find("ResultBGM").GetComponent<AudioSource>();
        ResultBGM.volume = PlayerPrefs.GetFloat("SEVolume", 0.7f);
        _AudioSource = GetComponent<AudioSource>();

        RankCalc();
        PrintResult();
    }

    // モード遷移
    public void onClick(int Num){
        UpdateRecord();
        switch (Num)
        {
            // リトライ
            case 0:
                SystemSEController.Instance.PlaySystemSE("Retry");
                // SceneManager.LoadScene("GameScene");
                FadeController.Instance.FadeOutScene("GameScene");
                break;
            
            // 曲選択
            case 1:
                SystemSEController.Instance.PlaySystemSE("Select");
                // SceneManager.LoadScene("Select");
                FadeController.Instance.FadeOutScene("Select");
                break;
            default:
                break;
        }
    }

    // スコアによるランク計算
    public void RankCalc(){
        Score = GameDirector.GetValue(1);
        Perfect = GameDirector.GetValue(2);
        Great = GameDirector.GetValue(3);
        Miss = GameDirector.GetValue(5);
        int Max = 1000 * (Perfect + Great + Miss);             // スコア理論値(達成率 = スコア/理論値)
        Achievement = ((float)Score / (float)Max) * 100000f;
        int CalcAchievement = (int)Achievement;                   // 小数点第3位未満を丸めるための変数
        Achievement = ((float)CalcAchievement / 1000f);   


        // 達成率によってランクを決める
        if(Achievement >= 95f) Rank = "SSS";
        else if(Achievement >= 92.5f) Rank = "SS";
        else if(Achievement >= 90f) Rank = "S";
        else if(Achievement >= 80f) Rank = "A";
        else if(Achievement >= 70f) Rank = "B";
        else if(Achievement >= 60f) Rank = "C";
        else Rank = "D";

        Debug.Log("Achieve = " + Achievement);
        Debug.Log(Rank);
    }

    // リザルトの表示
    public void PrintResult(){
        ComboUI.GetComponent<Text>().text = GameDirector.GetValue(0).ToString();
        ScoreUI.GetComponent<Text>().text = GameDirector.GetValue(1).ToString();
        PerfectUI.GetComponent<Text>().text = GameDirector.GetValue(2).ToString();
        GreatUI.GetComponent<Text>().text = GameDirector.GetValue(3).ToString();
        MissUI.GetComponent<Text>().text = GameDirector.GetValue(5).ToString();
        DamageUI.GetComponent<Text>().text = GameDirector.GetValue(6).ToString();
        RankUI.GetComponent<Text>().text = Rank;
        AchievementUI.GetComponent<Text>().text = Achievement.ToString();
        PrintClearLamp();

        MusicNameUI.GetComponent<Text>().text = ScrollController.GetStringData("MusicName");
        MusicComposerUI.GetComponent<Text>().text = ScrollController.GetStringData("Composer");
    }

    // クリアランプの表示
    public void PrintClearLamp(){
        if(Miss > 0 || Damage > 0) ClearLampIndex = 1;
        if(Miss == 0 && Damage == 0 && Great > 0) ClearLampIndex = 2;
        if(Miss == 0 && Damage == 0 && Great == 0) ClearLampIndex = 3;
        ClearLamp = ClearLampSource[ClearLampIndex];
        // フルコンボ以上で表示
        if(ClearLampIndex >= 2){
            ClearLampUI.GetComponent<Text>().text = ClearLamp;
        }
    }

    // スコア, 達成率の更新
    public void UpdateRecord(){
        // 書き換えるMusicListを指定 0->BASIC 1->ADVANCED 2->EXPERT
        int Index = ScrollController.GetintData("Diff");
        Debug.Log("Diff = " + ScrollController.GetintData("Diff"));
        Debug.Log("filePass[" + Index + "] = " + MusicListPass[Index]);

        // その譜面の過去のデータを取得
        // string csv = UnityEngine.Application.persistentDataPath + "/" + MusicListPass[index] + ".csv";
        string CSV = Application.streamingAssetsPath + "/" + MusicListPass[Index] + ".csv";
        string GetMusicName = ScrollController.GetStringData("MusicName");
        string GetMusicComposer = ScrollController.GetStringData("Composer");
        int GetScore = ScrollController.GetintData("Score");
        string GetRank = ScrollController.GetStringData("Rank");
        int GetLevel = ScrollController.GetintData("Level");
        string GetPass = ScrollController.GetStringData("Pass");
        string GetClearLamp = ScrollController.GetStringData("ClearLamp");

        StreamReader Reader = new StreamReader(CSV, System.Text.Encoding.GetEncoding("UTF-8"));

        int i = 0;
        int ArraySize = 0; // 楽曲の数

        // 全データを読み込ませてセーブするデータを探索
        while(Reader.Peek() > -1){
            string Line = Reader.ReadLine();
            string[] Values = Line.Split(',');
            for(int j = 0; j < Values.Length; j++){
                string LoadMusicName = Values[0];
                string LoadMusicComposer = Values[1];
                int LoadScore = int.Parse(Values[2]);
                string LoadRank = Values[3];
                int LoadLevel = int.Parse(Values[4]);
                string LoadPass = Values[5];
                float LoadAchieve = float.Parse(Values[6]);
                string LoadClearLamp = Values[7];

                // 更新するインデックスを見つけかつ、スコアを更新していたらその列を新しいスコアとランクに更新する
                if(GetMusicName == LoadMusicName && GetMusicComposer == LoadMusicComposer){
                    if(Score > LoadScore){
                        LoadScore = Score;
                        LoadRank = Rank;
                        LoadAchieve = Achievement;
                        Debug.Log("Searched NewRecord!");
                        Debug.Log("Score = " + LoadScore + ", Rank = " + LoadRank + ", Achieve = " + LoadAchieve);
                    }
                    // クリア以上へ更新
                    if(GetClearLamp == "NOTCLEAR" && ClearLampIndex >= 1){
                        LoadClearLamp = ClearLampSource[ClearLampIndex];
                    }
                    // フルコンボ以上へ更新
                    if(GetClearLamp == "CLEAR" && ClearLampIndex >= 2){
                        LoadClearLamp = ClearLampSource[ClearLampIndex];
                    }
                    // パーフェクトへ更新
                    if(GetClearLamp == "FULLCOMBO!" && ClearLampIndex >= 3){
                        LoadClearLamp = ClearLampSource[ClearLampIndex];
                    }
                }

                SaveMusicName[i] = LoadMusicName;
                SaveComposer[i] = LoadMusicComposer;
                SaveScore[i] = LoadScore;
                SaveRank[i] = LoadRank;
                SaveLevel[i] = LoadLevel;
                SavePass[i] = LoadPass;
                SaveAchieve[i] = LoadAchieve;
                SaveClearLamp[i] = LoadClearLamp;
            }
            i++;
        }

        Reader.Close();
        ArraySize = i;
        // セーブデータを書き込む
        using(StreamWriter Writer = new StreamWriter(CSV, false, System.Text.Encoding.GetEncoding("UTF-8"))){
            for(int k = 0; k < ArraySize; k++){
                string Updateline = SaveMusicName[k] + "," + SaveComposer[k] + "," + 
                                    SaveScore[k].ToString() + "," + SaveRank[k] + "," + 
                                    SaveLevel[k].ToString() + "," + SavePass[k] + "," +
                                    SaveAchieve[k] + "," + SaveClearLamp[k];
                if(k != ArraySize - 1){
                    Writer.WriteLine(Updateline);
                }else{
                    Writer.Write(Updateline);
                }
                Debug.Log(Updateline);
            }
        }

    }
    
}
