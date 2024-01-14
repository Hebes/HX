using Core;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 主菜单界面
/// </summary>
public class MainMenuView : UIBase, IUIAwake
{

    public void UIAwake()
    {
        InitUIBase(EUIType.Fixed, EUIMode.HideOther, EUILucenyType.ImPenetrable);

        UIComponent UIComponent = gameObject.GetComponent<UIComponent>();

        GameObject T_StartGame = UIComponent.Get<GameObject>("T_StartGame");
        GameObject T_Load = UIComponent.Get<GameObject>("T_Load");
        GameObject T_Setting = UIComponent.Get<GameObject>("T_Setting");
        GameObject T_Exit = UIComponent.Get<GameObject>("T_Exit");
        GameObject T_Battle = UIComponent.Get<GameObject>("T_Battle");

        T_StartGame.GetButton().onClick.AddListener(StartGame);
        T_Load.GetButton().onClick.AddListener(Load);
        T_Setting.GetButton().onClick.AddListener(Setting);
        T_Exit.GetButton().onClick.AddListener(Exit);
        T_Battle.GetButton().onClick.AddListener(Battle);
    }



    private void Battle()
    {
        LoadBattleScene().Forget();
    }
    private void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
        CoreBehaviour.StopAllCoroutines();      //停止所有协程
    }
    private void Setting()
    {

    }
    private void Load()
    {

    }
    private void StartGame()
    {
        LoadScene().Forget();
    }



    private async UniTask LoadScene()
    {
        await ManagerScene.LoadSceneAsync(ConfigScenes.unitySceneStart);
        CloseUIForm();
        await ManagerScene.LoadSceneAsync(ConfigScenes.unitySceneMain, ELoadSceneModel.Additive);
    }

    private async UniTask LoadBattleScene()
    {
        CloseUIForm();
        await ManagerScene.LoadSceneAsync(ConfigScenes.unitySceneBattle);
        SceneBattleManager sceneBattleManager = SceneBattleManager.Instance;

        Battle teamBattle = new Battle();   //创建一场战斗
        TeamTypeOne enemyTeam = new TeamTypeOne();  //创建敌人队伍
        TeamTypeOne ownTeam = new TeamTypeOne();    //创建自己队伍   

        //测试战斗
        //创建技能
        SkillNormalAttack skillNormalAttack = new SkillNormalAttack();//普通攻击
        skillNormalAttack.ID = 0;
        skillNormalAttack.Name = "普通攻击";
        skillNormalAttack.Des = "普通攻击技能的描述";
        skillNormalAttack.SkillType = ESkillType.NormalAttack;
        skillNormalAttack.SkillInit();


        //自己人
        //创建一名角色
        //RoleBattlerPlayer rolePlayer = new RoleBattlerPlayer();
        //rolePlayer.ID = 1;
        //rolePlayer.Name = "玩家1";
        //rolePlayer.RoleType = ERoleType.Player;
        //rolePlayer.Max_colldown = 5;
        //rolePlayer.MaxHP = 100;
        //rolePlayer.CurrentHP = 100;
        //rolePlayer.RoleBattlePoint = ERoleBattlePoint.Point1;
        //ISkillCarrier.AddSkill(rolePlayer, skillNormalAttack);

        RoleNPC npc1 = new RoleNPC();//创建一名NPC->队友
        npc1.ID = 2;
        npc1.Name = "NPC1";
        npc1.RoleType = ERoleType.NPC;
        npc1.Max_colldown = 5;
        npc1.MaxHP = 100;
        npc1.CurrentHP = 100;
        npc1.RoleBattlePoint = ERoleBattlePoint.Point1;
        ISkillCarrier.AddSkill(npc1, skillNormalAttack);

        RoleNPC npc2 = new RoleNPC();//创建一名NPC->队友
        npc2.ID = 3;
        npc2.Name = "NPC2";
        npc2.RoleType = ERoleType.NPC;
        npc2.Max_colldown = 5;
        npc2.MaxHP = 100;
        npc2.CurrentHP = 100;
        npc2.RoleBattlePoint = ERoleBattlePoint.Point2;
        ISkillCarrier.AddSkill(npc2, skillNormalAttack);



        //敌人
        RoleEnemy enemy1 = new RoleEnemy();
        enemy1.RoleBattleEnemyInit(3, "敌人1", ERoleType.Enemy, ERoleBattlePoint.Point1, 5);
        enemy1.MaxHP = 100;
        enemy1.CurrentHP = 100;
        ISkillCarrier.AddSkill(enemy1, skillNormalAttack);//添加技能

        RoleEnemy enemy2 = new RoleEnemy();
        enemy2.RoleBattleEnemyInit(4, "敌人2", ERoleType.Enemy, ERoleBattlePoint.Point2, 5);
        enemy2.MaxHP = 100;
        enemy2.CurrentHP = 100;
        enemy2.AddSkill(skillNormalAttack);


        //创建队伍
        //自己队伍
        ownTeam.TeamPoint = ETeamPoint.Left1;
        ownTeam.TeamType = ETeamType.Player;
        ownTeam.AddRole(npc1);
        ownTeam.AddRole(npc2);

        //敌人队伍
        enemyTeam.TeamPoint = ETeamPoint.Right1;
        enemyTeam.TeamType = ETeamType.Enemy;
        enemyTeam.AddRole(enemy1);
        enemyTeam.AddRole(enemy2);


        //添加一场战斗
        teamBattle.ID = 1;//一场战斗的编号
        teamBattle.AddBattleTeam(ownTeam);
        teamBattle.AddBattleTeam(enemyTeam);

        npc1.AddData(teamBattle, ownTeam);
        npc2.AddData(teamBattle, ownTeam);

        enemy1.AddData(teamBattle, enemyTeam);
        enemy2.AddData(teamBattle, enemyTeam);

        //添加到战斗管理器
        sceneBattleManager.SetBattle(teamBattle);
        ManagerRPGBattle.AddBattle(teamBattle);
    }
}
