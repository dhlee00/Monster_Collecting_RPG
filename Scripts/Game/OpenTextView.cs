using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTextView : MonoBehaviour
{
    // 텍스트 창
    public GameObject textView;

    void Update()
    {
        if (GameManager.instance.isTalking)
        {
            textView.SetActive(true);
        }

        else
        {
            textView.SetActive(false);
        }
    }
}
