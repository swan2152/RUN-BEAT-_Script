using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ミスの時の処理
public class MissLineController : MonoBehaviour
{
    float PlayerSpeed = 20f;
    bool Move = true;
    GameObject[] Slimes;
    GameObject[] Turtles;
    GameObject CloseSlimes;
    GameObject CloseTurtles;
    GameObject Player;                          // プレイヤー
    public CharaMove _CharaMove;                // プレイヤー挙動処理
    public ComboController _ComboController;    // コンボ処理
    public JudgeController _JudgeController;    // 判定処理
    public EffectController _EffectController;  // エフェクト処理
    public SEController _SEController;          // 判定表示処理
    public HPController _HPController;          // HP処理
    float[] PosPool = new float[]{-4f, -2f, 0, 2f, 4f};    

    void Start(){
        Player = GameObject.Find("Player");
    }

    // プレイヤーと同じ位置にミスの基準となるラインを追従させる(非表示)
    void Update(){
        float PosZ = PlayerSpeed * Time.deltaTime;
        if(Move){
            transform.position += new Vector3(0f, 0f, PosZ);
        }else{
            transform.position += new Vector3(0f, 0f, 0f);
        }
    }

    // MissLineと敵が重なったらミスの処理を行う
    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Slime" || other.gameObject.tag == "Turtle"){
            float ClosePos = other.transform.position.x;
            Player.GetComponent<CharaMove>().Damage();
            _JudgeController.JudgeOutput(ClosePos, "MISS");
            _EffectController.EffectGenerate("MISS", ClosePos);
            _HPController.HPfluc("MISS");
            _SEController.PlaySE("DAMAGE");
            _ComboController.ComboReset();
            other.gameObject.SetActive(false);
        }
    }

    // プレイヤーが静止したら, MissLineも静止する
    public void StopLine(){
        Move = false;
    }
}
