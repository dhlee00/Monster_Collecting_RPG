using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBManager : MonoBehaviour
{
    // 포켓몬 정보 창
    public PokemonInfoView pokemonInfoView;

    // 애니메이터
    Animator anim;

    // 몬스터볼 열림 여부
    bool isOpen;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // 몬스터볼 열리는 애니메이션
    public void OpenBall()
    {
        if (!isOpen)
        {
            anim.SetTrigger("open");
            isOpen = true;
        }
    }

    // 몬스터볼 닫히는 애니메이션
    public void CloseBall()
    {
        if (isOpen)
        {
            anim.SetTrigger("close");
            isOpen = false;
        }
    }

    // 몬스터볼 열리는 애니메이션에서 호출
    public void OpenInfo()
    {
        // 포켓몬을 아직 선택하지 않은 경우에만 활성화
        if (!GameManager.instance.selectPartner)
        {
            // 효과음 재생
            EMManager.instance.PlayEM("OpenUI");

            pokemonInfoView.mbm = this;
            pokemonInfoView.pokemonName = this.name;
            pokemonInfoView.gameObject. SetActive(true);
        }
    }
}
