using Core;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

/// <summary>
/// 主菜单界面
/// </summary>
public class MainMenuView : UIBase
{

    public override void UIAwake()
    {
        base.UIAwake();

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
    }

    private async UniTask LoadBattleScene()
    {
        await ManagerScene.LoadSceneAsync(ConfigScenes.unitySceneBattle2Team);
        CloseUIForm();

        //创建技能
        SkillNormalAttack skillNormalAttack = new SkillNormalAttack();//普通攻击
        skillNormalAttack.ID = 0;
        skillNormalAttack.Name = "普通攻击";
        skillNormalAttack.Des = "普通攻击技能的描述";

        //自己人
        //创建一名角色
        RoleBattlerPlayer rolePlayer = new RoleBattlerPlayer();
        rolePlayer.ID = 1;
        rolePlayer.Name = "玩家1";
        ISkillCarrier.AddSkill(rolePlayer, skillNormalAttack);
        //创建一名NPC->队友
        RoleBattleNPC npc1 = new RoleBattleNPC();
        npc1.ID = 2;
        npc1.Name = "NPC1";
        ISkillCarrier.AddSkill(npc1, skillNormalAttack);

        //敌人


        //创建一只队伍
        TeamTypeOne teamTypeOne = new TeamTypeOne();
        teamTypeOne.AddRole(rolePlayer);
        teamTypeOne.AddRole(npc1);

        //添加一场战斗
        TwoTeamBattle twoTeamBattle = new TwoTeamBattle();
        twoTeamBattle.AddBattleTeam(teamTypeOne);

        //添加到战斗管理器
        ManagerRPGBattle.AddBattle(twoTeamBattle);

    }
}
