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
    }
}

