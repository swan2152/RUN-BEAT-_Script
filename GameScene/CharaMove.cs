using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaMove : MonoBehaviour
{
    //レーンの移動の数値(左右2レーン + 中央1レーン = 5レーン)
    const int MinLane = -2;
    const int MaxLane = 2;
    const float LaneWidth = 2.0f;

    CharacterController Controller;
    Animator Animator;

    Vector3 MoveDirection = Vector3.zero;   // 方向ベクトル
    int TargetLane;                         // プレイヤーを走らせる位置の指定

    //プレイヤーのパラメーターの設定
    public float Gravity;           // 重力
    public float SpeedZ;            // 奥方向のスピード
    public float SpeedX;            // 横方向のスピード
    // public float SpeedJump;      // ジャンプのスピード(実装予定)
    
    // 敵攻撃判定用
    GameObject[] Slimes;
    GameObject[] Turtles;
    GameObject CloseSlimes;
    GameObject CloseTurtles;
    public Transform JudgeLine;
    
    public JudgeLineController _JudgeLineController; // 判定ライン
    
    void Start()
    {
        Controller = GetComponent<CharacterController>();
        Animator = GetComponent<Animator>();
    }

    // プレイヤーのアクションの実行
    void Update()
    {
        // プレイヤー左移動
        if (Input.GetKeyDown("left")) MoveToLeft();

        // プレイヤー右移動
        if (Input.GetKeyDown("right")) MoveToRight();

        // プレイヤー攻撃実行 (F,J -> スライム, D,K -> カメ)
        if (Input.GetKeyDown("f")) Attack_F();
        if (Input.GetKeyDown("j")) Attack_J();
        if (Input.GetKeyDown("d")) Attack_D();
        if (Input.GetKeyDown("k")) Attack_K();

        MoveDirection.z = SpeedZ;

        // 横方向のベクトルの調整
        float RatioX = (TargetLane * LaneWidth - transform.position.x) / LaneWidth;
        MoveDirection.x = RatioX * SpeedX;

        MoveDirection.y -= Gravity * Time.deltaTime;

        // ベクトルの値をプレイヤーに与える
        Vector3 GlobalDirection = transform.TransformDirection(MoveDirection);
        Controller.Move(GlobalDirection * Time.deltaTime);
        
        if (Controller.isGrounded) MoveDirection.y = 0;

        Animator.SetBool("run", MoveDirection.z > 0.0f);
        
    }

    // 左へ移動
    public void MoveToLeft(){
        if (Controller.isGrounded && TargetLane > MinLane) TargetLane--;
    }

    // 右へ移動
    public void MoveToRight(){
        if (Controller.isGrounded && TargetLane < MaxLane) TargetLane++;
    }

    // スライムへの攻撃
    public void Attack_F(){
        if (Controller.isGrounded) Animator.SetTrigger("Attack");
        _JudgeLineController.AttackJudge(Slimes, CloseSlimes, "Slime");
    }

    public void Attack_J(){
        if (Controller.isGrounded) Animator.SetTrigger("Attack");
        _JudgeLineController.AttackJudge(Slimes, CloseSlimes, "Slime");
    }

    // カメへの攻撃
    public void Attack_D(){
        if (Controller.isGrounded) Animator.SetTrigger("Attack");
        _JudgeLineController.AttackJudge(Turtles, CloseTurtles, "Turtle");
    }

    public void Attack_K(){
        if (Controller.isGrounded) Animator.SetTrigger("Attack");
        _JudgeLineController.AttackJudge(Turtles, CloseTurtles, "Turtle");
    }

    // 壁や敵にぶつかった
    public void Damage(){
        Animator.SetTrigger("damage");
    }

    // ゲームオーバー時、静止
    public void Stop(){
        SpeedZ = 0f;
    }
}