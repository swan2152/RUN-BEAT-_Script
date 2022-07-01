using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class ScrollController : MonoBehaviour
{
    public GameObject MusicNode;
    public GameObject Content; // どのScrollViewに入るか指定している
    public int MusicTotal;
    public string filePass;
    public static ScrollController Instance;

    // 曲リストのパス
    public string[] MusicListPass = 
        {"MusicListBASIC.x",
         "MusicListADVANCED.x",
         "MusicListEXPERT.x"};
    
    // 難易度ごとのスクロールビューの取得
    public GameObject BASICView;
    public GameObject ADVANCEDView;
    public GameObject EXPERTView;
    public Transform BASICContent;
    public Transform ADVANCEDContent;
    public Transform EXPERTContent;
    
    // 曲名, 作曲者名, スコア, ランク, レベル, パス名, 達成率, クリアランプを格納する配列
    public string[] MusicName = new string[1000];
    public string[] MusicComposer = new string[1000];
    public int[] Score = new int[1000];
    public string[] Rank = new string[1000];
    public int[] Level = new int[1000];
    public string[] Pass = new string[1000];
    public float[] Achieve = new float[1000];
    public string[] ClearLamp = new string[1000];

    // 曲名, 作曲者名, スコア, ランク, レベル, パス名, 達成率, クリアランプを保存する配列
    public static string[,] MusicNameDB = new string[1000, 3];
    public static string[,] MusicComposerDB = new string[1000, 3];
    public static int[,] ScoreDB = new int[1000, 3];
    public static string[,] RankDB = new string[1000, 3];
    public static int[,] LevelDB = new int[1000, 3];
    public static string[,] PassDB = new string[1000, 3];
    public static float[,] AchieveDB = new float[1000, 3];
    public static string[,] ClearLampDB = new string[1000, 3];

    // 曲名, 作曲者名, スコア, ランク, レベル, クリアランプを表示するUI
    GameObject MusicNameUI;
    GameObject MusicComposerUI;
    GameObject ScoreUI;
    GameObject RankUI;
    GameObject LevelUI;
    GameObject ClearLampUI;

    // ノードの音量を調整するオブジェクト
    AudioSource GameMusicVolume;

    // 曲名, 作曲者名, スコア, ランク, レベル, 達成率, クリアランプのプレビューを表示するUI
    GameObject MusicNamePreView;
    GameObject MusicComposerPreView;
    GameObject ScorePreView;
    GameObject RankPreView;
    GameObject LevelPreView;
    GameObject LevelStringPreView;
    GameObject DiffPreView;
    GameObject AchievePreView;
    // GameObject ClearLampPreView;

    // 難易度を示す文字列
    public string[] Difficult = {"BASIC", "ADVANCED", "EXPERT"};

    // 曲のプレビュー
    AudioSource _AudioSource;
    AudioSource SESource;
    AudioSource SelectMusic;

    // どのノードを指しているか示す(NowIndex -> 何曲目か NowDiff -> Difficult[NowDiff])
    public static int NowIndex = -1;
    public static int NowDiff = 0;

    // ノードの管理
    public List<GameObject> NodeList = new List<GameObject>();

    // 曲のパスを返す変数
    public static string SetPass;
    public static string SetMusicName;
    public static string SetComposer;
    public static string SetRank;
    public static int SetDiff;
    public static int SetScore;
    public static int SetLevel;
    public static string SetClearLamp;

    public void Awake(){
        if(Instance == null){
            Instance = this;
        }
    }

    void Start()
    {
        // どの難易度を表示するかによって、ロードするCSVを決定する
        LoadList(GetPassIndex(Content.name));

        // 音量のロード
        SelectMusic = GameObject.Find("SelectMusic").GetComponent<AudioSource>();
        SESource = GameObject.Find("SystemSEController").GetComponent<AudioSource>();
        SelectMusic.volume = PlayerPrefs.GetFloat("GameVolume", 0.4f);
        SESource.volume = PlayerPrefs.GetFloat("SEVolume", 0.7f);

        // 楽曲情報プレビューUI
        MusicNamePreView = GameObject.Find("MusicNamePreView");
        MusicComposerPreView = GameObject.Find("ComposerPreView");
        ScorePreView = GameObject.Find("ScorePreView");
        RankPreView = GameObject.Find("RankPreView");
        LevelPreView = GameObject.Find("LevelPreView");
        LevelStringPreView = GameObject.Find("LevelStringPreView");
        DiffPreView = GameObject.Find("DiffPreView");
        AchievePreView = GameObject.Find("AchievePreView");
        // ClearLampPreView = GameObject.Find("ClearLampPreView");

        // 楽曲リストUI
        MusicNameUI = MusicNode.transform.Find("MusicName").gameObject;
        MusicComposerUI = MusicNode.transform.Find("Composer").gameObject;
        ScoreUI = MusicNode.transform.Find("Score").gameObject;
        RankUI = MusicNode.transform.Find("RANK").gameObject;
        LevelUI = MusicNode.transform.Find("Level").gameObject;
        ClearLampUI = MusicNode.transform.Find("ClearLamp").gameObject;
        GameMusicVolume = MusicNode.transform.Find("GameMusic").gameObject
            .GetComponent<AudioSource>();

        // 楽曲リストUI生成
        for(int i = 0; i < MusicTotal; i++){
            MusicNameUI.GetComponent<Text>().text = MusicName[i];
            MusicComposerUI.GetComponent<Text>().text = MusicComposer[i];
            ScoreUI.GetComponent<Text>().text = Score[i].ToString();
            RankUI.GetComponent<Text>().text = Rank[i];
            LevelUI.GetComponent<Text>().text = Level[i].ToString();
            ClearLampUI.GetComponent<Text>().text = ClearLamp[i];
            GameMusicVolume.volume = PlayerPrefs.GetFloat("GameVolume", 0.4f);
            GameObject Music = Instantiate(MusicNode, Content.transform) as GameObject;
            Music.name = MusicNode.name;
            NodeList.Add(Music);
        }

        // リストに難易度ごとに色を付ける、配列に楽曲リストのデータを代入
        AddColor(Content.name);
        InsertDB(Content.name);

        // インデックス情報を初期化
        NowIndex = -1;
        NowDiff = 0;

        // /Users/swan1118/UnityProduction/RUN&BEAT!!!/Assets/StreamingAssets
        Debug.Log(Application.streamingAssetsPath);
    }

    // CSVでまとめた楽曲リストをロードする
    void LoadList(int Index){
       // string csv = UnityEngine.Application.persistentDataPath + "/" + MusicListPass[Index] + ".csv";
        string CSV = Application.streamingAssetsPath + "/" + MusicListPass[Index] + ".csv";
        StreamReader Reader = new StreamReader(CSV, System.Text.Encoding.GetEncoding("UTF-8"));

        int i = 0;

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

                MusicName[i] = LoadMusicName;
                MusicComposer[i] = LoadMusicComposer;
                Score[i] = LoadScore;
                Rank[i] = LoadRank;
                Level[i] = LoadLevel;
                Pass[i] = LoadPass;
                Achieve[i] = LoadAchieve;
                ClearLamp[i] = LoadClearLamp;
            }
            i++;
        }
    }

    // CSVでまとめた楽曲リストをロードする(WebGL版)


    // 曲のプレビュー
    public void PlayPreView(int Index){
        SelectMusic.Stop();
        if(NowIndex != -1 && _AudioSource.isPlaying){
            _AudioSource.Stop();
        }
        SystemSEController.Instance.PlaySystemSE("Select");
        LevelStringPreView.SetActive(true);
        _AudioSource = NodeList[Index].transform.Find("GameMusic").gameObject.GetComponent<AudioSource>();
        _AudioSource.clip = Resources.Load("PreViewMusic/" + Pass[Index]) as AudioClip;
        _AudioSource.Play();

        // 楽曲情報のプレビュー
        MusicNamePreView.GetComponent<Text>().text = MusicNameDB[Index, NowDiff];
        MusicComposerPreView.GetComponent<Text>().text = MusicComposerDB[Index, NowDiff];
        ScorePreView.GetComponent<Text>().text = ScoreDB[Index, NowDiff].ToString();
        RankPreView.GetComponent<Text>().text = RankDB[Index, NowDiff];
        LevelPreView.GetComponent<Text>().text = LevelDB[Index, NowDiff].ToString();
        DiffPreView.GetComponent<Text>().text = Difficult[NowDiff];
        AchievePreView.GetComponent<Text>().text = AchieveDB[Index, NowDiff].ToString() + "%";
        ChangeColor(NowDiff);
        
        NowIndex = Index;
    }

    // ゲームシーンに遷移, 楽曲情報を受け渡す
    public void LoadGameScene(){
        if(NowIndex != -1){
            SetPass = Pass[NowIndex];
            SetDiff = NowDiff;
            SetMusicName = MusicNameDB[NowIndex, NowDiff];
            SetComposer = MusicComposerDB[NowIndex, NowDiff];
            SetScore = ScoreDB[NowIndex, NowDiff];
            SetRank = RankDB[NowIndex, NowDiff];
            SetLevel = LevelDB[NowIndex, NowDiff];
            SetClearLamp = ClearLampDB[NowIndex, NowDiff];
            SystemSEController.Instance.PlaySystemSE("Start");
            // SceneManager.LoadScene("GameScene");
            FadeController.Instance.FadeOutScene("GameScene");
        }
    }

    // string型のデータを返す関数
    public static string GetStringData(string Data){
        switch (Data)
        {
            case "Pass":
                return SetPass;
                break;
            case "MusicName":
                return SetMusicName;
                break;
            case "Composer":
                return SetComposer;
                break;
            case "Rank":
                return SetRank;
                break;
            case "ClearLamp":
                return SetClearLamp;
                break;
            default:
                return "Error";
                break;
        }
    }

    // int型のデータを返す関数
    public static int GetintData(string data){
        switch (data)
        {
            case "Diff":
                return SetDiff;
                break;
            case "Score":
                return SetScore;
                break;
            case "Level":
                return SetLevel;
            default:
                return -1;
                break;
        }
    }

    // 表示する難易度のリストを切り替える
    public void SwitchDiff(int Num){
        switch (Num)
        {
            // BASICへ遷移
            case 0:
                BASICView.GetComponent<RectTransform>().anchoredPosition = new Vector3(-360f, 140f, 0f);
                ADVANCEDView.GetComponent<RectTransform>().anchoredPosition = new Vector3(-1560f, 140f, 0f);
                EXPERTView.GetComponent<RectTransform>().anchoredPosition = new Vector3(-1560f, 140f, 0f);
                NowDiff = 0;
                break;

            // ADVANCEDへ遷移
            case 1:
                BASICView.GetComponent<RectTransform>().anchoredPosition = new Vector3(-1560f, 140f, 0f);
                ADVANCEDView.GetComponent<RectTransform>().anchoredPosition = new Vector3(-360f, 140f, 0f);
                EXPERTView.GetComponent<RectTransform>().anchoredPosition = new Vector3(-1560f, 140f, 0f);
                NowDiff = 1;
                break;
            
            // EXPERTへ遷移
            case 2:
                BASICView.GetComponent<RectTransform>().anchoredPosition = new Vector3(-1560f, 140f, 0f);
                ADVANCEDView.GetComponent<RectTransform>().anchoredPosition = new Vector3(-1560f, 140f, 0f);
                EXPERTView.GetComponent<RectTransform>().anchoredPosition = new Vector3(-360f, 140f, 0f);
                NowDiff = 2;
                break;
            default:
                break;
        }
        SystemSEController.Instance.PlaySystemSE("ChangeDiff");
    }

    // 曲リストの色付け
    public void AddColor(string ContentName){
        switch (ContentName)
        {
            case "BASICContent":
                foreach (Transform i_Node in BASICContent){
                    if(i_Node.tag == "MusicNode"){
                        i_Node.GetComponent<Image>().color = new Color32 (128, 255, 128, 200);
                    }
                }
                break;
            case "ADVANCEDContent":
                foreach (Transform i_Node in ADVANCEDContent){
                    if(i_Node.tag == "MusicNode"){
                        i_Node.GetComponent<Image>().color = new Color32 (255, 200, 100, 200);
                    }
                }
                break;
            case "EXPERTContent":
                foreach (Transform i_Node in EXPERTContent){
                    if(i_Node.tag == "MusicNode"){
                        i_Node.GetComponent<Image>().color = new Color32 (255, 128, 88, 200);
                    }
                }
                break;
            default:
                break;
        }
    }

    // 難易度によってフォントの色を変化させる
    public void ChangeColor(int NowDiff){
        switch (NowDiff)
        {
            case 0:
                DiffPreView.GetComponent<Text>().color = new Color32 (0, 160, 0, 255);
                break;
            case 1:
                DiffPreView.GetComponent<Text>().color = new Color32 (255, 128, 0, 255);
                break;
            case 2:
                DiffPreView.GetComponent<Text>().color = new Color32 (255, 128, 88, 255);
                break;
            default:
                break;
        }
    }

    // 2次元配列に楽曲の情報を記録する
    public void InsertDB(string ContentName){
        int j = 0;
        switch (ContentName){
            case "BASICContent":
                break;
            case "ADVANCEDContent":
                j = 1;
                break;
            case "EXPERTContent":
                j = 2;
                break;
            default:
                break;
        }
        for(int i = 0; i < MusicTotal; i++){
            MusicNameDB[i, j] = MusicName[i];
            MusicComposerDB[i, j] = MusicComposer[i];
            ScoreDB[i, j] = Score[i];
            RankDB[i, j] = Rank[i];
            LevelDB[i, j] = Level[i];
            AchieveDB[i, j] = Achieve[i];
            ClearLampDB[i, j] = ClearLamp[i];
        }
        
    }

    // 楽曲リストのパスのインデックスを返す
    public int GetPassIndex(string ContentName){
        switch (ContentName){
            case "BASICContent":
                return 0;
                break;
            case "ADVANCEDContent":
                return 1;
                break;
            case "EXPERTContent":
                return 2;
                break;
            default:
                return -1;
                break;
        }
    }

    // モード選択へ戻る
    public void BackModeSelect(){
        SystemSEController.Instance.PlaySystemSE("Select");
        // SceneManager.LoadScene("ModeSelect");
        FadeController.Instance.FadeOutScene("ModeSelect");
    }
}
