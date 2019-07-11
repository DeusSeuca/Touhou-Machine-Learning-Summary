using CardSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseEnter()
    {
        Info.GlobalBattleInfo.PlayerFocusCard = GetComponent<Card>();
        Command.NetCommand.AsyncInfo(GameEnum.NetAcyncType.FocusCard);
    }
    private void OnMouseExit()
    {
        if (Info.GlobalBattleInfo.PlayerFocusCard == GetComponent<Card>())
        {
            Info.GlobalBattleInfo.PlayerFocusCard = null;
            Command.NetCommand.AsyncInfo(GameEnum.NetAcyncType.FocusCard);
        }
    }
}
