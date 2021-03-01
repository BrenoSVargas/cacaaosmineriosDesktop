using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelPosition : MonoBehaviour
{
    GameObject panel;

    public void PanelPositioner()
    {
        panel = this.gameObject;
        panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100, 0);

    }
}


