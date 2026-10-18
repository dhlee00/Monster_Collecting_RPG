using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CureSelect : MonoBehaviour
{
    // 싱글톤
    public static CureSelect instance;

    // 선택 창
    public GameObject selectView;

    // 아이콘
    public GameObject[] selectIcon;

    // 포켓몬 치료 시작
    public bool cureStart;

    // 포켓몬 치료 후
    public bool afterCurePokemon;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (selectView.activeSelf)
        {
            SelectView();
        }
    }

    // 치료 선택창
    void SelectView()
    {
        // 치료 선택
        if (selectIcon[0].activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ChangeIcon(1);
            }

            // 선택하면 치료
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ClickBtn(0);
            }
        }

        // 취소
        else if (selectIcon[1].activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ChangeIcon(0);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ClickBtn(1);
            }
        }
    }

    // 버튼 이벤트에서 호출
    public void ClickBtn(int num)
    {
        // 치료
        if (num == 0)
        {
            // 효과음 재생
            EMManager.instance.PlayEM("Select");
            selectView.SetActive(false);
        }

        // 취소
        else if (num == 1)
        {
            // 효과음 재생
            EMManager.instance.PlayEM("Cancle");
            StartCoroutine(WaitFrame());
        }
    }

    // 이벤트 트리거에서 호출
    public void ChangeIcon(int on)
    {
        // 효과음 재생
        EMManager.instance.PlayEM("Choose");

        for (int i = 0; i < selectIcon.Length; i++)
        {
            selectIcon[i].SetActive(false);
        }

        selectIcon[on].SetActive(true);
    }

    // 대기 함수
    public IEnumerator WaitFrame()
    {
        yield return new WaitForSeconds(0.1f);

        selectView.SetActive(false);
        GameManager.instance.isTalking = false;
    }
}
