using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// HP, HPゲージの管理
public class HPController : MonoBehaviour
{
    public GameObject HPUI;                 // HPの値のUI
    public GameObject HPGauge;              // HPのゲージのUI
    public GameDirector _GameDirector;      // ゲーム全体の管理
    public static HPController instance;
    public float HP = 1000;                 // HP
    float MaxHP = 1000;                     // 最大HP
    float HpRatio = 1.0f;                  // HPゲージの幅

    public void Awake(){
        if(instance == null){
            instance = this;
        }
    }

    void Start()
    {
        HPUI = GameObject.Find("HP");
        HPGauge = GameObject.Find("HPGauge");
        HPGauge.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
    }

    // HP, HPゲージの更新
    void Update()
    {
        HpRatio = HP / MaxHP;

        // ライフが0以下になったらゲームオーバー(チュートリアル除く)
        if(HP <= 0){
            HP = 0;
            if(SceneManager.GetActiveScene().name == "GameScene"){
                _GameDirector.Gameover();
            }
        }
        // ライフの最大値 = 1000?(未確定)
        if(HP >= 1000){
            HP = 1000;
            HpRatio = 1.0f;
        }
        
        // HP残量に応じてHPゲージの色を変化させる
        ColorChange(HpRatio);
        HPGauge.GetComponent<Image>().fillAmount = HpRatio;
        HPUI.GetComponent<Text>().text = HP.ToString();
    }

    // HPの管理
    public void HPfluc(string State){
        switch (State)
        {
            case "PERFECT":
                HP += 3;
                break;
            case "GREAT":
                HP += 1;
                break;
            case "MISS":
                HP -= 40;
                break;
            case "DAMAGE":
                HP -= 80;
                break;
            default:
                break;
        }
    }

    // HPゲージの色の管理(HPが少ないほど緑から赤へ変わる)
    public void ColorChange(float Ratio){
        if(Ratio >= 0.5){
            HPGauge.GetComponent<Image>().color = new Color32((byte)(255 - ((510*Ratio) - 255)), 255, 0, 255);
        }else{
            HPGauge.GetComponent<Image>().color = new Color32(255, (byte)((256*Ratio) + 63), 0, 255);
        }
    }
}
