using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfoManager : MonoBehaviour
{
    // 어떤 포켓몬의 기술을 확인할지 전달 받을 변수
    [HideInInspector] public Pokemon aboutPokemon;

    // 이미지
    public Image pokemonImg;

    // 이름 텍스트
    public TextMeshProUGUI nameTxt;

    // 기술 이름
    public TextMeshProUGUI[] skillNameTxt;

    // 기술 사용 가능 횟수
    public TextMeshProUGUI[] skillCountTxt;

    // 기술 데미지
    public TextMeshProUGUI[] skillDamageTxt;

    // 기술 명중률
    public TextMeshProUGUI[] skillAccuracyTxt;

    void OnEnable()
    {
        // 이미지 출력
        pokemonImg.sprite = aboutPokemon.image;

        // 이름 출력
        nameTxt.text = aboutPokemon.pokemonName;

        // 초기화
        for (int i = 0; i < 4; i++)
        {
            skillNameTxt[i].text = "";
            skillCountTxt[i].text = "";
            skillDamageTxt[i].text = "";
            skillAccuracyTxt[i].text = "";
        }

        // 기술 정보 출력
        for (int i = 0; i < aboutPokemon.skill.Length; i++)
        {
            skillNameTxt[i].text = $"{aboutPokemon.skill[i].skill_Name}";
            skillCountTxt[i].text = $"{aboutPokemon.skill[i].skill_Count} / {aboutPokemon.skill[i].skill_MaxCount}";
            skillDamageTxt[i].text =
                $"위력 : {Mathf.RoundToInt((aboutPokemon.attack * aboutPokemon.skill[i].skill_Damage) * 0.85f)}";
            skillAccuracyTxt[i].text = $"명중률 : {aboutPokemon.skill[i].skill_Accuracy}%";
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
