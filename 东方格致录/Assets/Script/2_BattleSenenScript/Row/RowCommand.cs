using CardSpace;
using GameEnum;
using Info;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Command
{
    public class RowCommand
    {
        public static async Task CreatTempCard(SingleRowInfo SingleInfo)
        {
            SingleInfo.TempCard = await CardCommand.CreatCard(RowsInfo.GetMyCardList(RegionTypes.Uesd)[0].CardId);
            SingleInfo.TempCard.IsGray = true;
            SingleInfo.TempCard.IsCanSee = true;
            SingleInfo.ThisRowCards.Insert(SingleInfo.Location, SingleInfo.TempCard);
            SingleInfo.TempCard.Init();
        }
        public static void DestoryTempCard(SingleRowInfo SingleInfo)
        {
            SingleInfo.ThisRowCards.Remove(SingleInfo.TempCard);
            GameObject.Destroy(SingleInfo.TempCard.gameObject);
            SingleInfo.TempCard = null;
        }
        public static void ChangeTempCard(SingleRowInfo SingleInfo)
        {
            SingleInfo.ThisRowCards.Remove(SingleInfo.TempCard);
            SingleInfo.ThisRowCards.Insert(SingleInfo.Location, SingleInfo.TempCard);
        }
        public static void RefreshHandCard(List<Card> ThisCardList, bool IsMyHandRegion)
        {
            if (IsMyHandRegion)
            {
                foreach (var item in ThisCardList)
                {
                    if (AgainstInfo.PlayerFocusCard != null && item == AgainstInfo.PlayerFocusCard && item.IsLimit == false)
                    {
                        item.IsPrePrepareToPlay = true;
                    }
                    else
                    {
                        item.IsPrePrepareToPlay = false;
                    }
                }
            }
        }
        [System.Obsolete("以后跟allcardlist统一api")]
        public static void SetPlayCardLimit(bool IsLimit)
        {
            //Info.AgainstInfo.AllRows.At( Orientation.Down).InRogin(RegionTypes.Leader, RegionTypes.Hand).ForEach(card => card.IsLimit = IsLimit);
            //Info.AgainstInfo.AllCardList.
            Info.RowsInfo.GetDownCardList(RegionTypes.Hand).ForEach(card => card.IsLimit = IsLimit);
            Info.RowsInfo.GetDownCardList(RegionTypes.Leader).ForEach(card => card.IsLimit = IsLimit);
        }
        /// <summary>
        /// 根据使用的卡片显示可部署区域
        /// </summary>
        /// <param name="CanBeSelected"></param>
        [System.Obsolete("需要优化下")]
        
        public static void SetAllRegionSelectable(bool CanBeSelected)
        {
            //singleRow.CanBeSelected = CanBeSelected;
            if (CanBeSelected)
            {
                List<SingleRowInfo> TargetSingleRow = new List<SingleRowInfo>();
                Card DeployCard = RowsInfo.GetMyCardList(RegionTypes.Uesd)[0];
                bool IsMyTerritory = (DeployCard.CardTerritory == Territory.My);
                switch (DeployCard.property)
                {
                    case Property.Water:
                        {
                            TargetSingleRow.Add(Info.RowsInfo.GetSingleRowInfo(RegionTypes.Water, IsMyTerritory));
                            break;
                        }
                    case Property.Fire:
                        {
                            TargetSingleRow.Add(Info.RowsInfo.GetSingleRowInfo(RegionTypes.Fire, IsMyTerritory));
                            break;
                        }
                    case Property.Wind:
                        {
                            TargetSingleRow.Add(Info.RowsInfo.GetSingleRowInfo(RegionTypes.Wind, IsMyTerritory));
                            break;
                        }
                    case Property.Soil:
                        {
                            TargetSingleRow.Add(Info.RowsInfo.GetSingleRowInfo(RegionTypes.Soil, IsMyTerritory));
                            break;
                        }
                    case Property.All:
                        {
                            TargetSingleRow.Add(Info.RowsInfo.GetSingleRowInfo(RegionTypes.Water, IsMyTerritory));
                            TargetSingleRow.Add(Info.RowsInfo.GetSingleRowInfo(RegionTypes.Fire, IsMyTerritory));
                            TargetSingleRow.Add(Info.RowsInfo.GetSingleRowInfo(RegionTypes.Wind, IsMyTerritory));
                            TargetSingleRow.Add(Info.RowsInfo.GetSingleRowInfo(RegionTypes.Soil, IsMyTerritory));
                            break;
                        }
                    case Property.None:
                        break;

                    default:
                        break;
                }
                TargetSingleRow.ForEach(SingleRow => SingleRow.SetRegionSelectable(true));
            }
            else
            {
                Debug.Log("全部清空");
                RowsInfo.Instance.singleBattleInfos.Values.ToList().ForEach(row => row.SetRegionSelectable(false));
            }
        }
        /// <summary>
        /// <para>返回符合条件的卡片集合</para>
        /// <para>卡片所属:my/op</para>
        /// <para>卡片位置:lead/hand/battle/grave/use </para>
        /// <para>卡片区域：wind/fire/wind/soil</para>
        /// <para>卡片标签：fairy/spell...</para>
        /// <para>"卡片点数：>x/=x..</para>
        /// <para>卡片最大最小：min3/max1</para>
        /// </summary>
        //[System.Obsolete("废弃啦")]
        //public static List<Card> GetCardList(string FilterText)
        //{
        //    string[] Features = FilterText.Split('&');
        //    List<Card> TargetCardList = new List<Card>();
        //    RowsInfo.GlobalCardList.ForEach(TargetCardList.AddRange);
        //    Debug.Log("总卡牌数量为" + TargetCardList.Count);

        //    Features.ForEach(Feature => Filter(TargetCardList, Feature));
        //    Debug.Log("检索到卡牌数量为" + TargetCardList.Count);
        //    return TargetCardList;
        //}
        //[System.Obsolete("废弃啦")]
        //private static List<Card> Filter(List<Card> cards, string Feature)
        //{
        //    if (Feature == "my")
        //    {
        //        Debug.Log("检索条件：友方");
        //        return cards.Where(card => card.CardTerritory == Territory.My).ToList();
        //    }
        //    else if (Feature == "op")
        //    {
        //        Debug.Log("检索条件：敌方");
        //        return cards.Where(card => card.CardTerritory == Territory.Op).ToList();
        //    }
        //    else if (Feature.Contains("max"))
        //    {
        //        int num = int.Parse(Feature.Replace("max", ""));
        //        Debug.Log($"检索条件：最大{num}个");
        //        return cards.OrderBy(card => card.CardPoint).Take(num).ToList();
        //    }
        //    else if (Feature.Contains("min"))
        //    {
        //        int num = int.Parse(Feature.Replace("min", ""));
        //        Debug.Log($"检索条件：最小{num}个");
        //        return cards.OrderByDescending(card => card.CardPoint).Take(num).ToList();
        //    }
        //    else if (Feature.Contains("<"))
        //    {
        //        int Point = int.Parse(Feature.Replace("<", ""));
        //        Debug.Log($"检索条件：小于{Point}的单位");
        //        return cards.Where(card => card.CardPoint < Point).ToList();
        //    }
        //    else if (Feature.Contains(">"))
        //    {
        //        int Point = int.Parse(Feature.Replace(">", ""));
        //        Debug.Log($"检索条件：大于{Point}的单位");
        //        return cards.Where(card => card.CardPoint > Point).ToList();
        //    }
        //    else if (Feature.Contains("="))
        //    {
        //        int Point = int.Parse(Feature.Replace("=", ""));
        //        Debug.Log($"检索条件：等于{Point}的单位");
        //        return cards.Where(card => card.CardPoint == Point).ToList();
        //    }
        //    else
        //    {
        //        Debug.Log($"检索条件：包含标签{Feature}的单位");
        //        return cards.Where(card => card.tag.Contains(Feature)).ToList();
        //    }
        //}

    }
}


