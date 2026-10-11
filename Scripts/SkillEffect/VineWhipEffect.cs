using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineWhipEffect : MonoBehaviour
{
    // µ¢±º√§¬Ô ¿Ã∆Â∆Æ
    public ParticleSystem[] effect;

    private void OnEnable()
    {
        EMManager.instance.PlayEM("VineWhip", 5, 0.2f);
    }

    void Update()
    {
        // ¿Ã∆Â∆Æ ≥°≥™∏È ªË¡¶
        if (!effect[0].IsAlive() && !effect[1].IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
