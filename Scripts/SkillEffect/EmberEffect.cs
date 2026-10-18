using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmberEffect : MonoBehaviour
{
    // 불꽃세례 이펙트
    public ParticleSystem[] effect;

    // 두번째 이펙트 재생
    bool playSecondEffect;

    private void OnEnable()
    {
        // 효과음 재생
        EMManager.instance.PlayEM("Ember_0", 3, 0.4f);
    }

    void Update()
    {
        // 불쏘는 이펙트 끝나면 타오르는 이펙트 재생
        if (!playSecondEffect && !effect[0].IsAlive())
        {
            // 효과음 재생
            EMManager.instance.PlayEM("Ember_1", 2, 0.5f);

            effect[1].Play();
            playSecondEffect = true;
        }

        // 타오르는 이펙트 끝나고 이펙트 삭제
        if (playSecondEffect && !effect[1].IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
