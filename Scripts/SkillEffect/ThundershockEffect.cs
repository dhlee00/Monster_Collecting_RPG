using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThundershockEffect : MonoBehaviour
{
    // 번개 쏘는 이펙트
    public ParticleSystem effect;

    // 배경 이미지
    Image backGround;

    // 번개 치는 이펙트
    public GameObject thunder;

    // 배경 어두워지는 시간
    float duration = 0.7f;
    public float timer = 0.0f;

    // 이펙트 시작
    [HideInInspector] public bool isEffect;

    // 소리 재생
    bool playSound = false;

    void Start()
    {
        backGround = GameManager.instance.canvas.GetComponentInChildren<Image>();

        timer = 0.0f;
        isEffect = true;
    }

    private void OnEnable()
    {
        playSound = false;

        EMManager.instance.PlayEM("Thundershock_0", 4, 0.1f);
    }

    void Update()
    {
        if (isEffect)
        {
            // 번개 쏘는 이펙트 끝나면 배경 점점 어두워지게
            if (!effect.IsAlive())
            {
                // 효과음 재생
                if (!playSound)
                {
                    EMManager.instance.PlayEM("Thundershock_1");
                    playSound = true;
                }

                timer += Time.deltaTime;

                float t = timer / duration;

                backGround.color = Color.Lerp(new Color(1.0f, 1.0f, 1.0f), new Color(0.4f, 0.4f, 0.4f), t);

                if (timer >= duration)
                {
                    thunder.SetActive(true);
                }
            }
        }

        else
        {
            timer += Time.deltaTime;

            float t = timer / duration;

            backGround.color = Color.Lerp(new Color(0.4f, 0.4f, 0.4f), new Color(1.0f, 1.0f, 1.0f), t);

            if (timer >= duration)
            {
                Destroy(gameObject);
            }
        }
    }
}
