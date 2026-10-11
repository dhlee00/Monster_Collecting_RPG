using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PokemonInfoManager : MonoBehaviour
{
    // 정보 출력할 포켓몬
    [HideInInspector] public Pokemon aboutPokemon;

    // 레벨
    public TextMeshProUGUI printLvTxt;

    // 포켓몬 이름
    public TextMeshProUGUI printNameTxt;

    // 포켓몬 이미지
    public Image pokemonImg;

    // 체력
    public TextMeshProUGUI printHpTxt;

    // 공격력
    public TextMeshProUGUI printAtkTxt;

    // 방어력
    public TextMeshProUGUI printDefTxt;

    // 스피드
    public TextMeshProUGUI printSpdTxt;

    // 현재 경험치
    public TextMeshProUGUI printNowEXPTxt;

    // 레벨업에 필요한 경험치
    public TextMeshProUGUI printNextEXPTxt;

    // 활성화 될 때 마다 출력
    void OnEnable()
    {
        // 초기화
        printLvTxt.text = "";
        printNameTxt.text = "";
        pokemonImg.gameObject.SetActive(true);
        printHpTxt.text = "";
        printAtkTxt.text = "";
        printDefTxt.text = "";
        printSpdTxt.text = "";
        printNowEXPTxt.text = "";
        printNextEXPTxt.text = "";

        // 포켓몬을 선택한 경우 출력
        if (GameManager.instance.selectPartner)
        {
            // 레벨 출력
            printLvTxt.text = $"Lv {aboutPokemon.level}";

            // 이름 출력
            printNameTxt.text = aboutPokemon.pokemonName;

            // 이미지 출력
            pokemonImg.sprite = aboutPokemon.image;

            // 현재 체력, 최대 체력 출력
            printHpTxt.text = $"{Mathf.RoundToInt(aboutPokemon.nowHP)} / {Mathf.RoundToInt(aboutPokemon.maxHP)}";

            // 공격력 출력
            printAtkTxt.text = $"{Mathf.RoundToInt(aboutPokemon.attack)}";

            // 방어력 출력
            printDefTxt.text = $"{Mathf.RoundToInt(aboutPokemon.defence)}";

            // 스피트 출력
            printSpdTxt.text = $"{aboutPokemon.speed}";

            // 현재 경험치 출력
            printNowEXPTxt.text = $"{Mathf.RoundToInt(aboutPokemon.nowEXP)}";

            // 레벨업에 필요한 경험치 출력
            printNextEXPTxt.text = $"{Mathf.RoundToInt(aboutPokemon.needEXP - aboutPokemon.nowEXP)}";
        }

        // 선택하지 않은 경우
        else
        {
            printNameTxt.text = "아직 없음";
            pokemonImg.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // ESC 누르면 창 닫기
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 효과음 재생
            EMManager.instance.PlayEM("Cancle");

            this.gameObject.SetActive(false);
        }
    }
}
