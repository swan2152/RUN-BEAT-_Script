using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// スタート前に「ARE YOU READY? GO!」の表記を行う
public class StartStringController : MonoBehaviour
{
    public GameObject StartString;
    float CountDown = 5f;           // 文字の表示を管理する時間
    float InitSize = 0.3f;          // 文字の最初のサイズ(徐々に大きくする)
    byte Alpha = 5;                 // 文字の初期の透明度(徐々に不透明にする)
    string[] BeforeStartString = {"ARE YOU READY?", "GO!"}; // 表示する文字

    void Start()
    {
        StartString = GameObject.Find("StartString");
        StartString.transform.localScale = new Vector3(InitSize, InitSize, InitSize);
        StartString.GetComponent<Text>().color = new Color32(255, 96, 96, Alpha);
        StartString.SetActive(false);
    }

    // 表示する文字の管理
    void Update()
    {
        CountDown -= Time.deltaTime;

        // ARE YOU READY? の表示
        if(CountDown <= 4){
            StartString.SetActive(true);
            StartString.GetComponent<Text>().text = BeforeStartString[0];
            if(InitSize < 1f){
                StartString.transform.localScale += new Vector3(0.06f, 0.06f, 0.06f);
                InitSize += 0.06f;
            }
            if(Alpha < 255){
                StartString.GetComponent<Text>().color = new Color32(255, 96, 96, Alpha);
                Alpha += 10;
            }
        }

        // GO! の表示
        if(CountDown <= 2){
            StartString.GetComponent<Text>().text = BeforeStartString[1];
        }

        // GO! の表示を消す
        if(CountDown <= 0.5f){
            StartString.SetActive(false);
        } 
    }
}
