using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBall_Catch : MonoBehaviour
{
    // 애니메이터
    Animator anim;

    // 몬스터볼 열렸을 때 이펙트
    public GameObject monsterBall_Effect;
    ParticleSystem p_monsterBall_Effect;

    // 몬스터를 잡았을 때 이펙트
    public GameObject catchEffect;
    ParticleSystem p_catchEffect;

    void Start()
    {
        anim = GetComponent<Animator>();
        p_monsterBall_Effect = monsterBall_Effect.GetComponentInChildren<ParticleSystem>();
        p_catchEffect = catchEffect.GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        // 이펙트 시간 끝나면
        if (monsterBall_Effect.activeSelf || catchEffect.activeSelf)
        {
            if (monsterBall_Effect.activeSelf && !p_monsterBall_Effect.IsAlive())
            {
                // 애니메이션 재생
                anim.Play("MonsterBall_Catch");

                // 이펙트 비활성화
                monsterBall_Effect.SetActive(false);
            }

            if (catchEffect.activeSelf && !p_catchEffect.IsAlive())
            {
                // 이펙트 비활성화
                catchEffect.SetActive(false);
            }
        }
    }

    // 몬스터볼 던지는 애니메이션에서 호출
    public void ThrowBall()
    {
        // 효과음 재생
        EMManager.instance.PlayEM("Catch");

        // 이펙트 활성화
        monsterBall_Effect.SetActive(true);

        // 포켓몬 연출 재생
        BattleManager.instance.enemyAnim.Play("EnemyCatched");
    }

    // 몬스터볼 왔다갔다하는 애니메이션에서 호출
    public void CatchPokemom()
    {
        if (CanCatch())
        {
            // 배경음 정지
            BGMManager.instance.audioSource.Stop();

            // 효과음 재생
            EMManager.instance.PlayEM("GetPokemon");

            // 이펙트 활성화
            catchEffect.SetActive(true);

            // 대사 출력
            BattleManager.instance.bt.PrintBattleInfo
                ($"신난다! {BattleManager.instance.enemyPokemon.pokemonName}(을)를 잡았다!!");

            // 연출 끝
            BattleManager.instance.catchSuccess = true;
            BattleManager.instance.catchMotion = false;
        }

        // 실패
        else
        {
            // 효과음 재생
            EMManager.instance.PlayEM("OutBall");

            // 액티브 한번 껐다 켜기
            BattleManager.instance.enemyImg.gameObject.SetActive(false);
            BattleManager.instance.enemyAnim.enabled = false;
            BattleManager.instance.enemyImg.transform.localPosition = new Vector3(15, 90);
            BattleManager.instance.enemyImg.transform.localScale = new Vector3(1, 1, 1);
            BattleManager.instance.enemyAnim.enabled = true;
            BattleManager.instance.enemyImg.gameObject.SetActive(true);

            // 흰색 연출 활성화
            BattleManager.instance.enemyImg.transform.GetChild(0).gameObject.SetActive(true);

            // 대사 출력
            BattleManager.instance.bt.PrintBattleInfo
                ($"앗! {BattleManager.instance.enemyPokemon.pokemonName}(이)가 탈출했다!");

            // 연출 끝
            BattleManager.instance.catchSuccess = false;
            BattleManager.instance.catchMotion = false;

            // 자기자신 비활성화
            this.gameObject.SetActive(false);
        }
    }

    // 포획 확률 계산 공식
    bool CanCatch()
    {
        // 포획 성공 확률
        float hpPercent = (BattleManager.instance.enemyPokemon.nowHP / BattleManager.instance.enemyPokemon.maxHP) * 100f;
        float catchProb;

        // 체력이 10% 이하면 100 확률
        if (hpPercent <= 10f)
        {
            catchProb = 100f;
        }

        else if (hpPercent >= 90f)
        {
            catchProb = 0f;
        }

        else
        {
            catchProb = Mathf.Clamp01((90f - hpPercent) / 80f) * 100f;
        }

        int ran = Random.Range(1, 101);
        if (ran <= (int)catchProb)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    // 애니메이션에서 호출 (효과음 재생)
    public void BounceSound()
    {
        // 효과음 재생
        EMManager.instance.PlayEM("Bounce");
    }

    public void ShakeSound()
    {
        // 효과음 재생
        EMManager.instance.PlayEM("Shake");
    }
}
