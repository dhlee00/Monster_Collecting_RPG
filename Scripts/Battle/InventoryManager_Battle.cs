using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager_Battle : InventoryManager
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 효과음 재생
            EMManager.instance.PlayEM("Cancle");

            transform.parent.gameObject.SetActive(false);
        }

        base.SelectIndex();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < itemList.Length; i++)
            {
                if (itemList[i].sprite == selectImage)
                {
                    UseItem(i);
                    break;
                }
            }
        }
    }

    // 버튼 호출
    public void UseItem(int num)
    {
        if (num < MyItem.item.Count)
        {
            if (MyItem.item[num].item_HaveCount > 0)
            {
                switch (num)
                {
                    // 몬스터볼
                    case 0:
                        // 가진 포켓몬이 7마리 미만일 때
                        if (MyPokemon.pokemon.Count < 6)
                        {
                            // 효과음 재생
                            EMManager.instance.PlayEM("Select");

                            MyItem.item[0].item_HaveCount--;
                            BattleManager.instance.catchMotion = true;
                            BattleManager.instance.ChangeState(BattleState.useBall);
                            BattleManager.instance.bt.PrintBattleInfo($"{GlobalValue.playerName}(은)는 몬스터볼을 사용했다!");
                            BattleManager.instance.monsterBallCatch.SetActive(true);
                            transform.parent.gameObject.SetActive(false);
                            BattleManager.instance.selectView.SetActive(false);
                        }

                        else
                        {
                            // 효과음 재생
                            EMManager.instance.PlayEM("Warning");
                        }
                        break;

                    // 상처약
                    case 1:
                        // 효과음 재생
                        EMManager.instance.PlayEM("Select");

                        MyItem.item[1].item_HaveCount--;
                        BattleManager.instance.ChangeState(BattleState.useMedi);
                        BattleManager.instance.bt.PrintBattleInfo($"{GlobalValue.playerName}(은)는 상처약을 사용했다.");
                        transform.parent.gameObject.SetActive(false);
                        BattleManager.instance.selectView.SetActive(false);
                        break;
                }
            }

            else
            {
                // 효과음 재생
                EMManager.instance.PlayEM("Warning");
            }
        }
    }
}
