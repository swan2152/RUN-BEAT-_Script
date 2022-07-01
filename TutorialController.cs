using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// チュートリアルに用いるテキストの再生
public class TutorialController : MonoBehaviour
{
    [TextArea] public string[] RuleTexts; // 全てのテキストを格納する
    int Index = -1;                       // 何番目のテキストかを示す
    string RuleText;                      // テキストを格納する
    int TextCharIndex;                    // テキストが何文字目かを示す
    float Intaval = 10.0f;                // テキストの間隔
    float Offset = 6.0f;                  // テキスト再生のずれの修正
    int TextSpeed = 0;                    // テキストの速度
    AudioSource MessageSource;            // テキストを流す時のSE
    public AudioClip MessageSE;         
    bool Called = false;                  // テキストが開始されたか
    float ElapsedTime = 0.0f;

    // 音量のロード
    AudioSource SESource;
    AudioSource GameMusic;

    // 新しいテキストを再生するタイミング
    float[] TimeTrigger = {6f, 11f, 16f, 21f, 31f, 36f, 46f, 51f, 61f, 71f};


    void Start()
    {
        MessageSource = GameObject.Find("MessageSE").GetComponent<AudioSource>();
        MessageSource.volume = PlayerPrefs.GetFloat("SEVolume", 0.7f);
    }

    void Update()
    {
        ElapsedTime += Time.deltaTime;
        // テキストの速度を調整
        TextSpeed++;
        if(TextSpeed % 3 == 0){
            if(Index > -1){
                if(TextCharIndex != RuleTexts[Index].Length){
                    RuleText = RuleText + RuleTexts[Index][TextCharIndex];
                    TextCharIndex = TextCharIndex + 1;
                    MessageSource.PlayOneShot(MessageSE);
                }else{
                    // 次のテキストに移動
                    if(Index != RuleTexts.Length - 1){
                        if(ElapsedTime > TimeTrigger[Index+1]){
                            Debug.Log(ElapsedTime);
                            RuleText = "";
                            TextCharIndex = 0;
                            Index = Index + 1;
                        }
                    }
                }
                this.GetComponent<Text>().text = RuleText;
            }
        }
        // テキストの再生のタイミングの調整
        if(ElapsedTime > 6f && !Called){
            Index = 0;
            Called = true;
            Debug.Log(ElapsedTime);
        }
    }
}
