using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuView : BaseView
{
    public void PlayGame()
    {
        GameBroker.Ins.PlayGame();
        Hide();
    }
}
