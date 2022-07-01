using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

// 時間指定して、3秒前にインスタンスする？
public class NotesGenerate : MonoBehaviour
{
    // ノーツのタイミング, 位置, 種類を格納する
    float[] Timing;
    int[] Pos;
    int[] Enemy;

    public string FilePass;        // ファイルのパス
    public int Diff;               // 難易度

    // 難易度毎のパス
    public string[] DiffList = {"BASIC/", "ADVANCED/", "EXPERT/"};
    int NotesCount = 0;           // ノーツカウント用
    int NotesNum = 0;             // ノーツ数

    public AudioSource _AudioSource;
    float StartTime = 0;

    public float TimeOffset = -3;   // ノーツ生成時とプレイヤーの位置のズレの修正
    float StartOffset;              // スタート時のズレ(プレイヤーが動き出すまで)

    bool IsPlaying = false;        // ノーツの生成が始まったか
    bool MusicStartFlag = false;   // 楽曲が再生されたか

    public ObjectPool _ObjectPool;
    public JudgeController _JudgeController;
    public GameDirector _GameDirector;

    GameObject Slime;
    GameObject Turtle;
    GameObject Wall;

    TextAsset CSV;

    public GameObject JudgeLine; // ゲーム開始検知用

    void Start()
    {
        // 通常のゲームの場合、パスを取得する(チュートリアルの場合は直接指定)
        if(SceneManager.GetActiveScene().name == "GameScene"){
            FilePass = ScrollController.GetStringData("Pass");
            Diff = ScrollController.GetintData("Diff");
            _AudioSource = GameObject.Find("GameMusic").GetComponent<AudioSource>();
            _AudioSource.clip = Resources.Load("GameMusic/" + FilePass) as AudioClip;
        }else{
            _AudioSource = GameObject.Find("GameMusic").GetComponent<AudioSource>();
            _AudioSource.clip = Resources.Load("GameMusic/Tutorial") as AudioClip;
        }
            _AudioSource.volume = PlayerPrefs.GetFloat("GameVolume", 0.4f);
            JudgeLine = GameObject.Find("JudgeLine");
            Timing = new float[1024];
            Pos = new int[1024];
            Enemy = new int[1024];

        // オブジェクトプール生成
        _ObjectPool.CreatePool("Slime", 30);
        _ObjectPool.CreatePool("Turtle", 30);
        _ObjectPool.CreatePool("Wall", 30);

        // 敵のロード
        Slime = Resources.Load<GameObject>("Slime");
        Turtle = Resources.Load<GameObject>("Turtle");
        Wall = Resources.Load<GameObject>("Wall");
    }

    void Update()
    {
        if(IsPlaying){
            CheckNextNotes();
        }else{
            CheckStartGame();
        }

        // 判定ラインのz座標が0になったら楽曲を再生(譜面のスタート地点)
        if(JudgeLine.transform.position.z >= 0 && !_AudioSource.isPlaying && !MusicStartFlag){
            _AudioSource.Play();
            MusicStartFlag = true;
        }

        // 曲が終了したらリザルト画面に遷移する(チュートリアルの場合はモード選択)
        if(!_AudioSource.isPlaying && NotesCount > 0 && HPController.instance.HP > 0
            && JudgeLine.transform.position.z > 0){
            if(SceneManager.GetActiveScene().name == "GameScene"){
                _GameDirector.LoadResult();
            }else{
                // SceneManager.LoadScene("ModeSelect");
                FadeController.Instance.FadeOutScene("ModeSelect");
            }
        }
    }

    void CheckStartGame(){
        // 楽曲開始3秒前からノーツの生成を行っておく
        if(JudgeLine.transform.position.z >= -60){
            IsPlaying = true;
            LoadCSV();
        }
    }

    // CSVの譜面データの読み込み
    void LoadCSV(){
        // DiffList = 難易度, filePass = 譜面
        // チュートリアルか通常のゲームかを区別
        if(SceneManager.GetActiveScene().name == "GameScene"){
            CSV = Resources.Load("CSV/" + DiffList[Diff] + FilePass) as TextAsset;
            Debug.Log("CSV/" + DiffList[Diff] + FilePass);
        }else{
            CSV = Resources.Load("CSV/Tutorial") as TextAsset;
        }
        StringReader Reader = new StringReader(CSV.text);

        int i = 0;

        // 配列にタイミング, 位置, 種類を記録
        while(Reader.Peek() > -1){
            string Line = Reader.ReadLine();
            string[] Values = Line.Split(',');
            for(int j = 0; j < Values.Length; j++){
                if(Values[0] != "Notes"){
                    float LoadTiming = float.Parse(Values[0]);
                    int LoadPos = int.Parse(Values[1]);
                    Timing[i] = LoadTiming;
                    Pos[i] = LoadPos;
                    Enemy[i] = int.Parse(Values[2]);
                }else{
                    // 総ノーツ数を受け渡す
                    NotesNum = int.Parse(Values[1]);
                    _JudgeController.GetNotesNum(NotesNum);
                }
            }
            i++;
        }
    }

    // ノーツの生成(生成する時間地点, ノーツの位置, 種類)
    void SpawnNotes(float FormTiming, int FormPos, int FormEnemy){
        float[] PosPool = new float[]{-4.5f, -2.25f, 0, 2.25f, 4.5f};
        if(FormEnemy == 0){
            // Instantiate(Slime);
            _ObjectPool.GetObj("Slime", new Vector3(PosPool[FormPos], 0, FormTiming*20f));
        }else if(FormEnemy == 1){
            // Instantiate(Turtle);
            _ObjectPool.GetObj("Turtle", new Vector3(PosPool[FormPos], 0, FormTiming*20f));
        }else{
            // Instantiate(Wall);
            _ObjectPool.GetObj("Wall", new Vector3(PosPool[FormPos], 0, FormTiming*20f));
        }
    }

    // ノーツを生成する時間になったら次のノーツを生成
    void CheckNextNotes(){
        if(Timing[NotesCount] + TimeOffset < GetMusicTime () && Timing [NotesCount] != 0){
            SpawnNotes (Timing[NotesCount], Pos[NotesCount], Enemy[NotesCount]);
            NotesCount++;
        }
    }

    // 再生時間の取得
    float GetMusicTime(){
        return Time.time - StartTime;
    }

}
