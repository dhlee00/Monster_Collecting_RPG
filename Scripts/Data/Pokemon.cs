using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon", menuName = "Add Pokemon/Pokemon")]
public class Pokemon : ScriptableObject
{
    // 포켓몬 이미지
    public Sprite image;
    public Sprite image_Battle;
    public Sprite[] icon = new Sprite[2];

    [Header("")]

    // 포켓몬 이름
    public string pokemonName;

    // 능력치
    [HideInInspector] public float nowHP;  // 현재 체력
    public float maxHP;                    // 최대 체력
    public float attack;                   // 공격력
    public float defence;                  // 방어력
    public int   speed;                    // 스피드

    // 레벨
    public int level = 1;

    // 경험치
    public float nowEXP = 0;

    // 필요 경험치
    public float needEXP = 10;

    [Header("")]

    // 스킬 데이터
    [SerializeField] private Skill[] skillData;

    // 스킬 복사해서 저장할 변수
    [HideInInspector] public Skill[] skill;

    // 체력, 스킬 세팅
    public void Setting()
    {
        nowHP = maxHP;

        skill = new Skill[skillData.Length];

        for (int i = 0; i < skillData.Length; i++)
        {
            skill[i] = Instantiate(skillData[i]);
        }
    }

    [Header("")]

    // 싸울 수 있는 상태
    public bool canFight;
}
