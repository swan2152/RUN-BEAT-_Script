using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 曲のプレビュー
public class PreViewController : MonoBehaviour
{
    AudioSource _AudioSource;

    // 曲のプレビューを再生
    public void PreViewMusic(){
        // インデックス(その難易度のリストの中で何曲目か)取得
        GameObject Content = transform.parent.gameObject;
        int Index = Content.transform.GetSiblingIndex();

        int NowIndex = Index;

        // プレビューの実行
        ScrollController.Instance.PlayPreView(Index);

        // Debug.Log(transform.parent.parent.gameObject.name);
    }
}
