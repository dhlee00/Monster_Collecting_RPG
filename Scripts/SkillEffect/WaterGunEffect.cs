using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGunEffect : MonoBehaviour
{
    // 물총발사 이펙트
    public ParticleSystem effect;

    private void OnEnable()
    {
        EMManager.instance.PlayEM("WaterGun", 5, 0.2f);
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
