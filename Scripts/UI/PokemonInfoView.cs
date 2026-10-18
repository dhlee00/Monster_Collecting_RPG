using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PokemonInfoView : MonoBehaviour
{
    // 몬스터볼 매니저
    [HideInInspector] public MBManager mbm;

    // 선택된 몬스터
    [HideInInspector] public string pokemonName;

    // 이미지
    public Image pokemonImg;

    // 정보 텍스트
    public TextMeshProUGUI pokemonInfo;

    // 스타팅 포켓몬
    Pokemon bulbasaur;   // 이상해씨
    Pokemon charmander;  // 파이리
    Pokemon squirtle;    // 꼬부기

    void Start()
    {
        bulbasaur = Resources.Load<Pokemon>("Pokemon/Bulbasaur");
        charmander = Resources.Load<Pokemon>("Pokemon/Charmander");
        squirtle = Resources.Load<Pokemon>("Pokemon/Squirtle");
    }

    void Update()
    {
        switch (pokemonName)
        {
            case "Bulbasaur_MB":
                pokemonImg.sprite = bulbasaur.image;
                pokemonInfo.text = $"{bulbasaur.pokemonName}\n\n" +
                                   $"풀타입 포켓몬\n\n" +
                                   $"체    력 : {bulbasaur.maxHP}\n" +
                                   $"공격력 : {bulbasaur.attack}\n" +
                                   $"방어력 : {bulbasaur.defence}\n" +
                                   $"스피드 : {bulbasaur.speed}";
                break;

            case "Charmander_MB":
                pokemonImg.sprite = charmander.image;
                pokemonInfo.text = $"{charmander.pokemonName}\n\n" +
                                   $"불타입 포켓몬\n\n" +
                                   $"체    력 : {charmander.maxHP}\n" +
                                   $"공격력 : {charmander.attack}\n" +
                                   $"방어력 : {charmander.defence}\n" +
                                   $"스피드 : {charmander.speed}";
                break;

            case "Squirtle_MB":
                pokemonImg.sprite = squirtle.image;
                pokemonInfo.text = $"{squirtle.pokemonName}\n\n" +
                                   $"물타입 포켓몬\n\n" +
                                   $"체    력 : {squirtle.maxHP}\n" +
                                   $"공격력 : {squirtle.attack}\n" +
                                   $"방어력 : {squirtle.defence}\n" +
                                   $"스피드 : {squirtle.speed}";
                break;
        }
    }

    // 선택 버튼
    public void SelectBtnClick()
    {
        // 선택된 포켓몬 획득
        switch (pokemonName)
        {
            case "Bulbasaur_MB":
                MyPokemon.pokemon.Add(DataLoader.instance.GetPokemon("이상해씨"));
                break;

            case "Charmander_MB":
                MyPokemon.pokemon.Add(DataLoader.instance.GetPokemon("파이리"));
                break;

            case "Squirtle_MB":
                MyPokemon.pokemon.Add(DataLoader.instance.GetPokemon("꼬부기"));
                break;
        }

        // 포켓몬 선택한 상태
        GameManager.instance.selectPartner = true;

        CloseBtnClick();
    }

    // 닫기 버튼
    public void CloseBtnClick()
    {
        // 효과음 재생
        if (GameManager.instance.selectPartner)
        {
            EMManager.instance.PlayEM("Select");
        }

        else
        {
            EMManager.instance.PlayEM("Cancle");
        }

        mbm.CloseBall();
        mbm = null;
        pokemonName = "";
        pokemonImg.sprite = null;
        pokemonInfo.text = "";
        GameManager.instance.cantDoing = false;
        this.gameObject.SetActive(false);
    }
}
