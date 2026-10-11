using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class BattleText : MonoBehaviour
{
    // 배틀 정보 텍스트
    public TextMeshProUGUI battleInfoTxt;

    // 출력 중, 출력 중인 코루틴이 있는지 여부
    public bool isPrint;
    public Coroutine nowPrinting;

    // 출력할 대사
    public string printLine;

    void Start()
    {
        // 효과음 재생
        EMManager.instance.PlayEM(BattleManager.instance.enemyPokemon.image.name);

        PrintBattleInfo($"야생의 {BattleManager.instance.enemyPokemon.pokemonName}(이)가 나타났다!");
    }

    // 배틀 정보 출력
    public void PrintBattleInfo(string line)
    {
        printLine = line;

        if (nowPrinting == null)
        {
            nowPrinting = StartCoroutine(TypingEffect(line));
        }
    }

    // 타이핑 효과
    IEnumerator TypingEffect(string text)
    {
        isPrint = true;

        battleInfoTxt.text = "";

        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 0; i < text.Length; i++)
        {
            if (!isPrint)
            {
                battleInfoTxt.text = text;
                break;
            }

            stringBuilder.Append(text[i]);
            battleInfoTxt.text = stringBuilder.ToString();
            yield return new WaitForSeconds(0.06f);
        }

        isPrint = false;
        nowPrinting = null;

        // 플레이어의 턴일 때 행동 선택 상태에서 대사 출력 끝나고 선택창 활성화
        if (BattleManager.instance.turn == Turn.player &&
            BattleManager.instance.state == BattleState.selectAction &&
            BattleManager.instance.progressNum == 0)
        {
            if (!BattleManager.instance.selectView.activeSelf)
            {
                BattleManager.instance.selectView.SetActive(true);
            }
        }
    }
}
