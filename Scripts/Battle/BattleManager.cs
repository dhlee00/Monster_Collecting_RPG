using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 턴
public enum Turn
{
    player,
    enemy
}

// 전투 상태
public enum BattleState
{
    battleEnter,    // 전투 돌입
    selectAction,   // 행동 선택
    Attack,         // 공격
    useBall,        // 몬스터볼 사용
    useMedi,        // 상처약 사용
    switchPokemon,  // 교체
    loseSwitch,     // 패배 교체
    run,            // 도망가기
    win,            // 승리
    lose            // 패배
}

public class BattleManager : MonoBehaviour
{
    // 스태틱화
    public static BattleManager instance;

    // 전투 상태
    public BattleState state;

    // 턴
    public Turn turn;

    // 전투 텍스트
    public BattleText bt;

    // 스킬 이펙트 담을 오브젝트
    public Transform skillEffect;

    // 진행 번호
    public int progressNum;

    // 코루틴 실행 중
    bool coroutineIng;

    // 포획 연출 중
    public bool catchMotion;

    // 포획 성공 여부
    public bool catchSuccess;

    // 레벨업 여부
    public bool levelUP;

    // 레벨업 시 증가할 스탯
    float increase;

    // 포켓몬 별 가중 치
    float[] growthWeight = new float[3];

    // 스탯 증가량
    float[] increaseStat = new float[3];

    #region UI 관련
    [Header("UI")]

    // 스킵
    public bool skipText;

    // 플레이어 애니메이터
    public Animator player_Battle;

    // 플레이어 포켓몬 애니메이터
    public Animator partnerAnim;

    // 적 포켓몬 애니메이터
    public Animator enemyAnim;

    // 포켓몬 UI
    public GameObject pokemonInfo;

    // 플레이어 포켓몬 UI
    public GameObject partnerInfo;

    // 교체 몬스터볼
    public GameObject monsterBallSwitch;

    // 포획 몬스터볼
    public GameObject monsterBallCatch;

    // 선택창
    public GameObject selectView;

    // 가방
    public GameObject inventory;

    // 포켓몬 목록
    public GameObject pokemonList;

    // 페이드
    public GameObject fade;
    #endregion

    #region 파트너 포켓몬 정보
    [Header("Partner Info")]

    // 파트너 이미지
    public Image partnerImg;

    // 파트너 이름
    public TextMeshProUGUI nameTxt;

    // 파트너 레벨
    public TextMeshProUGUI levelTxt;

    // 파트너 HP
    public Image hpBar;
    public TextMeshProUGUI hpTxt;

    // 파트너 경험치
    public Image expBar;

    // 패배 시 교체할 포켓몬
    int loseSwitchPokemon;

    // 사용한 스킬 번호
    public int skillNum;
    #endregion

    #region 적 포켓몬 정보
    [Header("Enemy Info")]  // 적 정보

    // 적 포켓몬
    [HideInInspector] public Pokemon enemyPokemon;

    // 적 이미지
    public Image enemyImg;

    // 적 이름
    public TextMeshProUGUI enemyNameTxt;

    // 적 레벨
    public TextMeshProUGUI enemyLevelTxt;

    // 적 HP
    public Image enemyHpBar;

    // 적이 사용할 기술
    int ranSkill;
    #endregion

    private void Awake()
    {
        if (instance == null) instance = this;

        GameManager.instance.cantDoing = false;
        GameManager.instance.goBattle = false;
    }

    void Start()
    {
        // 랜덤으로 포켓몬 복사본 생성
        enemyPokemon = DataLoader.instance.GetRandomPokemon();

        // 플레이어의 포켓몬의 레벨에 따라 적 포켓몬 레벨 증가
        if (2 < MyPokemon.pokemon[0].level)
        {
            // 플레이어 포켓몬의 레벨 -2 ~ +1 만큼 레벨 증가
            int ran = Random.Range(MyPokemon.pokemon[0].level - 2, MyPokemon.pokemon[0].level + 2);
            enemyPokemon.level = ran;

            // 스탯 증가
            for (int i = 1; i < ran; i++)
            {
                StatUp(enemyPokemon);

                // 필요 경험치 증가
                float needExp = enemyPokemon.needEXP;
                enemyPokemon.needEXP += needExp * 0.6f;

                // 반올림
                enemyPokemon.needEXP = Mathf.RoundToInt(enemyPokemon.needEXP);
            }
        }

        #region UI 출력
        // 파트너 이미지 넣기
        partnerImg.sprite = MyPokemon.pokemon[0].image_Battle;

        // 파트너 이름 출력
        nameTxt.text = MyPokemon.pokemon[0].pokemonName;

        // 적 이미지 넣기
        enemyImg.sprite = enemyPokemon.image;

        // 적 이름 출력
        enemyNameTxt.text = enemyPokemon.pokemonName;

        // 적 레벨
        enemyLevelTxt.text = $"Lv {enemyPokemon.level}";
        #endregion

        // 선공 선택
        if (MyPokemon.pokemon[0].speed >= enemyPokemon.speed)
        {
            turn = Turn.player;
        }

        else if (MyPokemon.pokemon[0].speed < enemyPokemon.speed)
        {
            turn = Turn.enemy;
        }
    }

    void Update()
    {
        #region UI 출력
        // 파트너 체력 출력
        hpBar.fillAmount = MyPokemon.pokemon[0].nowHP / MyPokemon.pokemon[0].maxHP;
        hpTxt.text = $"{Mathf.RoundToInt(MyPokemon.pokemon[0].nowHP)} / {Mathf.RoundToInt(MyPokemon.pokemon[0].maxHP)}";

        // 파트너 레벨, 경험치 출력
        levelTxt.text = $"Lv {MyPokemon.pokemon[0].level}";
        expBar.fillAmount = MyPokemon.pokemon[0].nowEXP / MyPokemon.pokemon[0].needEXP;

        // 적 체력 출력
        if (enemyHpBar.fillAmount >= 1.0f)
        {
            enemyHpBar.fillAmount = 0.0f;
        }

        enemyHpBar.fillAmount = enemyPokemon.nowHP / enemyPokemon.maxHP;
        #endregion

        // 플레이어의 포켓몬의 체력이 0일 때 & 스킬 카운트를 모두 소비했을 때
        if (MyPokemon.pokemon[0].nowHP <= 0 &&
            state != BattleState.loseSwitch && state != BattleState.lose)
        {
            // 싸울 수 없는 상태로 전환
            MyPokemon.pokemon[0].canFight = false;

            // 교체할 포켓몬이 있다면
            if (MyPokemon.pokemon.Count > 1 && HaveCanFightPokemon())
            {
                // 전투 가능한 포켓몬으로 교체
                ChangeState(BattleState.loseSwitch);
            }

            // 없다면 패배
            else
            {
                ChangeState(BattleState.lose);
            }
        }

        // 적 포켓몬의 체력이 0일 때
        if (enemyPokemon.nowHP <= 0 && state != BattleState.win)
        {
            // 상태 변환
            ChangeState(BattleState.win);
        }

        // 스페이스바, 마우스 클릭하면 진행
        if (bt.gameObject.activeSelf && !bt.isPrint && !selectView.activeSelf && !catchMotion &&
            skillEffect.childCount <= 0 &&  // 기술 이펙트 넣을 오브젝트의 자식이 없다 = 실행 중인 이펙트가 없다
            (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            // 효과음 재생
            EMManager.instance.PlayEM("Select");

            progressNum++;

            // 전투 돌입
            if (state == BattleState.battleEnter)
            {
                BattleEnter();
            }

            // 패배 교체
            else if (state == BattleState.loseSwitch)
            {
                LoseSwitch();
            }

            // 승리
            else if (state == BattleState.win)
            {
                Win();
            }

            // 패배
            else if (state == BattleState.lose)
            {
                Lose();
            }

            // 플레이어 턴
            if (turn == Turn.player)
            {
                switch (state)
                {
                    // 행동 선택
                    case BattleState.selectAction:
                        SelectAction();
                        break;

                    // 공격
                    case BattleState.Attack:
                        SkillAttack();
                        break;

                    // 몬스터볼 사용
                    case BattleState.useBall:
                        UseBall();
                        break;

                    // 상처약 사용
                    case BattleState.useMedi:
                        UseMedi();
                        break;

                    // 포켓몬 교체
                    case BattleState.switchPokemon:
                        SwitchPokemon();
                        break;

                    // 도망
                    case BattleState.run:
                        Run();
                        break;
                }
            }

            // 적 턴
            else if (turn == Turn.enemy)
            {
                // 기술 공격
                if (state == BattleState.Attack)
                {
                    SkillAttack();
                }
            }
        }

        // 출력 중일 때 한번 더 스페이스바 누르면 대사 전체 출력
        else if (bt.isPrint && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            bt.isPrint = false;

            // 플레이어의 턴일 때 행동 선택 상태에서 대사 출력 끝나고 선택창 활성화
            if (turn == Turn.player &&
                state == BattleState.selectAction &&
                progressNum == 0)
            {
                if (!selectView.activeSelf)
                {
                    selectView.SetActive(true);
                }
            }
        }
    }

    #region 전투 돌입
    void BattleEnter()
    {
        switch (progressNum)
        {
            case 1:
                bt.PrintBattleInfo($"가라! {MyPokemon.pokemon[0].pokemonName}!!");

                // 플레이어 애니메이션 재생
                player_Battle.enabled = true;

                GlobalValue.matchCount++;
                break;

            case 2:
                bt.PrintBattleInfo($"{GlobalValue.playerName}(은)는 {MyPokemon.pokemon[0].pokemonName}(을)를 꺼냈다.");

                // 정보창 비활성화 상태면 활성화
                if (!pokemonInfo.activeSelf) pokemonInfo.SetActive(true);
                if (!partnerInfo.activeSelf) partnerInfo.SetActive(true);

                // 턴에 따라 상태 변경
                if (turn == Turn.player)
                {
                    ChangeState(BattleState.selectAction);
                }

                else if (turn == Turn.enemy)
                {
                    ChangeState(BattleState.Attack);
                }
                break;
        }
    }
    #endregion

    #region 행동 선택
    void SelectAction()
    {
        switch (progressNum)
        {
            case 0:

                bt.PrintBattleInfo($"{MyPokemon.pokemon[0].pokemonName}(은)는 무엇을 할까?");

                // 정보창 비활성화 상태면 활성화
                if (!pokemonInfo.activeSelf) pokemonInfo.SetActive(true);
                if (!partnerInfo.activeSelf) partnerInfo.SetActive(true);
                break;
        }
    }
    #endregion

    #region 공격
    void SkillAttack()
    {
        // 플레이어 턴
        if (turn == Turn.player)
        {
            switch (progressNum)
            {
                case 0:
                    int accuracy = Random.Range(1, 101);

                    // 명중
                    if (accuracy <= MyPokemon.pokemon[0].skill[skillNum].skill_Accuracy)
                    {
                        enemyAnim.Play("EnemyDamage");

                        // 30% 확률로 급소공격
                        int critical = Random.Range(1, 11);

                        if (critical <= 3)
                        {
                            bt.PrintBattleInfo("급소에 맞았다!");
                            TakeDamage(enemyPokemon, Damage(MyPokemon.pokemon[0], skillNum, enemyPokemon, true));
                        }

                        else
                        {
                            bt.PrintBattleInfo("공격이 명중했다!");
                            TakeDamage(enemyPokemon, Damage(MyPokemon.pokemon[0], skillNum, enemyPokemon));
                        }
                    }

                    // 빗나감
                    else if (accuracy > MyPokemon.pokemon[0].skill[skillNum].skill_Accuracy)
                    {
                        bt.PrintBattleInfo("공격이 빗나갔다!");
                    }

                    ChangeTurn();
                    break;
            }
        }

        // 적 턴
        else if (turn == Turn.enemy)
        {
            switch (progressNum)
            {
                case 0:
                    // 랜덤으로 스킬 사용 (사용 횟수 차감 X)
                    ranSkill = Random.Range(0, enemyPokemon.skill.Length);

                    bt.PrintBattleInfo($"{enemyPokemon.pokemonName}(은)는 {enemyPokemon.skill[ranSkill].skill_Name}(을)를 사용했다!");

                    // 몸통박치기가 아니면
                    if (ranSkill != 0)
                    {
                        // 이펙트 실행
                        Instantiate(enemyPokemon.skill[ranSkill].E_Skill_Effect, skillEffect);
                    }

                    // 몸통박치기면
                    else if (ranSkill == 0)
                    {
                        // 애니메이션 실행
                        enemyAnim.Play("EnemyAttack");
                    }
                    break;

                case 1:
                    int accuracy = Random.Range(1, 101);

                    // 명중
                    if (accuracy <= enemyPokemon.skill[ranSkill].skill_Accuracy)
                    {
                        partnerAnim.Play("PartnerDamage");

                        // 30% 확률로 급소공격
                        int critical = Random.Range(1, 11);

                        if (critical <= 3)
                        {
                            bt.PrintBattleInfo("급소에 맞았다!");
                            TakeDamage(MyPokemon.pokemon[0], Damage(enemyPokemon, ranSkill, MyPokemon.pokemon[0], true));
                        }

                        else
                        {
                            bt.PrintBattleInfo("공격이 명중했다!");
                            TakeDamage(MyPokemon.pokemon[0], Damage(enemyPokemon, ranSkill, MyPokemon.pokemon[0]));
                        }
                    }

                    // 빗나감
                    else if (accuracy > enemyPokemon.skill[ranSkill].skill_Accuracy)
                    {
                        bt.PrintBattleInfo("공격이 빗나갔다!");
                    }

                    ChangeTurn();
                    break;
            }
        }
    }
    #endregion

    #region 아이템 사용
    // 몬스터볼
    void UseBall()
    {
        // 포획 성공 시
        if (catchSuccess)
        {
            switch (progressNum)
            {
                case 0:
                    // 포획 연출 끝나고
                    bt.PrintBattleInfo($"{GlobalValue.playerName}(은)는 {enemyPokemon.pokemonName}(을)를 얻었다!");

                    // 적 포켓몬 가져오기
                    MyPokemon.pokemon.Add(enemyPokemon);
                    break;

                case 1:
                    GlobalValue.winCount++;
                    GameManager.instance.endBattle = true;
                    fade.SetActive(true);
                    break;
            }
        }

        // 포획 실패 시
        else
        {
            switch (progressNum)
            {
                case 0:
                    // 포획 연출 끝나고
                    bt.PrintBattleInfo($"{GlobalValue.playerName}(은)는 {enemyPokemon.pokemonName} 포획에 실패했다...");
                    ChangeTurn();
                    break;
            }
        }
    }

    // 상처약
    void UseMedi()
    {
        switch (progressNum)
        {
            case 0:
                // 효과음 재생
                EMManager.instance.PlayEM("Medicine");

                bt.PrintBattleInfo($"{MyPokemon.pokemon[0].pokemonName}의 상처가 치료되었다!");

                // 회복
                Heal(MyPokemon.pokemon[0]);

                ChangeTurn();
                break;
        }
    }
    #endregion

    #region 포켓몬 교체
    void SwitchPokemon()
    {
        switch (progressNum)
        {
            case 0:
                bt.PrintBattleInfo($"가라! {MyPokemon.pokemon[0].pokemonName}!!");

                // 교체 연출
                monsterBallSwitch.SetActive(true);

                // UI 최신화
                partnerImg.sprite = MyPokemon.pokemon[0].image_Battle;
                nameTxt.text = MyPokemon.pokemon[0].pokemonName;

                ChangeState(BattleState.selectAction);
                break;
        }
    }
    #endregion

    #region 패배 교체
    void LoseSwitch()
    {
        switch (progressNum)
        {
            case 0:
                bt.PrintBattleInfo($"{MyPokemon.pokemon[0].pokemonName}(은)는 쓰러졌다!");
                partnerAnim.Play("PartnerDie");

                // UI 잠시 비활성화
                partnerInfo.SetActive(false);
                break;

            case 1:
                bt.PrintBattleInfo($"{GlobalValue.playerName}(은)는 {MyPokemon.pokemon[0].pokemonName} 대신\n" +
                                   $"{MyPokemon.pokemon[loseSwitchPokemon].pokemonName}(을)를 내보냈다!");

                // 포켓몬 교체
                MyPokemon.SwitchPokemon(loseSwitchPokemon);

                // 교체 연출
                monsterBallSwitch.SetActive(true);

                // UI 최신화
                partnerImg.sprite = MyPokemon.pokemon[0].image_Battle;
                nameTxt.text = MyPokemon.pokemon[0].pokemonName;

                ChangeState(BattleState.selectAction);
                break;
        }
    }
    #endregion

    #region 도망
    void Run()
    {
        switch (progressNum)
        {
            case 0:
                GlobalValue.loseCount++;
                GameManager.instance.endBattle = true;
                fade.SetActive(true);
                break;
        }
    }
    #endregion

    #region 상태 변경
    public void ChangeState(BattleState changeState)
    {
        state = changeState;
        progressNum = -1;
    }
    #endregion

    #region 턴 변경
    public void ChangeTurn()
    {
        // 플레이어 턴이었다면 적 턴으로
        if (turn == Turn.player)
        {
            turn = Turn.enemy;

            // 기술 공격 상태
            ChangeState(BattleState.Attack);
        }

        // 적 턴이었다면 플레이어 턴으로
        else if (turn == Turn.enemy)
        {
            turn = Turn.player;

            // 행동 선택
            ChangeState(BattleState.selectAction);
        }
    }
    #endregion

    #region 데미지 공식
    float Damage(Pokemon attacker, int skillNum, Pokemon defender, bool critical = false)
    {
        // 데미지 계산
        // (공격력 * 기술 위력)의 85% - (방어력의 85%)
        float damage
            = ((attacker.attack * attacker.skill[skillNum].skill_Damage) * 0.85f) - (defender.defence * 0.85f);

        // 급소 공격 시 30% 추가
        if (critical) damage *= 1.3f;

        // 최종 데미지는 반올림
        damage = Mathf.RoundToInt(damage);

        // 최소 데미지 3
        return Mathf.Max(3, damage);
    }
    #endregion

    #region 회복
    void Heal(Pokemon pokemon)
    {
        // 코루틴 중복 처리
        if (!coroutineIng)
        {
            StartCoroutine(HealCoroutine(pokemon));
        }
    }

    IEnumerator HealCoroutine(Pokemon pokemon)
    {
        coroutineIng = true;

        // 최대 체력 비례해서 늘어나는 속도 조절
        float getSpeed = pokemon.maxHP * Time.deltaTime;

        // 최대 체력까지 회복
        while (pokemon.nowHP <= pokemon.maxHP)
        {
            pokemon.nowHP += getSpeed;

            yield return null; // 다음 프레임까지 대기
        }

        // 연출 끝나고 다시 한번 값 넣기
        pokemon.nowHP = pokemon.maxHP;

        coroutineIng = false;
    }
    #endregion

    #region 피격
    void TakeDamage(Pokemon pokemon, float damage)
    {
        // 효과음 재생
        EMManager.instance.PlayEM("Damaged", 2, 0.17f);

        // 코루틴 중복 처리
        if (!coroutineIng)
        {
            StartCoroutine(TakeDamageCoroutine(pokemon, damage));
        }
    }

    IEnumerator TakeDamageCoroutine(Pokemon pokemon, float damage)
    {
        coroutineIng = true;

        // 최대 체력 비례해서 줄어드는 속도 조절
        float getSpeed = pokemon.maxHP * Time.deltaTime;

        while (damage > Mathf.Epsilon && pokemon.nowHP > 0.0f)
        {
            pokemon.nowHP -= getSpeed;
            damage -= getSpeed;

            // HP가 0이 되면 중단
            if (pokemon.nowHP <= 0f)
            {
                break;
            }

            yield return null; // 다음 프레임까지 대기
        }

        // 소수점 반올림
        pokemon.nowHP = Mathf.RoundToInt(pokemon.nowHP);

        // 체력 범위 제한
        pokemon.nowHP = Mathf.Clamp(pokemon.nowHP, 0, pokemon.maxHP);

        coroutineIng = false;
    }
    #endregion

    #region 교체 가능한 포켓몬 확인
    public bool HaveCanFightPokemon()
    {
        // 가지고 있는 포켓몬 중에
        for (int i = 0; i < MyPokemon.pokemon.Count; i++)
        {
            // 하나라도 싸울 수 있는 포켓몬이 있는 경우
            if (MyPokemon.pokemon[i].canFight)
            {
                loseSwitchPokemon = i;
                return true;
            }
        }

        return false;
    }
    #endregion

    #region 기술 사용 가능 횟수 소모 확인
    public bool ConsumeSkillCount()
    {
        int count = 0;

        for (int i = 0; i < MyPokemon.pokemon[0].skill.Length; i++)
        {
            count += MyPokemon.pokemon[0].skill[i].skill_Count;
        }

        return count <= 0;
    }
    #endregion

    #region 승리
    void Win()
    {
        switch (progressNum)
        {
            case 0:
                bt.PrintBattleInfo($"{enemyPokemon.pokemonName}(은)는 쓰러졌다!");
                enemyAnim.Play("EnemyDie");
                break;

            case 1:
                // 배경음 변경
                BGMManager.instance.PlayBattleWinBGM();

                bt.PrintBattleInfo($"{GlobalValue.playerName}(은)는 승리했다!!");
                GlobalValue.winCount++;
                break;

            case 2:
                bt.PrintBattleInfo($"{MyPokemon.pokemon[0].pokemonName}(은)는 경험치를 얻었다.");

                // 경험치 = (기본(5) + 플레이어 포켓몬 레벨 + 적 포켓몬 레벨) x 랜덤값
                float ran = Random.Range(1, 3);
                float getExp = (5 + (MyPokemon.pokemon[0].level + enemyPokemon.level)) * ran;

                // 반올림
                getExp = Mathf.Round(getExp);

                GetExp(getExp);

                if (levelUP)
                {
                    progressNum = 2;
                }

                else
                {
                    progressNum = 4;
                }
                break;

            case 3:
                // 효과음
                EMManager.instance.PlayEM("LevelUP");

                bt.PrintBattleInfo($"{MyPokemon.pokemon[0].pokemonName}의 레벨이 올랐다!");
                break;

            case 4:
                bt.PrintBattleInfo($"체력 + {increaseStat[0]}\n" +
                                   $"공격력 + {increaseStat[1]}\n" +
                                   $"방어력 + {increaseStat[2]}");
                break;

            case 5:
                bt.PrintBattleInfo($"{GlobalValue.playerName}(은)는 {GetMoney()}원을 얻었다!");
                break;

            case 6:
                GameManager.instance.endBattle = true;
                fade.SetActive(true);
                break;
        }
    }

    // 경험치 증가
    void GetExp(float exp)
    {
        // 코루틴 중복 처리
        if (!coroutineIng)
        {
            StartCoroutine(GetExpCoroutine(exp));
        }
    }

    IEnumerator GetExpCoroutine(float exp)
    {
        coroutineIng = true;

        // 만약 획득할 경험치가 레벨업에 필요한 경험치 이상이라면
        if (exp >= MyPokemon.pokemon[0].needEXP - MyPokemon.pokemon[0].nowEXP)
        {
            levelUP = true;
        }

        // 필요 경험치양에 비례해서 늘어나는 속도 조절
        float getSpeed = MyPokemon.pokemon[0].needEXP * Time.deltaTime;

        // 효과음 재생
        EMManager.instance.PlayEM("GetEXP");

        // 정해진 경험치양 만큼 경험치 증가
        while (exp >= 0.0f)
        {
            MyPokemon.pokemon[0].nowEXP += getSpeed;
            exp -= getSpeed;

            if (MyPokemon.pokemon[0].nowEXP >= MyPokemon.pokemon[0].needEXP)
            {
                LevelUp();
            }

            yield return null; // 다음 프레임까지 대기
        }

        // 최종적으로 경험치 정수화
        MyPokemon.pokemon[0].nowEXP = Mathf.RoundToInt(MyPokemon.pokemon[0].nowEXP);

        coroutineIng = false;
    }

    // 레벨업
    void LevelUp()
    {
        // 현재 경험치 초기화
        MyPokemon.pokemon[0].nowEXP = 0.0f;

        // 필요 경험치 증가
        float needExp = MyPokemon.pokemon[0].needEXP;
        MyPokemon.pokemon[0].needEXP += needExp * 0.6f;

        // 반올림
        MyPokemon.pokemon[0].needEXP = Mathf.RoundToInt(MyPokemon.pokemon[0].needEXP);

        // 레벨 증가
        MyPokemon.pokemon[0].level++;

        // 스탯 증가
        StatUp(MyPokemon.pokemon[0]);

        // 기술 사용 가능 횟수 회복
        for (int i = 0; i < MyPokemon.pokemon[0].skill.Length; i++)
        {
            MyPokemon.pokemon[0].skill[i].skill_Count = MyPokemon.pokemon[0].skill[i].skill_MaxCount;
        }
    }

    // 스탯 증가
    void StatUp(Pokemon pokemon)
    {
        // 레벨에 따라 스텟 증가량 결정
        float levelValue = ((MyPokemon.pokemon[0].level - 1) * 5) + 5;

        // 포켓몬 별 가중 치
        switch (pokemon.pokemonName)
        {
            case "이상해씨": growthWeight[0] = 1.0f; growthWeight[1] = 0.6f; growthWeight[2] = 1.4f; break;
            case "파이리": growthWeight[0] = 0.9f; growthWeight[1] = 1.5f; growthWeight[2] = 0.6f; break;
            case "꼬부기": growthWeight[0] = 1.1f; growthWeight[1] = 1.0f; growthWeight[2] = 1.0f; break;
            case "피카츄": growthWeight[0] = 1.0f; growthWeight[1] = 1.3f; growthWeight[2] = 0.8f; break;
            case "꼬렛": growthWeight[0] = 0.8f; growthWeight[1] = 1.0f; growthWeight[2] = 0.8f; break;
        }

        for (int i = 0; i < 3; i++)
        {
            int ran = Random.Range(1, 101);

            // 최대 수치로 증가 (10%)
            if (ran <= 10)
            {
                increase = 0.3f;
            }

            // 약간 증가한 수치로 증가 (30%)
            else if (11 <= ran && ran <= 40)
            {
                increase = 0.2f;
            }

            // 기본 수치로 증가 (60%)
            else if (41 <= ran)
            {
                increase = 0.1f;
            }

            // 스탯 증가
            switch (i)
            {
                case 0:
                    // 증가값 올림
                    increaseStat[0] = Mathf.RoundToInt(levelValue * increase * growthWeight[0]);
                    pokemon.maxHP += increaseStat[0];

                    // 체력 최대치로 회복
                    pokemon.nowHP = pokemon.maxHP;
                    break;

                case 1:
                    // 증가값 올림
                    increaseStat[1] = Mathf.RoundToInt(levelValue * increase * growthWeight[1]);
                    pokemon.attack += increaseStat[1];
                    break;

                case 2:
                    // 증가값 올림
                    increaseStat[2] = Mathf.RoundToInt(levelValue * increase * growthWeight[2]);
                    pokemon.defence += increaseStat[2];
                    break;
            }
        }
    }

    // 돈 증가
    int GetMoney()
    {
        // 적 포켓몬의 레벨에 비례해서 돈 증가
        int ranMoney = Random.Range(1, 6);
        ranMoney += enemyPokemon.level;
        GlobalValue.playerMoney += ranMoney * 50;
        return ranMoney * 50;
    }
    #endregion

    #region 패배
    void Lose()
    {
        switch (progressNum)
        {
            case 0:
                bt.PrintBattleInfo($"{MyPokemon.pokemon[0].pokemonName}(은)는 쓰러졌다!");
                partnerAnim.Play("PartnerDie");
                break;

            case 1:
                bt.PrintBattleInfo("더 이상 싸울 수 있는 포켓몬이 없다...");
                break;

            case 2:
                bt.PrintBattleInfo($"{GlobalValue.playerName}(은)는 도망쳤다.");
                GlobalValue.loseCount++;
                break;

            case 3:
                GameManager.instance.endBattle = true;
                fade.SetActive(true);
                break;
        }
    }
    #endregion
}
