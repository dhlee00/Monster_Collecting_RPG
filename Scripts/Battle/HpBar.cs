using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    // Hp 바 UI
    Image hpBar;

    // Hp 바 스프라이트
    public Sprite[] hpBarImg;

    // 반복
    Coroutine ing;

    void Start()
    {
        hpBar = GetComponent<Image>();
    }

    void Update()
    {
        // 현재 체력 수치에 따라 hp바 색 변경
        if (0.66f <= hpBar.fillAmount)
        {
            hpBar.sprite = hpBarImg[0];
        }

        if (0.33f <= hpBar.fillAmount &&
            hpBar.fillAmount < 0.6f)
        {
            hpBar.sprite = hpBarImg[1];
        }

        if (hpBar.fillAmount < 0.33f)
        {
            // 플레이어 포켓몬의 체력 바인 경우
            if (!GameManager.instance.endBattle &&
                this.gameObject.name == "PartnerHpBar")
            {
                if (ing == null)
                {
                    ing = StartCoroutine(HPWarning());
                }
            }

            hpBar.sprite = hpBarImg[2];
        }
    }

    IEnumerator HPWarning()
    {
        // 효과음 재생
        EMManager.instance.PlayEM("HPWarning");

        yield return new WaitForSeconds(1.2f);

        ing = null;
    }
}
