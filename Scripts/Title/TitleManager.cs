using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    // 타이틀
    public GameObject title;

    // 버튼
    public GameObject buttons;

    // 포켓몬
    public GameObject title_Pokemon;

    // 암전
    public GameObject fade;

    // 포켓몬 스프라이트
    public Sprite[] pokemonImg;

    // 포켓몬 번호
    int pokemonNum;

    // 설정창
    public GameObject optionView;

    // 타이틀 배경 애니메이션에서 호출
    public void AppearTitle()
    {
        title.SetActive(true);
    }

    // 타이틀 애니메이션에서 호출
    public void AppearButtons()
    {
        buttons.SetActive(true);
    }

    public void AppearPokemon()
    {
        title_Pokemon.SetActive(true);
    }

    // 타이틀 포켓몬 애니메이션에서 호출
    public void ChangePokemon()
    {
        if (pokemonNum < 4) pokemonNum++;
        else pokemonNum = 0;

        switch (pokemonNum)
        {
            case 0: this.GetComponent<SpriteRenderer>().sprite = pokemonImg[0]; break;
            case 1: this.GetComponent<SpriteRenderer>().sprite = pokemonImg[1]; break;
            case 2: this.GetComponent<SpriteRenderer>().sprite = pokemonImg[2]; break;
            case 3: this.GetComponent<SpriteRenderer>().sprite = pokemonImg[3]; break;
            case 4: this.GetComponent<SpriteRenderer>().sprite = pokemonImg[4]; break;
        }
    }

    // 플레이 버튼
    public void PlayBtnClick()
    {
        // 효과음 재생
        EMManager.instance.PlayEM("Select");

        // 데이터 초기화
        DataManager.ResetData();

        // 페이드 활성화
        fade.SetActive(true);
    }

    // 컨티뉴 버튼
    public void ContinueBtnClick()
    {
        // 효과음 재생
        EMManager.instance.PlayEM("Select");

        // 데이터 불러오기
        DataManager.Load();

        // 페이드 활성화
        fade.SetActive(true);
    }

    // 설정 버튼
    public void OptionBtnClick()
    {
        // 효과음 재생
        EMManager.instance.PlayEM("Select");

        optionView.SetActive(true);
    }

    // 나가기 버튼
    public void QuitBtnClick()
    {
        Application.Quit();
    }
}
