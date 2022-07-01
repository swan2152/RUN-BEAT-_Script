using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SEVolumeController : MonoBehaviour
{
    public float SEVolumeValue;   // 効果音の音量
    public Slider SEVolumeSlider; // 音量調整スライダー
    public AudioSource SESource;  // 効果音
    public GameObject VolumeNum;  // 音量の数値のUI

    // Volume を変更して全体の音量を調整する
    void Start()
    {
        SESource = GetComponent<AudioSource>();
        VolumeNum = GameObject.Find("SEVolumeNum");
    }

    
    void Update()
    {
        SEVolumeValue = SEVolumeSlider.value;
        SESource.volume = SEVolumeValue;
        VolumeNum.GetComponent<Text>().text = ((int)(SEVolumeValue * 100)).ToString();
    }

    public void SetSlider(float LoadValue){
        SEVolumeSlider.value = LoadValue;
    }
}
