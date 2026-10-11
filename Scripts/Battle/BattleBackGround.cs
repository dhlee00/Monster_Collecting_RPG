using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleBackGround : MonoBehaviour
{
    // 배경 이미지
    Image backGround;

    // 잔디 이미지
    public RectTransform partnerSeat;
    public RectTransform enemySeat;

    void Start()
    {
        backGround = GetComponent<Image>();
    }

    void Update()
    {
        // 전투 정보 활성화 전까지
        if (!BattleManager.instance.bt.gameObject.activeSelf)
        {
            // 배경 연출
            if (backGround.fillAmount < 1.0f)
            {
                backGround.fillAmount += 4.0f * Time.deltaTime;
            }

            else if (backGround.fillAmount >= 1.0f)
            {
                partnerSeat.localPosition -= new Vector3(1200 * Time.deltaTime, 0, 0);
                partnerSeat.localPosition =
                    new Vector3(Mathf.Clamp(partnerSeat.localPosition.x, -300.0f, 970.0f), partnerSeat.localPosition.y);

                enemySeat.localPosition += new Vector3(1200 * Time.deltaTime, 0, 0);
                enemySeat.localPosition =
                    new Vector3(Mathf.Clamp(enemySeat.localPosition.x, -990.0f, 298.5f), enemySeat.localPosition.y);

                // 연출이 끝나면 UI 활성화
                if (partnerSeat.localPosition.x <= -300.0f && enemySeat.localPosition.x >= 298.5f)
                {
                    BattleManager.instance.bt.gameObject.SetActive(true);
                }
            }
        }
    }
}
