using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboController : MonoBehaviour
{
    GameObject ComboUI;
    GameObject ComboString;

    // 最大コンボ, 現在のコンボ, コンボ表示に用いる変数
    public int MaxCombo = 0;
    public int Combo = 0;
    float NowSize = 0.9f;
    
    void Start()
    {
        ComboUI = GameObject.Find("Combo");
        ComboString = GameObject.Find("ComboString");
    }

    // 3コンボ以上でコンボ表示コンボ上昇のアニメーション
    void Update()
    {
        if(Combo < 3){
            ComboUI.SetActive(false);
            ComboString.SetActive(false);
        }else{
            ComboUI.SetActive(true);
            ComboString.SetActive(true);
        }
        // コンボ上昇のアニメーション
        if(NowSize < 1f){
            ComboUI.transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
            NowSize += 0.05f;
        }
        ComboUI.GetComponent<Text>().text = Combo.ToString();
    }

    // コンボ上昇
    public void ComboPlus(){
        Combo++;
        ComboUI.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        NowSize = 0.9f;
    }

    // コンボのリセット, 最大コンボの更新
    public void ComboReset(){
        if(Combo > MaxCombo){
            MaxCombo = Combo;
        }
        Combo = 0;
    }

    // ゲーム終了時の最大コンボの取得
    public int FinishedCombo(){
        if(Combo > MaxCombo){
            MaxCombo = Combo;
        }
        return MaxCombo;
    }
}