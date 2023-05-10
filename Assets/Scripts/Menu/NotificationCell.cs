using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class NotificationCell : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI Content;
    private NotificationSystem system;
    public CanvasGroup group;

    public void Show(Sprite sprite,string content, float time)
    {
        icon.sprite = sprite;
        Content.text = content;
        StartCoroutine(Disapear(time));
    }

    private IEnumerator Disapear(float time)
    {
        yield return new WaitForSeconds(time);
        StartCoroutine(system.OnCellDestroy(this));
    }

    public void ForceSurpress()
    {
        StopAllCoroutines();
        StartCoroutine(system.OnCellDestroy(this));
    }

    public void OnDestroyCell()
    {
        Destroy(gameObject);
    }

    private IEnumerator DisapearStuff()
    {
        yield return null;
    }

    public static NotificationCell Create(Transform parent,Sprite sprite,string content,float time,NotificationSystem system)
    {
        var cell = Addressables.InstantiateAsync(AddressablesPath.NotificationCellPath, parent).WaitForCompletion().GetComponent<NotificationCell>();
        cell.system = system;
        cell.Show(sprite, content, time);
        return cell;
    }
}
