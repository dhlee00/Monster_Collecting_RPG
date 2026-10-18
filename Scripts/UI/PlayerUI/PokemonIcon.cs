using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokemonIcon : MonoBehaviour
{
    // 이미지
    Image img;

    // 파트너 이미지
    public Sprite[] IconImg = new Sprite[2];

    // 이미지 번호
    int imgNum;

    // 타이머
    float timer = 0.4f;

    void Start()
    {
        img = GetComponent<Image>();
    }

    void Update()
    {
        // 이미지 할당
        img.sprite = IconImg[imgNum];

        // 타이머 0초 됐을 때
        timer -= Time.deltaTime;
        if (timer <= 0.0f)
        {
            // 0이면 1, 1이면 0
            if (imgNum == 0) imgNum = 1;
            else if (imgNum == 1) imgNum = 0;

            // 타이머 초기화
            timer = 0.4f;
        }
    }
}
