using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ゲーム部分以外のSEを管理
public class SystemSEController : MonoBehaviour
{
    public AudioClip[] SystemSE;
    public AudioSource SESource;
    public static SystemSEController Instance;

    public void Awake(){
        if(Instance == null){
            Instance = this;
        }
    }

    void Start()
    {
        SESource = GetComponent<AudioSource>();
    }


    // ゲーム中以外のSEを再生する関数
    public void PlaySystemSE(string Action){
        switch (Action)
        {
            case "Select":
                SESource.PlayOneShot(SystemSE[0]); // 曲, モード選択
                break;
            case "Start":
                SESource.PlayOneShot(SystemSE[1]); // 曲を選択して始める
                break;
            case "Retry":
                SESource.PlayOneShot(SystemSE[2]); // リトライする
                break;
            case "ChangeDiff":
                SESource.PlayOneShot(SystemSE[3]); // 難易度変更
                break;
            case "Clear":
                SESource.PlayOneShot(SystemSE[4]); // ゲームクリア, 歓声
                SESource.PlayOneShot(SystemSE[5]);
                break;
            default:
                break;
        }
    }
    
}
