using Framework.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 玩家移动状态，NPC或者其他移动请用RoleStateRun
/// </summary>
public class RoleStatePlayerRun : IRoleState
{
    #region 接口字段和属性
    public long ID { get ; set ; }
    public ERoleSateType RoleSateType => ERoleSateType.Run;
    public RoleData RoleData { get; set; }
    #endregion


    #region 本类特有
    /// <summary>
    /// 玩家输入是否禁用
    /// </summary>
    private bool _playerInputIsDisabled { get; set; } = false;
    private float xInput { get; set; }
    private float yInput { get; set; }
    /// <summary>
    /// 移动速度
    /// </summary>
    private float movementSpeed { get; set; }

    private bool isCarrying = false;
    private bool isIdle;
    private bool isLiftingToolDown;
    private bool isLiftingToolLeft;
    private bool isLiftingToolRight;
    private bool isLiftingToolUp;
    private bool isRunning;
    private bool isUsingToolDown;
    private bool isUsingToolLeft;
    private bool isUsingToolRight;
    private bool isUsingToolUp;
    private bool isSwingingToolDown;
    private bool isSwingingToolLeft;
    private bool isSwingingToolRight;
    private bool isSwingingToolUp;
    private bool isWalking;
    private bool isPickingUp;
    private bool isPickingDown;
    private bool isPickingLeft;
    private bool isPickingRight;


    /// <summary>
    /// 方向
    /// </summary>
    private Direction playerDirection { get; set; }
    /// <summary>
    /// 禁用玩家工具使用
    /// </summary>
    private bool playerToolUseDisabled = false;
    #endregion


    #region 接口方法
    public void StateEnter()
    {
        RoleData.gameObject = CoreResource.Load<GameObject>(ConfigPrefab.prefabCommonRole);
    }
    public void StateExit()
    {
    }
    public void StateUpdata()
    {
        if (_playerInputIsDisabled) return;
    }
    #endregion


    #region 本类私有方法
    /// <summary>
    /// 重置动画触发器
    /// </summary>
    private void ResetAnimationTriggers()
    {
        isPickingRight = false;
        isPickingLeft = false;
        isPickingUp = false;
        isPickingDown = false;
        isUsingToolRight = false;
        isUsingToolLeft = false;
        isUsingToolUp = false;
        isUsingToolDown = false;
        isLiftingToolRight = false;
        isLiftingToolLeft = false;
        isLiftingToolUp = false;
        isLiftingToolDown = false;
        isSwingingToolRight = false;
        isSwingingToolLeft = false;
        isSwingingToolUp = false;
        isSwingingToolDown = false;
        //toolEffect = ToolEffect.none;
    }
    /// <summary>
    /// 玩家动作输入
    /// </summary>
    private void PlayerMovementInput()
    {
        yInput = Input.GetAxisRaw("Vertical");
        xInput = Input.GetAxisRaw("Horizontal");

        if (yInput != 0 && xInput != 0)
        {
            xInput = xInput * 0.71f;
            yInput = yInput * 0.71f;
        }

        if (xInput != 0 || yInput != 0)
        {
            isRunning = true;
            isWalking = false;
            isIdle = false;
            movementSpeed = RolePlayerSettings.runningSpeed;

            // 捕捉玩家的方向保存游戏
            if (xInput < 0)
                playerDirection = Direction.left;
            else if (xInput > 0)
                playerDirection = Direction.right;
            else if (yInput < 0)
                playerDirection = Direction.down;
            else
                playerDirection = Direction.up;
        }
        else if (xInput == 0 && yInput == 0)
        {
            isRunning = false;
            isWalking = false;
            isIdle = true;
        }
    }
    /// <summary>
    /// 输入玩家行走
    /// </summary>
    private void PlayerWalkInput()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            isRunning = false;
            isWalking = true;//上面两个键按下走路
            isIdle = false;
            movementSpeed = RolePlayerSettings.walkingSpeed;
        }
        else
        {
            isRunning = true;//上面两个键没按下跑步
            isWalking = false;
            isIdle = false;
            movementSpeed = RolePlayerSettings.runningSpeed;
        }
    }
    /// <summary>
    /// 玩家点击输入
    /// </summary>
    private void PlayerClickInput()
    {
        if (playerToolUseDisabled) return;
        if (Input.GetMouseButton(0))
        {
            //if (gridCursor.CursorIsEnabled || cursor.CursorIsEnabled)
            //{
            //    // Get Cursor Grid Position
            //    Vector3Int cursorGridPosition = gridCursor.GetGridPositionForCursor();

            //    // Get Player Grid Position
            //    Vector3Int playerGridPosition = gridCursor.GetGridPositionForPlayer();

            //    ProcessPlayerClickInput(cursorGridPosition, playerGridPosition);
            //}
        }
    }
    /// <summary>
    /// 进程播放器点击输入
    /// </summary>
    /// <param name="cursorGridPosition"></param>
    /// <param name="playerGridPosition"></param>
    private void ProcessPlayerClickInput(Vector3Int cursorGridPosition, Vector3Int playerGridPosition)
    {
        //ResetMovement();

        //Vector3Int playerDirection = GetPlayerClickDirection(cursorGridPosition, playerGridPosition);

        //// Get Grid property details at cursor position (the GridCursor validation routine ensures that grid property details are not null)
        //GridPropertyDetails gridPropertyDetails = GridPropertiesManager.Instance.GetGridPropertyDetails(cursorGridPosition.x, cursorGridPosition.y);

        //// Get Selected item details
        //ItemDetails itemDetails = InventoryManager.Instance.GetSelectedInventoryItemDetails(InventoryLocation.player);

        //if (itemDetails != null)
        //{
        //    switch (itemDetails.itemType)
        //    {
        //        case ItemType.Seed:
        //            if (Input.GetMouseButtonDown(0))
        //            {
        //                ProcessPlayerClickInputSeed(gridPropertyDetails, itemDetails);
        //            }
        //            break;

        //        case ItemType.Commodity:
        //            if (Input.GetMouseButtonDown(0))
        //            {
        //                ProcessPlayerClickInputCommodity(itemDetails);
        //            }
        //            break;

        //        case ItemType.Watering_tool:
        //        case ItemType.Breaking_tool:
        //        case ItemType.Chopping_tool:
        //        case ItemType.Hoeing_tool:
        //        case ItemType.Reaping_tool:
        //        case ItemType.Collecting_tool:
        //            ProcessPlayerClickInputTool(gridPropertyDetails, itemDetails, playerDirection);
        //            break;

        //        case ItemType.none:
        //            break;

        //        case ItemType.count:
        //            break;

        //        default:
        //            break;
        //    }
        //}
    }
    /// <summary>
    /// 获取玩家点击的方向
    /// </summary>
    /// <param name="cursorGridPosition"></param>
    /// <param name="playerGridPosition"></param>
    /// <returns></returns>
    private Vector3Int GetPlayerClickDirection(Vector3Int cursorGridPosition, Vector3Int playerGridPosition)
    {
        if (cursorGridPosition.x > playerGridPosition.x)
        {
            return Vector3Int.right;
        }
        else if (cursorGridPosition.x < playerGridPosition.x)
        {
            return Vector3Int.left;
        }
        else if (cursorGridPosition.y > playerGridPosition.y)
        {
            return Vector3Int.up;
        }
        else
        {
            return Vector3Int.down;
        }
    }
    /// <summary>
    /// 获取玩家点击方向
    /// </summary>
    /// <param name="cursorPosition"></param>
    /// <param name="playerPosition"></param>
    /// <returns></returns>
    private Vector3Int GetPlayerDirection(Vector3 cursorPosition, Vector3 playerPosition)
    {
        return default;
        //if (

        //    cursorPosition.x > playerPosition.x
        //    &&
        //    cursorPosition.y < (playerPosition.y + cursor.ItemUseRadius / 2f)
        //    &&
        //    cursorPosition.y > (playerPosition.y - cursor.ItemUseRadius / 2f)
        //    )
        //{
        //    return Vector3Int.right;
        //}
        //else if (
        //    cursorPosition.x < playerPosition.x
        //    &&
        //    cursorPosition.y < (playerPosition.y + cursor.ItemUseRadius / 2f)
        //    &&
        //    cursorPosition.y > (playerPosition.y - cursor.ItemUseRadius / 2f)
        //    )
        //{
        //    return Vector3Int.left;
        //}
        //else if (cursorPosition.y > playerPosition.y)
        //{
        //    return Vector3Int.up;
        //}
        //else
        //{
        //    return Vector3Int.down;
        //}
    }
    /// <summary>
    /// 进程播放器点击输入种子
    /// </summary>
    /// <param name="gridPropertyDetails"></param>
    /// <param name="itemDetails"></param>
    //private void ProcessPlayerClickInputSeed(GridPropertyDetails gridPropertyDetails, ItemDetails itemDetails)
    //{
    //    //if (itemDetails.canBeDropped && gridCursor.CursorPositionIsValid && gridPropertyDetails.daysSinceDug > -1 && gridPropertyDetails.seedItemCode == -1)
    //    //{
    //    //    PlantSeedAtCursor(gridPropertyDetails, itemDetails);
    //    //}
    //    //else if (itemDetails.canBeDropped && gridCursor.CursorPositionIsValid)
    //    //{
    //    //    EventHandler.CallDropSelectedItemEvent();
    //    //}
    //}

    //在光标处播种
    //private void PlantSeedAtCursor(GridPropertyDetails gridPropertyDetails, ItemDetails itemDetails)
    //{
    //    // Process if we have cropdetails for the seed
    //    if (GridPropertiesManager.Instance.GetCropDetails(itemDetails.itemCode) != null)
    //    {
    //        // Update grid properties with seed details
    //        gridPropertyDetails.seedItemCode = itemDetails.itemCode;
    //        gridPropertyDetails.growthDays = 0;

    //        // Display planted crop at grid property details
    //        GridPropertiesManager.Instance.DisplayPlantedCrop(gridPropertyDetails);

    //        // Remove item from inventory
    //        EventHandler.CallRemoveSelectedItemFromInventoryEvent();

    //        // Make planting sound
    //        AudioManager.Instance.PlaySound(SoundName.effectPlantingSound);

    //    }
    //}

    //private void ProcessPlayerClickInputCommodity(ItemDetails itemDetails)
    //{
    //    if (itemDetails.canBeDropped && gridCursor.CursorPositionIsValid)
    //    {
    //        EventHandler.CallDropSelectedItemEvent();
    //    }
    //}

    //private void ProcessPlayerClickInputTool(GridPropertyDetails gridPropertyDetails, ItemDetails itemDetails, Vector3Int playerDirection)
    //{
    //    // Switch on tool
    //    switch (itemDetails.itemType)
    //    {
    //        case ItemType.Hoeing_tool:
    //            if (gridCursor.CursorPositionIsValid)
    //            {
    //                HoeGroundAtCursor(gridPropertyDetails, playerDirection);
    //            }
    //            break;

    //        case ItemType.Watering_tool:
    //            if (gridCursor.CursorPositionIsValid)
    //            {
    //                WaterGroundAtCursor(gridPropertyDetails, playerDirection);
    //            }
    //            break;

    //        case ItemType.Chopping_tool:
    //            if (gridCursor.CursorPositionIsValid)
    //            {
    //                ChopInPlayerDirection(gridPropertyDetails, itemDetails, playerDirection);
    //            }
    //            break;


    //        case ItemType.Collecting_tool:
    //            if (gridCursor.CursorPositionIsValid)
    //            {
    //                CollectInPlayerDirection(gridPropertyDetails, itemDetails, playerDirection);
    //            }
    //            break;

    //        case ItemType.Breaking_tool:
    //            if (gridCursor.CursorPositionIsValid)
    //            {
    //                BreakInPlayerDirection(gridPropertyDetails, itemDetails, playerDirection);
    //            }
    //            break;

    //        case ItemType.Reaping_tool:
    //            if (cursor.CursorPositionIsValid)
    //            {
    //                playerDirection = GetPlayerDirection(cursor.GetWorldPositionForCursor(), GetPlayerCentrePosition());
    //                ReapInPlayerDirectionAtCursor(itemDetails, playerDirection);
    //            }
    //            break;


    //        default:
    //            break;
    //    }
    //}

    //private void HoeGroundAtCursor(GridPropertyDetails gridPropertyDetails, Vector3Int playerDirection)
    //{
    //    //Play sound
    //    AudioManager.Instance.PlaySound(SoundName.effectHoe);

    //    // Trigger animation
    //    StartCoroutine(HoeGroundAtCursorRoutine(playerDirection, gridPropertyDetails));
    //}

    //private IEnumerator HoeGroundAtCursorRoutine(Vector3Int playerDirection, GridPropertyDetails gridPropertyDetails)
    //{
    //    PlayerInputIsDisabled = true;
    //    playerToolUseDisabled = true;

    //    // Set tool animation to hoe in override animation
    //    toolCharacterAttribute.partVariantType = PartVariantType.hoe;
    //    characterAttributeCustomisationList.Clear();
    //    characterAttributeCustomisationList.Add(toolCharacterAttribute);
    //    animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeCustomisationList);

    //    if (playerDirection == Vector3Int.right) 
    //    {
    //        isUsingToolRight = true;
    //    }
    //    else if (playerDirection == Vector3Int.left)
    //    {
    //        isUsingToolLeft = true;
    //    }
    //    else if (playerDirection == Vector3Int.up)
    //    {
    //        isUsingToolUp = true;
    //    }
    //    else if (playerDirection == Vector3Int.down)
    //    {
    //        isUsingToolDown = true;
    //    }

    //    yield return useToolAnimationPause;

    //    // Set Grid property details for dug ground
    //    if (gridPropertyDetails.daysSinceDug == -1)
    //    {
    //        gridPropertyDetails.daysSinceDug = 0;
    //    }

    //    // Set grid property to dug
    //    GridPropertiesManager.Instance.SetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY, gridPropertyDetails);

    //    // Display dug grid tiles
    //    GridPropertiesManager.Instance.DisplayDugGround(gridPropertyDetails);


    //    // After animation pause
    //    yield return afterUseToolAnimationPause;

    //    PlayerInputIsDisabled = false;
    //    playerToolUseDisabled = false;
    //}

    //private void WaterGroundAtCursor(GridPropertyDetails gridPropertyDetails, Vector3Int playerDirection)
    //{
    //    //Play sound
    //    AudioManager.Instance.PlaySound(SoundName.effectWateringCan);

    //    // Trigger animation
    //    StartCoroutine(WaterGroundAtCursorRoutine(playerDirection, gridPropertyDetails));
    //}
    #endregion
}


/// <summary>
/// 玩家角色设置
/// </summary>
public static class RolePlayerSettings
{
    #region 玩家移动
    /// <summary>
    /// 跑步速度
    /// </summary>
    public const float runningSpeed = 5.333f;
    /// <summary>
    /// 步行速度
    /// </summary>
    public const float walkingSpeed = 2.666f;
    /// <summary>
    /// 使用工具动画暂停时间
    /// </summary>
    public static float useToolAnimationPause = 0.25f;
    /// <summary>
    /// 接触工具暂定等待时间
    /// </summary>
    public static float liftToolAnimationPause = 0.4f;
    /// <summary>
    /// 拾取动画等待时间
    /// </summary>
    public static float pickAnimationPause = 1f;
    /// <summary>
    /// 在使用工具之后的等待时间
    /// </summary>
    public static float afterUseToolAnimationPause = 0.2f;
    public static float afterLiftToolAnimationPause = 0.4f;
    public static float afterPickAnimationPause = 0.2f;
    #endregion

}