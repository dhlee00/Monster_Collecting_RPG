using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineWhipEffect : MonoBehaviour
{
    // 덩굴채찍 이펙트
    public ParticleSystem[] effect;

    private void OnEnable()
    {
        EMManager.instance.PlayEM("VineWhip", 5, 0.2f);
    }

    void Update()
    {
        // 이펙트 끝나면 삭제
        if (!effect[0].IsAlive() && !effect[1].IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
