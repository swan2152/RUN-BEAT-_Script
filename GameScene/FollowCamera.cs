using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    Vector3 Vector;             // カメラの位置

    public GameObject Target;   // プレイヤー
    public float FollowSpeed;   // カメラの速度
    bool Move = true;           // プレイヤーが動いているかどうか


    void Start()
    {
        // カメラの初期位置を設定
        transform.position = new Vector3(0f, 5f, -121.5f);
    }

    // プレイヤーに合わせてカメラを動かす
    void Update()
    {
        float PosZ = FollowSpeed * Time.deltaTime;
        if(Move) transform.position += new Vector3(0f, 0f, PosZ);
    }

    // ゲームオーバー時に追従をやめる
    public void Stop(){
        Move = false;
    }
}
