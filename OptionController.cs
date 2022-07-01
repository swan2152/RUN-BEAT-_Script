using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionController : MonoBehaviour
{
    public GameVolumeController _GameVolumeController;
    public SEVolumeController _SEVolumeController;
    public static OptionController Instance;

    public void Awake(){
        if(Instance == null){
            Instance = this;
        }
    }

    void Start(){
        _GameVolumeController.SetSlider(PlayerPrefs.GetFloat("GameVolume", 0.4f));
        _SEVolumeController.SetSlider(PlayerPrefs.GetFloat("SEVolume", 0.7f));
        _GameVolumeController.GameVolumeValue = PlayerPrefs.GetFloat("GameVolume", 0.4f);
        _SEVolumeController.SEVolumeValue = PlayerPrefs.GetFloat("SEVolume", 0.7f);
    }

    public void OnClick(int Num){
        switch (Num)
        {
            // モード選択に戻る
            case 0:
                // 音量を保存
                PlayerPrefs.SetFloat("GameVolume", _GameVolumeController.GameVolumeValue);
                PlayerPrefs.SetFloat("SEVolume", _SEVolumeController.SEVolumeValue);

                SystemSEController.Instance.PlaySystemSE("Select");
                // SceneManager.LoadScene("Select");
                FadeController.Instance.FadeOutScene("ModeSelect");
                break;

            // 効果音のテスト
            case 1:
                SystemSEController.Instance.PlaySystemSE("Attack");
                break;
            
            default:
                break;
        }
    }

    // 設定した音量をロード
    public void GetVolume(AudioSource BackGroundMusic, AudioSource SE){
        BackGroundMusic.volume = PlayerPrefs.GetFloat("GameVolume", 0.4f);
        SE.volume = PlayerPrefs.GetFloat("SEVolume", 0.7f);
    }
}
