using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    // 인스턴스화
    public static DataLoader instance;

    // 모든 포켓몬 불러올 딕셔너리
    Dictionary<string, Pokemon> pokemonDic = new Dictionary<string, Pokemon>();

    // 모든 아이템 불러올 딕셔너리
    Dictionary<string, Item> itemDic = new Dictionary<string, Item>();

    // 포켓몬 랜덤 뽑기 위한 배열
    List<Pokemon> pokemonNum = new List<Pokemon>();

    void Awake()
    {
        // 모든 포켓몬 로드
        Pokemon[] allPokemon = Resources.LoadAll<Pokemon>("Pokemon");

        // 딕셔너리에 포켓몬의 이름으로 저장
        foreach (Pokemon pokemon in allPokemon)
        {
            if (!pokemonDic.ContainsKey(pokemon.pokemonName))
            {
                pokemonDic.Add(pokemon.pokemonName, pokemon);
                pokemonNum.Add(pokemon);
            }
        }

        // 모든 아이템 로드
        Item[] allItem = Resources.LoadAll<Item>("Item");

        // 딕셔너리에 아이템의 이름으로 저장
        foreach (Item item in allItem)
        {
            if (!itemDic.ContainsKey(item.item_Name))
            {
                itemDic.Add(item.item_Name, item);
            }
        }

        // 인스턴스에 값이 없으면 자기자신 할당
        if (instance == null)
        {
            instance = this;

            // 사라지지 않음
            DontDestroyOnLoad(gameObject);

            // 아이템 복사해서 인벤토리에 저장
            MyItem.item.Add(GetItem("몬스터볼"));
            MyItem.item.Add(GetItem("상처약"));
        }

        // 인스턴스에 값이 있으면 삭제
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // 해당 이름의 포켓몬이 있다면 복사본 생성
    public Pokemon GetPokemon(string pokemonName)
    {
        if (pokemonDic.ContainsKey(pokemonName))
        {
            // 스킬 얻은 상태로 복사
            Pokemon pokemon = Instantiate(pokemonDic[pokemonName]);
            pokemon.Setting();

            // 스킬 사용 횟수 동기화
            for (int i = 0; i < pokemon.skill.Length; i++)
            {
                pokemon.skill[i].CountSync();
            }

            return Instantiate(pokemon);
        }

        else
        {
            return null;
        }
    }

    // 랜덤으로 복사본 생성 (배틀에서)
    public Pokemon GetRandomPokemon()
    {
        int ranNum;

        // 가진 포켓몬이 한개 밖에 없고 포켓몬의 레벨이 2이하인 경우
        if (MyPokemon.pokemon.Count <= 1 && MyPokemon.pokemon[0].level <= 2)
        {
            // 꼬렛이나 이상해씨만 등장
            ranNum = Random.Range(0, 2) == 0 ? 0 : 3;
        }

        else
        {
            ranNum = Random.Range(0, pokemonNum.Count);
        }

        return Instantiate(GetPokemon(pokemonNum[ranNum].pokemonName));
    }

    // 해당 이름의 아이템이 있다면 복사본 생성
    public Item GetItem(string itemName)
    {
        if (itemDic.ContainsKey(itemName))
        {
            return Instantiate(itemDic[itemName]);
        }

        else
        {
            return null;
        }
    }
}
