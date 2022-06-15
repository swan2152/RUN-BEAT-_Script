using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSetActive : MonoBehaviour
{
    public float DeleteTime;

    void Start()
    {
        Destroy(gameObject, DeleteTime);
    }

    void Update()
    {
        transform.position += new Vector3(0f, 0f, 20f * Time.deltaTime);
    }
}