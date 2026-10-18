using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartnerImg_Battle : MonoBehaviour
{
    // 애니메이션에서 호출
    public void SwitchSound()
    {
        // 효과음 재생
        EMManager.instance.PlayEM("Switch");
    }

    public void Switch()
    {
        // 위치 원래대로
        transform.localPosition = new Vector2(0, 165);

        this.gameObject.SetActive(false);
    }

    public void Die()
    {
        // 효과음 재생
        EMManager.instance.PlayEM("Die");

        // 색 원래대로 돌리기
        GetComponent<Image>().color = new Color(1, 1, 1, 1);

        this.gameObject.SetActive(false);
    }

    public void Attack()
    {
        // 효과음 재생
        EMManager.instance.PlayEM("Tackle");
    }
}
