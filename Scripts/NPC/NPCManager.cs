using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    // 스프라이트 랜더러
    SpriteRenderer spr;

    // 플레이어 트랜스폼
    Transform playerT;

    // 스프라이트
    public Sprite[] npcSprite;

    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        playerT = GameManager.instance.playerT;
    }

    void Update()
    {
        // 플레이어가 위치상으로 위에 있다면 레이어 변경
        if (transform.position.y < playerT.position.y)
        {
            spr.sortingOrder = 2;
        }

        else if (transform.position.y > playerT.position.y)
        {
            spr.sortingOrder = 0;
        }
    }

    public void ChangeSprite()
    {
        // 충돌한 대상 쪽 향하기
        // 대상 아래쪽
        if (playerT.gameObject.CompareTag("Dir_Up"))    spr.sprite = npcSprite[0];

        // 대상 위쪽
        if (playerT.gameObject.CompareTag("Dir_Down"))  spr.sprite = npcSprite[1];

        // 대상 왼쪽
        if (playerT.gameObject.CompareTag("Dir_Right")) spr.sprite = npcSprite[2];

        // 대상 오른쪽
        if (playerT.gameObject.CompareTag("Dir_Left"))  spr.sprite = npcSprite[3];
    }
}
