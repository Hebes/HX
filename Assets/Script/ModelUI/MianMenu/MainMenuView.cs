using Core;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using ExpansionUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = Core.ExtensionDebug;

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
        T_Exit.GetButton().onClick.AddListener(() => { CoreBehaviour.AddCoroutine(Exit()); });
        T_Battle.GetButton().onClick.AddListener(Battle);
    }



    private void Battle()
    {
        CoreBehaviour.AddCoroutine(LoadBattleScene());
    }
    private IEnumerator Exit()
    {
        CoreBehaviour.StopAllCoroutines();      //停止所有协程
        yield return ModelRun.ModelExit();                   //退出所有模块

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    private void Setting()
    {

    }
    private void Load()
    {

    }
    private void StartGame()
    {
        CoreBehaviour.AddCoroutine(LoadScene());
    }



    private IEnumerator LoadScene()
    {
        //await ManagerScene.LoadScene(ConfigScenes.unitySceneStart);
        yield return ManagerScene.LoadScene(ConfigScenes.unitySceneMain, LoadSceneMode.Additive);
        CloseUIForm();
    }

    private IEnumerator LoadBattleScene()
    {
        CloseUIForm();
        yield return CoreScene.LoadSceneAsync(ConfigScenes.unitySceneBattle, LoadSceneMode.Additive);
        //await ManagerScene.LoadScene(ConfigScenes.unitySceneBattle);
        //SceneBattleManager sceneBattleManager = SceneBattleManager.Instance;
        //Debug.Log("准备开始战斗");
        ////创建技能
        //SkillNormalAttack skillNormalAttack = new SkillNormalAttack();//普通攻击
        //skillNormalAttack.ID = 0;
        //skillNormalAttack.Name = "普通攻击";
        //skillNormalAttack.Des = "普通攻击技能的描述";
        //skillNormalAttack.SkillType = ESkillType.NormalAttack;
        //skillNormalAttack.SkillInit();


        //BattleData teamBattle = new BattleData();   //创建一场战斗
        //teamBattle.ID = 1;


        //TeamData enemyTeam = new TeamData();  //创建敌人队伍
        //enemyTeam.TeamPoint = ETeamPoint.Right1;
        //enemyTeam.TeamType = ERoleOrTeamType.Enemy;


        //TeamData ownTeam = new TeamData();    //创建自己队伍   
        //ownTeam.TeamPoint = ETeamPoint.Left1;
        //ownTeam.TeamType = ERoleOrTeamType.NPC;



        ////自己人
        ////创建一名角色
        ////RoleBattlerPlayer rolePlayer = new RoleBattlerPlayer();
        ////rolePlayer.ID = 1;
        ////rolePlayer.Name = "玩家1";
        ////rolePlayer.RoleType = ERoleType.Player;
        ////rolePlayer.Max_colldown = 5;
        ////rolePlayer.MaxHP = 100;
        ////rolePlayer.CurrentHP = 100;
        ////rolePlayer.RoleBattlePoint = ERoleBattlePoint.Point1;
        ////ISkillCarrier.AddSkill(rolePlayer, skillNormalAttack);

        //RoleNPC npc1 = new RoleNPC();//创建一名NPC->队友
        //ownTeam.AddRole(npc1);
        //npc1.SetRole(2, HelperName.GetManName(), ERoleOrTeamType.NPC, ERoleBattlePoint.Point1);
        //npc1.Team = ownTeam;
        //AttributesData roleAttributes1 = new AttributesData();
        //roleAttributes1.MaxHP = 100;
        //roleAttributes1.CurrentHP = 100;
        //roleAttributes1.CurColldown = 0; roleAttributes1.MaxColldown = 5;
        //npc1.SetRoleAttributes(roleAttributes1);
        //npc1.AddSkill(skillNormalAttack);
        //npc1.SwitchRoleState<RoleStateBattle>().SetBattleData(teamBattle);


        //RoleNPC npc2 = new RoleNPC();//创建一名NPC->队友
        //ownTeam.AddRole(npc2);
        //npc2.Team = ownTeam;
        //npc2.SetRole(3, HelperName.GetManName(), ERoleOrTeamType.NPC, ERoleBattlePoint.Point2);
        //AttributesData roleAttributes2 = new AttributesData();
        //roleAttributes2.CurrentHP = roleAttributes2.MaxHP = 100;
        //roleAttributes2.CurColldown = 0; roleAttributes2.MaxColldown = 5;
        //npc2.SetRoleAttributes(roleAttributes2);
        //npc2.AddSkill(skillNormalAttack);
        //npc2.SwitchRoleState<RoleStateBattle>().SetBattleData(teamBattle);


        ////敌人
        //RoleEnemy enemy1 = new RoleEnemy();
        //enemyTeam.AddRole(enemy1);
        //enemy1.Team = enemyTeam;
        //enemy1.SetRole(4, HelperName.GetManName(), ERoleOrTeamType.Enemy, ERoleBattlePoint.Point1);
        //AttributesData roleAttributes3 = new AttributesData();
        //roleAttributes3.CurrentHP = roleAttributes3.MaxHP = 100;
        //roleAttributes3.CurColldown = 0; roleAttributes3.MaxColldown = 5;
        //enemy1.SetRoleAttributes(roleAttributes3);
        //enemy1.AddSkill(skillNormalAttack);
        //enemy1.SwitchRoleState<RoleStateBattle>().SetBattleData(teamBattle);

        //RoleEnemy enemy2 = new RoleEnemy();
        //enemyTeam.AddRole(enemy2);
        //enemy2.Team = enemyTeam;
        //enemy2.SetRole(5, HelperName.GetManName(), ERoleOrTeamType.Enemy, ERoleBattlePoint.Point2);
        //AttributesData roleAttributes4 = new AttributesData();
        //roleAttributes4.CurrentHP = roleAttributes4.MaxHP = 100;
        //roleAttributes4.CurColldown = 0; roleAttributes4.MaxColldown = 5;
        //enemy2.SetRoleAttributes(roleAttributes4);
        //enemy2.AddSkill(skillNormalAttack);
        //enemy2.SwitchRoleState<RoleStateBattle>().SetBattleData(teamBattle);


        ////添加一场战斗
        //teamBattle.AddBattleTeam(ownTeam);
        //teamBattle.AddBattleTeam(enemyTeam);



        ////添加到战斗管理器
        //SceneBattleManager.Instance.SetBattle(teamBattle);
        //ManagerRPGBattle.AddBattle(teamBattle);
    }
}
