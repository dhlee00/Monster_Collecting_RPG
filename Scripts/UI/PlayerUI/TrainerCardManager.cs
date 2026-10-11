using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrainerCardManager : MonoBehaviour
{
    // 출력할 텍스트들
    // 아이디 넘버
    public TextMeshProUGUI printIdNumTxt;

    // 이름
    public TextMeshProUGUI printNameTxt;

    // 파트너 포켓몬
    public TextMeshProUGUI printPartnerTxt;
    public PokemonIcon partnerIcon;

    // 소지금
    public TextMeshProUGUI printMoneyTxt;

    // 전적
    public TextMeshProUGUI printRecordTxt;

    // 활성화 될 때 마다 출력
    void OnEnable()
    {
        // 초기화
        printIdNumTxt.text = "";
        printNameTxt.text = "";
        printPartnerTxt.text = "";
        partnerIcon.gameObject.SetActive(true);
        printMoneyTxt.text = "";
        printRecordTxt.text = "";

        // 아이디 넘버 출력
        for (int i = 0; i < GlobalValue.idNum.Length; i++)
        {
            printIdNumTxt.text += $"{GlobalValue.idNum[i]}";
        }

        // 이름 출력
        printNameTxt.text = $"{GlobalValue.playerName}";

        // 파트너 포켓몬 출력 (선택한 경우)
        if (GameManager.instance.selectPartner)
        {
            printPartnerTxt.text = $"{MyPokemon.pokemon[0].pokemonName}";
            partnerIcon.IconImg[0] = MyPokemon.pokemon[0].icon[0];
            partnerIcon.IconImg[1] = MyPokemon.pokemon[0].icon[1];
        }

        else
        {
            printPartnerTxt.text = $"아직 없음";
            partnerIcon.gameObject.SetActive(false);
        }

        // 소지금 출력
        if (GlobalValue.playerMoney == 0) printMoneyTxt.text = "0원";
        else printMoneyTxt.text = $"{GlobalValue.playerMoney:#,###}원";

        // 전적 출력
        printRecordTxt.text = $"{GlobalValue.matchCount}전 {GlobalValue.winCount}승 {GlobalValue.loseCount}패";
    }
}
