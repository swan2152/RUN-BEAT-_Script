using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 判定時のUIの表示の管理
public class UISetActive : MonoBehaviour
{
    public float DeleteTime;
    
    // 一定時間で判定文字が消える
    void Start()
    {
        Destroy(gameObject, DeleteTime);
    }

    // 判定文字が少しずつ上に上がる
    void Update()
    {
        transform.position += new Vector3(0f, 1.5f, 0f);
    }
}
