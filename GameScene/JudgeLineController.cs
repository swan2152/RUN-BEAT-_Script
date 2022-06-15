using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 判定ラインの処理（プレイヤーと一緒に動く）
public class JudgeLineController : MonoBehaviour
{
    float PlayerSpeed = 20f;                   // プレイヤー速度
    float JudgeWidth = 2f;                     // 判定幅
    bool Move = true;                          // プレイヤーが動いているかどうか

    // 判定ラインの周りの敵の管理
    GameObject[] Slimes;
    GameObject[] Turtles;
    GameObject closeSlimes;
    GameObject closeTurtles;

    GameObject Player;                         // プレイヤー
    public Transform JudgeLine;                // 判定ライン位置取得
    public ScoreController _ScoreController;   // スコア処理
    public ComboController _ComboController;   // コンボ処理
    public SEController _SEController;         // SE処理
    public JudgeController _JudgeController;   // 判定表示処理
    public EffectController _EffectController; // エフェクト処理
    public HPController _HPController;         // HP処理

    // プレイヤーと同じ速度で判定ラインを動かす
    void Update(){
        Player = GameObject.Find("Player");
        float posZ = PlayerSpeed * Time.deltaTime;
        if(Move){
            transform.position += new Vector3(0f, 0f, posZ);
        }else{
            JudgeLine.transform.position = Player.transform.position + new Vector3(0f, 0f, 2f);
        }
    }

    // キーが押された時の判定
    public void AttackJudge(GameObject[] Enemys, GameObject CloseEnemys, string Name){
        float MaxDist = float.MaxValue;
        Enemys = GameObject.FindGameObjectsWithTag(Name);

        // 敵が判定ラインの近くにいたら処理
        if(Enemys.Length != 0){
            foreach (GameObject i_Enemy in Enemys){
                float Dist = Mathf.Abs(i_Enemy.transform.position.z - JudgeLine.position.z);
                if(Dist < MaxDist){
                    CloseEnemys = i_Enemy;
                    MaxDist = Dist;
                }
            }
            // 一番近い敵の判定を行う
            float ClosePos = CloseEnemys.transform.position.x;

            // PERFECTの処理
            if(MaxDist <= 1.2f){
                NotesProcessing(CloseEnemys, ClosePos, "PERFECT");

            // GREATの処理
            }else if(MaxDist <= 2.4f){
                NotesProcessing(CloseEnemys, ClosePos, "GREAT");

            // 空振りの処理
            }else{
                _SEController.PlaySE("SWING");
            }
        }else{
            // 敵が判定ラインの近くにいなければ空振りのSEを鳴らす
            _SEController.PlaySE("SWING");
        }
    }
    
    // ノーツの処理 スコア・コンボ・SE・HP管理を行う
    void NotesProcessing(GameObject CloseEnemys, float ClosePos, string Judge){
        _ScoreController.ScoreUp(Judge);
        _JudgeController.JudgeOutput(ClosePos, Judge);
        _SEController.PlaySE("Attack");
        _ComboController.ComboPlus();
        _HPController.HPfluc(Judge);
        _EffectController.EffectGenerate(Judge, ClosePos);
        CloseEnemys.SetActive(false);
    }

    // 壁への衝突の判定
    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Wall"){
            float Dist = Mathf.Abs(Player.transform.position.x - other.transform.position.x);
            float ClosePos = other.transform.position.x;

            // もし判定ライン上でプレイヤーと壁の位置が一致していればダメージ処理
            if(Dist <= 0.5f){
                _JudgeController.JudgeOutput(ClosePos, "DAMAGE");
                _ScoreController.ScoreUp("DAMAGE");
                _ComboController.ComboReset();
                _HPController.HPfluc("DAMAGE");
                _EffectController.EffectGenerate("DAMAGE", ClosePos);
                _SEController.PlaySE("DAMAGE");
                Player.GetComponent<CharaMove>().Damage();

            // そうでなければ、避けたことを示す処理
            }else{
                _JudgeController.JudgeOutput(ClosePos, "PASS");
                _EffectController.EffectGenerate("PASS", ClosePos);
                _SEController.PlaySE("WallDudge");
            }
            other.gameObject.SetActive(false);
        }
        
    }
    
    // プレイヤーが止まった時、判定ラインも停止させる
    public void StopLine(){
        Move = false;
    }
    
}
