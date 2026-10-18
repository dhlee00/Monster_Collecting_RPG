using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandScape : MonoBehaviour
{
    // 스프라이트 랜더러
    SpriteRenderer spr;

    // 플레이어 트랜스폼
    Transform playerT;

    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        playerT = GameManager.instance.playerT;
    }

    void Update()
    {
        // 레이어 조절 (플레이어 보다 앞)
        if (playerT.position.y - transform.position.y >= 1.0f)
        {
            spr.sortingOrder = 2;
        }

        // 레이어 조절 (플레이어 보다 뒤)
        else if (playerT.position.y - transform.position.y < 1.0f)
        {
            spr.sortingOrder = 0;
        }
    }
}
