using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    public ThundershockEffect t;

    public void Sound_2()
    {
        // 효과음 재생
        EMManager.instance.PlayEM("Thundershock_2");
    }
    public void Sound_3()
    {
        // 효과음 재생
        EMManager.instance.PlayEM("Thundershock_3");
    }

    public void EndEffect()
    {
        gameObject.SetActive(false);
        t.timer = 0.0f;
        t.isEffect = false;
    }
}
