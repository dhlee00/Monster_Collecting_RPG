using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CureEffect : MonoBehaviour
{
    // 애니메이터
    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        anim.enabled = false;
    }

    // 애니메이션에서 실행
    public void EndEffect()
    {
        CureSelect.instance.afterCurePokemon = true;
        CureSelect.instance.cureStart = false;
        this.gameObject.SetActive(false);
    }
}
