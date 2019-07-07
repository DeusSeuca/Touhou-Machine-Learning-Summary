using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using static NetInfoModel;

namespace Info
{
    /// <summary>
    /// 双方信息
    /// </summary>
    public class AllPlayerInfo : SerializedMonoBehaviour
    {
        //玩家的用户信息
        [ShowInInspector]
        public static PlayerInfo UserInfo;
        //对手的用户信息
        [ShowInInspector]
        public static PlayerInfo OpponentInfo;
        //开房者信息
        //[ShowInInspector]
        //public static PlayerInfo Player1Info=>Info.GlobalBattleInfo.IsPlayer1? UserInfo: OpponentInfo;
        ////加入者信息
        //[ShowInInspector]
        //public static PlayerInfo Player2Info => !Info.GlobalBattleInfo.IsPlayer1 ? UserInfo : OpponentInfo;
    }
}

