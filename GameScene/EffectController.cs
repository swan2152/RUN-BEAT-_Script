using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 攻撃成功時, ダメージ時のエフェクトの表示,
public class EffectController : MonoBehaviour
{
    public GameObject[] Effect;
    public GameObject JudgeLine;
    public Image DamageImg;
    GameObject DamageImgObject;
    

    void Start(){
        JudgeLine = GameObject.Find("JudgeLine");
        DamageImgObject = GameObject.Find("DamageImage");
        DamageImg = DamageImgObject.GetComponent<Image>();
    }

    // ダメージを受けた後、色を元に戻す
    void Update(){
        if(DamageImg.color != Color.clear){
            DamageImg.color = Color.Lerp(DamageImg.color, Color.clear, Time.deltaTime*3);
        }
    }

    // エフェクト生成
    public void EffectGenerate(string GenerateEffect, float EnemyPos){
        float JudgeLinePos = JudgeLine.transform.position.z;
        switch (GenerateEffect)
        {
            case "PERFECT":
                Instantiate(Effect[0], new Vector3(EnemyPos, 0.5f, JudgeLinePos), Quaternion.identity);
                break;
            case "GREAT":
                Instantiate(Effect[1], new Vector3(EnemyPos, 0.5f, JudgeLinePos), Quaternion.identity);
                break;
            case "PASS":
                Instantiate(Effect[2], new Vector3(EnemyPos, 0.5f, JudgeLinePos), Quaternion.identity);
                break;
            case "MISS":
                DamageImg.color = new Color (0.5f, 0f, 0f, 0.5f);
                break;
            case "DAMAGE":
                DamageImg.color = new Color (0.5f, 0f, 0f, 0.5f);
                break;
            default:
                break;
        }
    }
}
