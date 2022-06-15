using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// タイトル画面の処理
public class TitleController : MonoBehaviour
{
    GameObject PressEnter;
    public float AlphaChangeTime = 0f;
    // Text AnnounceText;
    Image AnnounceImage;

    void Start()
    {
        PressEnter = GameObject.Find("PRESSENTERLogo");
        // AnnounceText = PressEnter.GetComponent<Text>();
        AnnounceImage = PressEnter.GetComponent<Image>();
    }

    // スペースキーが押されたらゲーム開始
    void Update()
    {
        if (Input.GetKeyDown("return")){
            EnterGame();
        }
        // AnnounceText.color = ChangeAlpha(AnnounceText.color);
        AnnounceImage.color = ChangeAlpha(AnnounceImage.color);
    }

    // モード選択に遷移
    void EnterGame(){
        SystemSEController.Instance.PlaySystemSE("Select");
        // SceneManager.LoadScene("ModeSelect");
        FadeController.Instance.FadeOutScene("ModeSelect");
    }

    // PRESSENTERを点滅させる
    Color ChangeAlpha(Color color){
        AlphaChangeTime += Time.deltaTime * 5.0f;
        color.a = Mathf.Sin (AlphaChangeTime) * 0.25f + 0.75f;
        return color;
    }
}
