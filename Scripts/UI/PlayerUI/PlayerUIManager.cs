using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    // 플레이어 카드
    public GameObject trainerCard;

    // 포켓몬 목록
    public GameObject pokemonList;

    // 가방
    public GameObject inventory;

    // 현재 표시된 UI 이름
    public TextMeshProUGUI uiNameTxt;

    // UI 페이지
    int uiPage;

    // 설정창
    public GameObject optionView;

    // 첫 활성화 시 초기화
    void Start()
    {
        ChangePage(trainerCard, "트레이너 카드");

        uiPage = 0;
    }

    void Update()
    {
        // 키보드 좌우키 눌렀을 때
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            LeftBtnClick();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            RightBtnClick();
        }
    }

    // 페이지 별 활성화 될 창
    void UIPage()
    {
        switch (uiPage)
        {
            case 0:
                ChangePage(trainerCard, "트레이너 카드");
                break;

            case 1:
                ChangePage(pokemonList, "포켓몬 목록");
                break;

            case 2:
                ChangePage(inventory, "가방");
                break;
        }
    }

    // UI 전환
    void ChangePage(GameObject page, string pageName)
    {
        trainerCard.SetActive(false);
        pokemonList.SetActive(false);
        inventory.SetActive(false);

        page.SetActive(true);
        uiNameTxt.text = pageName;
    }

    // 왼쪽 버튼
    public void LeftBtnClick()
    {
        // 효과음 재생
        EMManager.instance.PlayEM("Choose");

        if (uiPage == 0) uiPage = 2;
        else uiPage--;

        UIPage();
    }

    // 오른쪽 버튼
    public void RightBtnClick()
    {
        // 효과음 재생
        EMManager.instance.PlayEM("Choose");

        if (uiPage == 2) uiPage = 0;
        else uiPage++;

        UIPage();
    }

    // 옵션 버튼
    public void OptionBtn()
    {
        // 효과음 재생
        EMManager.instance.PlayEM("Select");

        optionView.SetActive(true);
    }
}
