using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIResetPositionOnPlay : MonoBehaviour
{
    [SerializeField] bool isVisibleAtStart;
    Transform trans;
    RectTransform rect;
    CanvasGroup cgroup;

    void Awake()
    {
        gameObject.TryGetComponent(out trans);
        gameObject.TryGetComponent(out rect);
        gameObject.TryGetComponent(out cgroup);
        //if(rect != null)
        //    rect.sizeDelta = new Vector2(0, 0);
        transform.localPosition = Vector3.zero;
        SetVisible(isVisibleAtStart);
    }

    public void SetVisible(bool isShown) {
        if (cgroup) {
            if (isShown)
                cgroup.alpha = 1f;
            else
                cgroup.alpha = 0f;

            cgroup.blocksRaycasts = isShown;
            cgroup.interactable = isShown;
        }
        else {
            if (!isShown) gameObject.SetActive(false);
        }
    }
}
