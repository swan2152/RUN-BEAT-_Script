using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEController : MonoBehaviour
{
    public AudioClip[] SE;
    public AudioSource SESource;

    void Start()
    {
        SESource = GetComponent<AudioSource>();
    }

    // ゲーム中のSEを再生する関数
    public void PlaySE(string Action){
        switch (Action)
        {
            case "Attack":
                SESource.PlayOneShot(SE[0]); // 攻撃
                break;
            case "WallDudge":
                SESource.PlayOneShot(SE[1]); // 壁よけ
                break;
            case "DAMAGE":
                SESource.PlayOneShot(SE[2]); // ダメージ・ミス
                break;
            case "SWING":
                SESource.PlayOneShot(SE[3]); // 素振り(敵がいない時の攻撃)
                break;
            default:
                break;
        }
    }
}
