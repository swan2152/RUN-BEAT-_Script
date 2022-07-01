using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// スコアの管理を行う
public class ScoreController : MonoBehaviour
{
    GameObject ScoreUI;

    public int Score = 0;

    void Start()
    {
        ScoreUI = GameObject.Find("Score");
    }

    // ゲーム中のスコアの表示
    void Update()
    {
        ScoreUI.GetComponent<Text>().text = Score.ToString();
    }

    // 判定に応じてスコアを管理する
    public void ScoreUp(string Judge){
        switch (Judge)
        {
            case "PERFECT":
                Score += 1000;
                break;
            case "GREAT_FAST":
                Score += 500;
                break;
            case "GREAT_LATE":
                Score += 500;
                break;
            case "DAMAGE":
                Score -= 250;
                if(Score < 0){
                    Score = 0;
                }
                break;
            default:
                break;
        }
    }

}
