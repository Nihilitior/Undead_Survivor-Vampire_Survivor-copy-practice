using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LevelUp : UI_Popup
{
    public enum Panels
    {
        GridPanel,

    }

    private int _maxUpgradeNum = 3;

    public override Define.PopupUIGroup PopupID
    {
        get { return Define.PopupUIGroup.UI_LevelUp; }
    }



    public override void Init()
    {
        base.Init();
        Managers.Sound.Play("LevelUp", Define.Sound.Effect);
        Bind<GameObject>(typeof(Panels));

        GameObject gridPanel = Get<GameObject>((int)Panels.GridPanel);

        foreach(Transform child in gridPanel.transform)
        {
            Managers.Resource.Destroy(child.gameObject);
        }
        PlayerStat player = Managers.Game.getPlayer().GetOrAddComponent<PlayerStat>();
        List<string[]> itemList = Managers.Event.SetRandomItem(player, 3);
        for(int i = 0; i<itemList.Count; i++) 
        {
            Debug.Log($"Item{i + 1}  : {itemList[i][1]}");
        }

        //here we choose stat or weapon random number.
        string title = "�г� �׽�Ʈ";
        string desc = "�г� ���� �׽�Ʈ";
        for(int i = 0; i< _maxUpgradeNum; i++)
        {
            GameObject upgradePanel = Managers.UI.MakeSubItem<UpgdPanel>(parent:gridPanel.transform).gameObject;
            UpgdPanel upgradeDesc = upgradePanel.GetOrAddComponent<UpgdPanel>();
            upgradeDesc.SetData(itemList[i]);
            title = itemList[i][1];
            desc = itemList[i][1];
            upgradeDesc.SetInfo(title, desc);
        }
    }
}
