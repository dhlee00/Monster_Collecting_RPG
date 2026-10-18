using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro_MonsterBall : MonoBehaviour
{
    // 인트토 피카츄
    public GameObject intro_Pikachu;

    // 인트로 몬스터볼 애니메이션에서 호출
    public void SpawnPikachu()
    {
        // 효과음 재생
        EMManager.instance.PlayEM("OutBall");

        intro_Pikachu.SetActive(true);
        Destroy(gameObject);
    }
}
