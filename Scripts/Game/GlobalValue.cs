using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalValue
{
    // 아이디 넘버
    public static int[] idNum = new int[6];

    // 플레이어 이름
    public static string playerName = "";

    // 플레이어 전적
    public static int matchCount = 0;
    public static int winCount = 0;
    public static int loseCount = 0;

    // 플레이어 소지금
    public static int playerMoney = 0;
}

public class MyPokemon
{
    // 플레이어가 가진 포켓몬
    public static List<Pokemon> pokemon = new List<Pokemon>();

    // 포켓몬 순서 교체
    public static void SwitchPokemon(int num)
    {
        // 예외처리
        if (num <= 0 || num >= pokemon.Count) return;

        // 현재 대표 포켓몬을 리스트 끝으로 이동
        Pokemon firstPokemon = pokemon[0];
        pokemon.RemoveAt(0);
        pokemon.Add(firstPokemon);

        // 선택된 포켓몬을 앞쪽으로 가져옴
        Pokemon selected = pokemon[num - 1];
        pokemon.RemoveAt(num - 1);
        pokemon.Insert(0, selected);
    }
}

public class MyItem
{
    // 플레이어가 가진 아이템
    public static List<Item> item = new List<Item>();
}

// 데이더 저장용 클래스
public class DataManager
{
    // 아이디 넘버
    public int[] idNum;

    // 플레이어 이름
    public string playerName;

    // 플레이어 전적
    public int matchCount;
    public int winCount;
    public int loseCount;

    // 플레이어 소지금
    public int playerMoney;

    // 포켓몬 데이터
    public string[] pokemonName = new string[MyPokemon.pokemon.Count];
    public float[] nowHP = new float[MyPokemon.pokemon.Count];
    public float[] maxHP = new float[MyPokemon.pokemon.Count];
    public float[] attack = new float[MyPokemon.pokemon.Count];
    public float[] defence = new float[MyPokemon.pokemon.Count];
    public int[] speed = new int[MyPokemon.pokemon.Count];
    public int[] level = new int[MyPokemon.pokemon.Count];
    public float[] nowEXP = new float[MyPokemon.pokemon.Count];
    public float[] needEXP = new float[MyPokemon.pokemon.Count];
    public int[] skill_0Count = new int[MyPokemon.pokemon.Count];
    public int[] skill_1Count = new int[MyPokemon.pokemon.Count];

    // 아이템 개수
    public int[] itemCount = new int[MyItem.item.Count];

    // 현재 씬
    public int sceneNum;

    // 현재 위치
    public float posX, posY;

    // 상태값
    public bool doctor_O_FirstTalk;
    public bool selectPartner;
    public bool afterSelectTalk;
    public bool tutorialOver;

    // 데이터 저장
    public static void Save()
    {
        DataManager data = new DataManager();

        data.idNum = GlobalValue.idNum;              // 아이디 넘버
        data.playerName = GlobalValue.playerName;    // 플레이어 이름
        data.matchCount = GlobalValue.matchCount;    // 전적
        data.winCount = GlobalValue.winCount;        // 이긴 횟수
        data.loseCount = GlobalValue.loseCount;      // 진 횟수
        data.playerMoney = GlobalValue.playerMoney;  // 소지금

        // 포켓몬 능력치 저장
        for (int i = 0; i < MyPokemon.pokemon.Count; i++)
        {
            data.pokemonName[i] = MyPokemon.pokemon[i].pokemonName;
            data.nowHP[i] = MyPokemon.pokemon[i].nowHP;
            data.maxHP[i] = MyPokemon.pokemon[i].maxHP;
            data.attack[i] = MyPokemon.pokemon[i].attack;
            data.defence[i] = MyPokemon.pokemon[i].defence;
            data.speed[i] = MyPokemon.pokemon[i].speed;
            data.level[i] = MyPokemon.pokemon[i].level;
            data.nowEXP[i] = MyPokemon.pokemon[i].nowEXP;
            data.needEXP[i] = MyPokemon.pokemon[i].needEXP;
            data.skill_0Count[i] = MyPokemon.pokemon[i].skill[0].skill_Count;
            data.skill_1Count[i] = MyPokemon.pokemon[i].skill[1].skill_Count;
        }

        // 아이템
        for (int i = 0; i < MyItem.item.Count; i++)
        {
            data.itemCount[i] = MyItem.item[i].item_HaveCount;
        }

        data.sceneNum = GameManager.instance.sceneNum;  // 씬 번호
        data.posX = GameManager.instance.playerX;       // x 좌표
        data.posY = GameManager.instance.playerY;       // y 좌표

        data.doctor_O_FirstTalk = GameManager.instance.doctor_O_FirstTalk;  // 오박사와 첫 대화
        data.selectPartner = GameManager.instance.selectPartner;            // 파트너 고른 상태
        data.afterSelectTalk = GameManager.instance.afterSelectTalk;        // 파트너 고른 후 대화
        data.tutorialOver = GameManager.instance.tutorialOver;              // 튜토리얼 끝난 상태

        string json = JsonUtility.ToJson(data);
        string path = System.IO.Path.Combine(Application.persistentDataPath, "save.json");
        System.IO.File.WriteAllText(path, json);
    }

    // 데이터 불러오기
    public static void Load()
    {
        string path = System.IO.Path.Combine(Application.persistentDataPath, "save.json");

        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            DataManager data = JsonUtility.FromJson<DataManager>(json);

            GlobalValue.idNum = data.idNum;              // 아이디 넘버
            GlobalValue.playerName = data.playerName;    // 플레이어 이름
            GlobalValue.matchCount = data.matchCount;    // 전적
            GlobalValue.winCount = data.winCount;        // 이긴 횟수
            GlobalValue.loseCount = data.loseCount;      // 진 횟수
            GlobalValue.playerMoney = data.playerMoney;  // 소지금

            // 포켓몬
            MyPokemon.pokemon.Clear();
            // 이름만 먼저 불러와서 포켓몬 이름으로 추가
            for (int i = 0; i < data.pokemonName.Length; i++)
            {
                MyPokemon.pokemon.Add(DataLoader.instance.GetPokemon(data.pokemonName[i]));
            }

            // 능력치 덮어 씌우기
            for (int i = 0; i < data.pokemonName.Length; i++)
            {
                MyPokemon.pokemon[i].pokemonName = data.pokemonName[i];
                MyPokemon.pokemon[i].nowHP = data.nowHP[i];
                MyPokemon.pokemon[i].maxHP = data.maxHP[i];
                MyPokemon.pokemon[i].attack = data.attack[i];
                MyPokemon.pokemon[i].defence = data.defence[i];
                MyPokemon.pokemon[i].speed = data.speed[i];
                MyPokemon.pokemon[i].level = data.level[i];
                MyPokemon.pokemon[i].nowEXP = data.nowEXP[i];
                MyPokemon.pokemon[i].needEXP = data.needEXP[i];
                MyPokemon.pokemon[i].skill[0].skill_Count = data.skill_0Count[i];
                MyPokemon.pokemon[i].skill[1].skill_Count = data.skill_1Count[i];
            }

            // 아이템
            for (int i = 0; i < MyItem.item.Count; i++)
            {
                MyItem.item[i].item_HaveCount = data.itemCount[i];
            }

            GameManager.instance.sceneNum = data.sceneNum;  // 씬 번호
            GameManager.instance.playerX = data.posX;       // x 좌표
            GameManager.instance.playerY = data.posY;       // y 좌표

            GameManager.instance.doctor_O_FirstTalk = data.doctor_O_FirstTalk;  // 오박사와 첫 대화
            GameManager.instance.selectPartner = data.selectPartner;            // 파트너 고른 상태
            GameManager.instance.afterSelectTalk = data.afterSelectTalk;        // 파트너 고른 후 대화
            GameManager.instance.tutorialOver = data.tutorialOver;              // 튜토리얼 끝난 상태

            GameManager.instance.isContinue = true;
        }
    }

    // 데이터 초기화
    public static void ResetData()
    {
        GlobalValue.idNum = new int[6];  // 아이디 넘버
        GlobalValue.playerName = "";     // 이름
        GlobalValue.matchCount = 0;      // 전적
        GlobalValue.winCount = 0;        // 이긴 횟수
        GlobalValue.loseCount = 0;       // 진 횟수
        GlobalValue.playerMoney = 0;     // 소지금

        // 포켓몬
        MyPokemon.pokemon.Clear();

        // 아이템
        for (int i = 0; i < MyItem.item.Count; i++)
        {
            MyItem.item[i].item_HaveCount = 0;
        }

        GameManager.instance.sceneNum = 0;  // 씬 번호
        GameManager.instance.playerX = 0;   // x 좌표
        GameManager.instance.playerY = 0;   // y 좌표

        GameManager.instance.doctor_O_FirstTalk = false;  // 오박사와 첫 대화
        GameManager.instance.selectPartner = false;       // 파트너 고른 상태
        GameManager.instance.afterSelectTalk = false;     // 파트너 고른 후 대화
        GameManager.instance.tutorialOver = false;        // 튜토리얼 끝난 상태
    }
}
