using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameVolumeController : MonoBehaviour
{
    public float GameVolumeValue;   // ゲーム内の音量
    public Slider GameVolumeSlider; // 音量調整スライダー
    public AudioSource GameMusic;   // ゲーム内の音楽
    public GameObject VolumeNum;    // 音量の数値のUI

    // Volume を変更して全体の音量を調整する
    void Start()
    {
        GameMusic = GetComponent<AudioSource>();
        VolumeNum = GameObject.Find("GameVolumeNum");
    }

    
    void Update()
    {
        GameVolumeValue = GameVolumeSlider.value;
        GameMusic.volume = GameVolumeValue;
        VolumeNum.GetComponent<Text>().text = ((int)(GameVolumeValue * 100)).ToString();
    }

    public void SetSlider(float LoadValue){
        GameVolumeSlider.value = LoadValue;
    }
}
