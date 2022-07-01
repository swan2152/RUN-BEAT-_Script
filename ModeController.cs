using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// モード選択の管理
public class ModeController : MonoBehaviour
{
    // ボタン
    GameObject Play;
    GameObject Tutorial;
    GameObject Back;
    GameObject Quit;
    
    // 音楽
    AudioSource SESource;
    AudioSource SelectMusic;

    void Start()
    {
        Play = GameObject.Find("Play");
        Tutorial = GameObject.Find("Tutorial");
        Back = GameObject.Find("Back");
        Quit = GameObject.Find("Quit");
        SelectMusic = GameObject.Find("SelectMusic").GetComponent<AudioSource>();
        SESource = GameObject.Find("SystemSEController").GetComponent<AudioSource>();
        SelectMusic.volume = PlayerPrefs.GetFloat("GameVolume", 0.4f);
        SESource.volume = PlayerPrefs.GetFloat("SEVolume", 0.7f);
        // OptionController.Instance.GetVolume(SelectMusic, SESource);
    }

    
    // それぞれのモードに遷移
    public void OnClick(int Num){
        switch (Num)
        {
            // ゲームプレイ(曲選択)
            case 0:
                SystemSEController.Instance.PlaySystemSE("Select");
                // SceneManager.LoadScene("Select");
                FadeController.Instance.FadeOutScene("Select");
                break;
            
            // チュートリアル
            case 1:
                SystemSEController.Instance.PlaySystemSE("Select");
                // SceneManager.LoadScene("Tutorial");
                FadeController.Instance.FadeOutScene("Tutorial");
                break;
            
            // 設定
            case 2:
                SystemSEController.Instance.PlaySystemSE("Select");
                // SceneManager.LoadScene("Option");
                FadeController.Instance.FadeOutScene("Option");
                break;

            // タイトルに戻る
            case 3:
                SystemSEController.Instance.PlaySystemSE("Select");
                // SceneManager.LoadScene("Title");
                FadeController.Instance.FadeOutScene("Title");
                break;
            
            // ゲーム終了
            case 4:
                QuitGame();
                break;
            default:
                break;
        }
    }

    // ゲーム終了処理
    public void QuitGame(){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
