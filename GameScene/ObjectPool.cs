using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ノーツをオブジェクトプールで管理する
public class ObjectPool : MonoBehaviour
{
    public GameObject Slime;
    public GameObject Turtle;
    public GameObject Wall;
    public GameObject Road;
    public GameObject ErrorObject; // エラー用
    List<GameObject> SlimePool; // スライムの管理
    List<GameObject> TurtlePool; // 亀の管理
    List<GameObject> WallPool; // 壁の管理
    
    // プールの生成 MaxCount = 生成限界数
    public void CreatePool(string Enemy, int MaxCount){
        switch (Enemy)
        {
            case "Slime":
                SlimePool = new List<GameObject>();
                for(int i = 0; i < MaxCount; i++){
                    // オブジェクト生成
                    GameObject Obj = Instantiate(Slime);
                    Obj.SetActive(false);
                    SlimePool.Add(Obj);
                }
                break;
            case "Turtle":
                TurtlePool = new List<GameObject>();
                for(int i = 0; i < MaxCount; i++){
                    // オブジェクト生成
                    GameObject Obj = Instantiate(Turtle);
                    Obj.SetActive(false);
                    TurtlePool.Add(Obj);
                }
                break;
            case "Wall":
                WallPool = new List<GameObject>();
                for(int i = 0; i < MaxCount; i++){
                    // オブジェクト生成
                    GameObject Obj = Instantiate(Wall);
                    Obj.SetActive(false);
                    WallPool.Add(Obj);
                }
                break;
            default:
                break;
        }
    }
    
    // 使うときに場所を指定して表示する
    public GameObject GetObj(string Enemy, Vector3 Pos){
        switch (Enemy)
        {
            case "Slime":
                for(int i = 0; i < SlimePool.Count; i++){
                    if(SlimePool[i].activeSelf == false){
                        GameObject Obj = SlimePool[i];
                        Obj.transform.position = Pos;
                        Obj.SetActive(true);
                        return Obj;
                    }
                }
                GameObject SlimeObj = Instantiate(Slime, Pos, Quaternion.Euler(0, 180, 0));
                SlimeObj.SetActive(true);
                SlimePool.Add(SlimeObj);
                return SlimeObj;
                break;
            case "Turtle":
                for(int i = 0; i < TurtlePool.Count; i++){
                    if(TurtlePool[i].activeSelf == false){
                        GameObject Obj = TurtlePool[i];
                        Obj.transform.position = Pos;
                        Obj.SetActive(true);
                        return Obj;
                    }
                }
                GameObject TurtleObj = Instantiate(Turtle, Pos, Quaternion.Euler(0, 180, 0));
                TurtleObj.SetActive(true);
                TurtlePool.Add(TurtleObj);
                return TurtleObj;
                break;
            case "Wall":
                for(int i = 0; i < WallPool.Count; i++){
                    if(WallPool[i].activeSelf == false){
                        GameObject Obj = WallPool[i];
                        Obj.transform.position = Pos;
                        Obj.SetActive(true);
                        return Obj;
                    }
                }
                GameObject WallObj = Instantiate(Wall, Pos, Quaternion.identity);
                WallObj.SetActive(true);
                WallPool.Add(WallObj);
                return WallObj;
                break;
            /*
            // 実装時のエラーが直らないためまだ未実装
            case "Road":
                // 使ってないものを探す
                for(int i = 0; i < RoadPool.Count; i++){
                    // 使っていなければ
                    if(RoadPool[i].activeSelf == false){
                        GameObject Obj = RoadPool[i];
                        Obj.transform.position = Pos;
                        Obj.SetActive(true);
                        return Obj;
                    }
                }
                // poolの中のものを全部使っていたら、新たに生成
                GameObject RoadObj = Instantiate(Road, Pos, Quaternion.identity);
                RoadObj.SetActive(true);
                RoadPool.Add(RoadObj);
                Debug.Log("Add Road!");
                return RoadObj;
                break;
            */
            default:
                Debug.Log("ERROR!");
                return ErrorObject;
        }
    }
}

