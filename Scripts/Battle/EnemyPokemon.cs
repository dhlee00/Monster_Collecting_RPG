using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPokemon : MonoBehaviour
{
    // 애니메이션에서 호출
    public void Attack()
    {
        // 효과음 재생
        EMManager.instance.PlayEM("Tackle");
    }

    public void Die()
    {
        // 효과음 재생
        EMManager.instance.PlayEM("Die");
    }
}
