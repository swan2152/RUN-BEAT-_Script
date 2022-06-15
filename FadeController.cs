using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeController : MonoBehaviour
{
    Image FadePanel;                        // フェード用のパネル
    public static FadeController Instance;  // 呼び出し用
    bool IsFadeIn = true;                   // フェードイン中か
    bool IsFadeOut = false;                 // フェードアウト中か
    float Alpha;                            // パネルの透明度
    float FadeSpeed = 0.04f;                // 透明度を変える速さ
    string SetNextScene;                    // 遷移するシーン

    public void Awake(){
        if(Instance == null){
            Instance = this;
        }
    }

    // パネルの表示
    void Start()
    {
        FadePanel = GetComponent<Image>();
        FadePanel.enabled = true;
        Alpha = FadePanel.color.a;
    }

    // 毎フレーム毎にフェード処理を行う
    void Update()
    {
        if(IsFadeIn){
            FadeIn();
        }
        if(IsFadeOut){
            FadeOut();
        }
    }

    // 画面を明るくする
    public void FadeIn(){
        Alpha -= FadeSpeed;
        SetAlpha();
        if(Alpha <= 0){
            IsFadeIn = false;
            FadePanel.enabled = false;
        }
    }

    // 画面を暗くする, 完全に暗くなったら次のシーンに遷移
    public void FadeOut(){
        FadePanel.enabled = true;
        Alpha += FadeSpeed;
        SetAlpha();
        if(Alpha >= 1){
            Debug.Log("Alpha = 1");
            IsFadeOut = false;
            SceneManager.LoadScene(SetNextScene);
        }
    }

    // 遷移するシーンの設定
    public void FadeOutScene(string NextScene){
        FadePanel.enabled = true;
        IsFadeOut = true;
        SetNextScene = NextScene;
    }

    // パネルの透明度の反映
    public void SetAlpha(){
        FadePanel.color = new Color(0, 0, 0, Alpha);
    }
}
