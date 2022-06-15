using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 奥行きが約3区間分しか見えないようにする壁の移動(プレイヤーに合わせる)
public class FieldWallController : MonoBehaviour
{
    public Transform Player;

    // プレイヤーと壁の距離
    public Vector3 Offset = new Vector3(0, 0.5f, 60f);

    // 壁の移動
    void Update()
    {
        transform.position = Player.position + Offset;
    }
}
