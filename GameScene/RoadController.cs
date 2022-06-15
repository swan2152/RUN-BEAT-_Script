using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadController : MonoBehaviour
{
    public int Block;        // 予め出しておく区間の数
    public Transform Player; // プレイヤーの座標取得用
    public List<GameObject> RoadList = new List<GameObject>(); // 街の管理
    public GameObject Road;  // 街1区間
    int BlockSize = 20;      // 1区間の大きさ
    int PlayerIndex = 0;     // プレイヤーがいる区間のインデックス
    int NextIndex;           // 次に出す区間のインデックス
    float PlayTime = 0;      // デバッグ用

    // ゲームの背景（街）を生成する
    void Start()
    {
        CreateBackground();
        Application.targetFrameRate = 60;
    }

    // プレイヤーの位置に合わせて新しい区間を生成する
    void Update()
    {
        int Index = (int)(Player.position.z / BlockSize);
        if(Index > 3 && Index > PlayerIndex){
            PlayerIndex = Index;
            BackgroundDestroy();
            BackgroundUpdate();
        }
    }

    // 予め用意する区間の生成
    void CreateBackground(){
        for(int i = 1; i <= Block; i++){
            GameObject RoadPrefab = Instantiate(Road, new Vector3(12.5f, -3f, i*20f), Quaternion.identity);
            RoadList.Add(RoadPrefab);
        }
        NextIndex = Block + 1;
    }

    // 通り過ぎた区間の後片付け
    void BackgroundDestroy(){
        GameObject OldStage = RoadList[0];
        RoadList.RemoveAt(0);
        Destroy(OldStage);
    }

    // 新しい区間の生成
    void BackgroundUpdate(){
        GameObject RoadPrefab = Instantiate(Road, new Vector3(12.5f, -3f, NextIndex*20f), Quaternion.identity);
        RoadList.Add(RoadPrefab);
        NextIndex++;
        
    }
}
