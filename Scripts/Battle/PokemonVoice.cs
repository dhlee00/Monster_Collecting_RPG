using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonVoice : MonoBehaviour
{
    // 애니메이션에서 호출
    public void PlayVoice()
    {
        // 인트로 씬인 경우
        if (GameManager.instance.sceneNum == 1)
        {
            EMManager.instance.PlayEM("Pikachu");
        }

        else
        {
            if (gameObject.CompareTag("Player"))
            {
                EMManager.instance.PlayEM(MyPokemon.pokemon[0].image.name);
                EMManager.instance.PlayEM($"{MyPokemon.pokemon[0].image.name}_Battle");
            }
        }
    }
}
