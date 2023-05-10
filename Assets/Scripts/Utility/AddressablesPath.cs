using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class AddressablesPath
{
    public static string NotificationCellPath = "UI/NotificationCell";
    #region Loot GO
    public static string EtherPath = "Ether";
    public static string meetPath = "Loot/Item/Meat";
    #endregion

    #region Loot SO
    public static string meatSOPath = "Loot/Item/MeatSO";
    #endregion

    public static string BaseScene = "Scene/BaseScene";

    public static string OpenWorldScene = "Scene/OpenWorld";
    public static string MainMenuScene = "Scene/MainMenu";
    public static string LoadingScene = "Scene/LoadingScreen";

    #region Loading
    public static string @conduit_video = "Loading/conduit";
    public static string @swirl_plant_video = "Loading/swirl-plants";
    #endregion

    #region UI
    public static string inventorySlotCellPath = "UI/Inv-Cell";
    public static string weaponInventoryCell = "UI/Weapon-Inv-Cell";
    public static string ui_inv_meat = "UI/Inv/Meat";
    public static string damagePopup = "UI/DamagePopup";
    #endregion



    public static async void ClearOps(IEnumerable<AsyncOperationHandle> ops)
    {
        foreach (var op in ops)
        {
            Addressables.Release(op);
        }
        await Task.Yield();
    }

    public static async void ClearOps(AsyncOperationHandle op)
    {
        Addressables.Release(op);
        await Task.Yield();
    }
}
