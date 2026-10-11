using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBall_Switch : MonoBehaviour
{
    // 포켓몬 이미지
    public GameObject partnerImg;

    // 애니메이션에서 호출
    public void EndSwitch()
    {
        // 효과음 재생
        EMManager.instance.PlayEM("OutBall");

        partnerImg.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
