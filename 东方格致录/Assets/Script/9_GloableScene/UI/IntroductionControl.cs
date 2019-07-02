using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Info.GlobalBattleInfo.PlayerFocusCard!=null)
        {
            Command.UiCommand.ChangeIntroduction(Info.GlobalBattleInfo.PlayerFocusCard);
        }
    }
}
