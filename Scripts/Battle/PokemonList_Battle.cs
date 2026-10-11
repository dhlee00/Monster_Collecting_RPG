using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonList_Battle : PokemonListManager
{
    // 교체 할 포켓몬 인덱스
    int outPokemon;

    void Update()
    {
        // 선택하기 전
        if (!selectIndex)
        {
            optionView.SetActive(false);

            SelectIndex();

            // 배틀씬인 경우 "ESC" 키로 닫기
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // 효과음 재생
                EMManager.instance.PlayEM("Cancle");

                this.transform.parent.gameObject.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                for (int i = 0; i < listImage.Length; i++)
                {
                    if (listImage[i].sprite == selectListImg)
                    {
                        SelectPokemonIndex(i + 1);
                        break;
                    }
                }
            }
        }

        // 선택 후
        else if (selectIndex)
        {
            optionView.SetActive(true);

            // 한프레임 쉬기 (창 바로 열리는 문제 때문에)
            StartCoroutine(WaitFrame());

            if (canSelectOption)
            {
                SelectOption();
            }
        }
    }

    // 선택한 포켓몬 정보 전달
    public override void SelectPokemonIndex(int num)
    {
        if (MyPokemon.pokemon.Count >= num + 1)
        {
            if (MyPokemon.pokemon[num].canFight)
            {
                // 효과음 재생
                EMManager.instance.PlayEM("Select");

                outPokemon = num;
                selectIndex = true;
            }
        }
    }

    #region 옵션 창
    // 옵션 이동
    void SelectOption()
    {
        // 교체
        if (selectIcon[0].activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                // 효과음 재생
                EMManager.instance.PlayEM("Choose");

                selectIcon[0].SetActive(false);
                selectIcon[1].SetActive(true);
            }

            // 선택하면 포켓몬 교체
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // 효과음 재생
                EMManager.instance.PlayEM("Select");

                ClickBtn_Battle(0);
            }
        }

        // 취소
        else if (selectIcon[1].activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                // 효과음 재생
                EMManager.instance.PlayEM("Choose");

                selectIcon[1].SetActive(false);
                selectIcon[0].SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                // 효과음 재생
                EMManager.instance.PlayEM("Cancle");

                ClickBtn_Battle(1);
            }
        }
    }

    // 버튼 이벤트에서 호출
    public void ClickBtn_Battle(int num)
    {
        canSelectOption = false;
        selectIndex = false;

        // 교체
        if (num == 0)
        {
            // 효과음 재생
            EMManager.instance.PlayEM("Select");

            BattleManager.instance.ChangeState(BattleState.switchPokemon);
            BattleManager.instance.bt.PrintBattleInfo($"{MyPokemon.pokemon[0].pokemonName}! 돌아와!");

            // 포켓몬 교체
            MyPokemon.SwitchPokemon(outPokemon);

            // 포켓몬 교체 연출
            BattleManager.instance.partnerAnim.Play("PartnerSwitch");

            // UI 잠시 비활성화
            BattleManager.instance.partnerInfo.SetActive(false);

            transform.parent.gameObject.SetActive(false);
            BattleManager.instance.selectView.SetActive(false);
        }

        // 취소
        if (num == 1)
        {
            // 효과음 재생
            EMManager.instance.PlayEM("Cancle");

            return;
        }
    }
    #endregion
}
