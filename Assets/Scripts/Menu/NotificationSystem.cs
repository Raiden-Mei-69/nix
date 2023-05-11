using DG.Tweening;
using Loot;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NotificationSystem : MonoBehaviour
{
    public static NotificationSystem Instance;

    public int MaxStackNotif = 5;
    public float timeDisplay = 5f;
    [SerializeField] private List<NotificationCell> cells = new();
    [SerializeField] LootDrop loot;
    [SerializeField] private Transform contentHolder;
    [SerializeField] private Transform DisapearPos;


    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    //private void Update()
    //{
    //    if (Keyboard.current.numpad9Key.wasPressedThisFrame)
    //    {
    //        CreateNewNotif(loot.lootInfo._itemSO.Icon, $"{loot.lootInfo._itemSO.Name} X{loot.lootInfo.Number}");
    //    }
    //}

    /// <summary>
    /// Create a new notification in the notification holder 
    /// </summary>
    /// <param name="sprite">The icon to show in the notification</param>
    /// <param name="content">The content of the notification for the text</param>
    public void CreateNewNotif(Sprite sprite, string content)
    {
        NotificationCell cell = NotificationCell.Create(contentHolder, sprite, content, timeDisplay, this);
        cells.RemoveAll((item) => item == null);
        int index = cells.Count == MaxStackNotif ? -1 : 0;
        if (index == -1)
        {
            //remove the first one and move all the other to the first
            //add at last 
            cells.First().ForceSurpress();
            cells.RemoveAt(0);
            cells.Add(cell);
        }
        else
        {
            //add at the position
            cells.Add(cell);
        }
    }

    public IEnumerator OnCellDestroy(NotificationCell cell)
    {
        cell.transform.SetParent(transform);
        Vector3 pos = cell.transform.position;
        while (cell.transform.position.y != DisapearPos.transform.position.y)
        {
            Vector3 newPos = Vector3.Lerp(cell.transform.position, DisapearPos.position, .5f);
            cell.transform.position = newPos;
            yield return null;
        }
        DOTweenModuleUI.DOFade(cell.group, 0, .5f).OnComplete(cell.OnDestroyCell);
    }
}
