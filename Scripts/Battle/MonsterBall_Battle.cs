using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBall_Battle : MonoBehaviour
{
    // 파트너 이미지
    public GameObject partnerImg;

    public void PartnerImgAnim()
    {
        // 효과음 재생
        EMManager.instance.PlayEM("OutBall");

        partnerImg.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
