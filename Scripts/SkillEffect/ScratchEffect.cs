using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ScratchEffect : MonoBehaviour
{
    // 할퀴기 이펙트
    public ParticleSystem effect;

    private void OnEnable()
    {
        EMManager.instance.PlayEM("Scratch", 2, 0.3f);
    }

    void Update()
    {
        // 이펙트 끝나면 삭제
        if (!effect.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
