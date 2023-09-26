using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_GameVictory : UI_Popup
{
    enum Images
    {
        VictoryLogo
    }

    enum Buttons
    {
        BackToMainButton
    }

    public override Define.PopupUIGroup PopupID { get { return Define.PopupUIGroup.UI_GameVictory; } }

    public override void Init()
    {
        base.Init();
        Managers.Sound.Play("Win");
        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));


        GetButton((int)Buttons.BackToMainButton).gameObject.AddUIEvent(OnClickBackToMain);
    }


    void OnClickBackToMain(PointerEventData data)
    {
        Managers.Sound.Play("Select", Define.Sound.Effect);
        Managers.Scene.LoadScene(Define.SceneType.MainMenuScene);
        Managers.GamePlay();
    }
}
