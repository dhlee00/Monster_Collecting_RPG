using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Add Skill/Skill")]
public class Skill : ScriptableObject
{
    // 기술 이펙트
    public GameObject P_Skill_Effect;  // 플레이어
    public GameObject E_Skill_Effect;  // 적
    
    // 기술 이름
    public string skill_Name;

    // 기술 사용 가능 횟수
    public int skill_MaxCount;

    // 실제 사용한 횟수
    [HideInInspector] public int skill_Count;
    
    // 사용 횟수 동기화
    public void CountSync()
    {
        skill_Count = skill_MaxCount;
    }

    // 기술 데미지 (배율)
    public float skill_Damage;
    
    // 기술 명중률
    public int skill_Accuracy;
}
