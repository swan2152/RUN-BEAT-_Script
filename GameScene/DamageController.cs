using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ダメージ時に画面を赤くする
public class DamageController : MonoBehaviour
{
    public Image DamageImg;

    void Start()
    {
        DamageImg = GetComponent<Image>();
    }

    public void DamageGenerate(){
        DamageImg.color = new Color (0.5f, 0f, 0f, 0.5f);
        DamageImg.color = Color.Lerp (DamageImg.color, Color.clear, Time.deltaTime);
    }

}
