using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CureManager : MonoBehaviour
{
    // 간호사
    public SpriteRenderer nurse_Spr;

    // 간호사 이미지
    public Sprite[] nurseImg;

    // 치료 중인 몬스터볼
    public GameObject[] cureMonsterBall;
    public Animator[] cureMBAnim;

    // 모니터
    public Animator monitorAnim;

    // 타이머
    public float timer = 0.7f;

    // 치료할 포켓몬 수
    public int pokemonCount;

    void Start()
    {
        pokemonCount = MyPokemon.pokemon.Count;
    }

    void Update()
    {
        // 치료 이펙트 시작
        if (CureSelect.instance.cureStart && !CureSelect.instance.afterCurePokemon)
        {
            // 간호사 이미지 변경
            nurse_Spr.sprite = nurseImg[1];

            // 0.7초 마다 치료할 몬스터볼 활성화
            if (pokemonCount > 0)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    // 플레이어가 가진 포켓몬 마리 수만큼
                    cureMonsterBall[MyPokemon.pokemon.Count - pokemonCount].SetActive(true);
                    pokemonCount--;
                    timer = 0.7f;
                }
            }

            // 모두 활성화 하면 애니메이션 재생
            else if (pokemonCount == 0)
            {
                timer = 0.7f;
                monitorAnim.gameObject.SetActive(true);

                // 잠시 대기 후
                StartCoroutine(PlayAnim());

                pokemonCount = -1;
            }
        }

        // 치료 후
        else if (CureSelect.instance.afterCurePokemon)
        {
            // 포켓몬 치료
            for (int i = 0; i < MyPokemon.pokemon.Count; i++)
            {
                MyPokemon.pokemon[i].nowHP = MyPokemon.pokemon[i].maxHP;
                MyPokemon.pokemon[i].skill[0].skill_Count = MyPokemon.pokemon[i].skill[0].skill_MaxCount;
                MyPokemon.pokemon[i].skill[1].skill_Count = MyPokemon.pokemon[i].skill[1].skill_MaxCount;
                MyPokemon.pokemon[i].canFight = true;
            }

            // 간호사 이미지 변경
            nurse_Spr.sprite = nurseImg[0];

            // 변수 초기화
            pokemonCount = MyPokemon.pokemon.Count;
            timer = 0.7f;
        }
    }

    // 대기 함수
    IEnumerator PlayAnim()
    {
        yield return new WaitForSeconds(0.7f);

        for (int i = 0; i < MyPokemon.pokemon.Count; i++)
        {
            cureMBAnim[i].enabled = true;
            monitorAnim.enabled = true;
        }

        // 효과음 재생
        EMManager.instance.PlayEM("PokemonHeal");
    }
}
