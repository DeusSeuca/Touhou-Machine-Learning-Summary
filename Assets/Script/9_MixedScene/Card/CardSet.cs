using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardModel;
using CardSpace;
using GameEnum;
using Info;
using Sirenix.OdinInspector;
using UnityEngine;

public class CardSet
{
    //主体信息
    [ShowInInspector]
    public static List<List<Card>> globalCardList = new List<List<Card>>();
    public List<SingleRowInfo> singleRowInfos = new List<SingleRowInfo>();
    [ShowInInspector]
    public List<Card> cardList = null;
    public int count => cardList.Count;
    /// <summary>
    /// 得到触发牌之外的卡牌列表，用于广播触发事件的前后相关事件
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    public List<Card> BroastCardList(Card card) => Info.AgainstInfo.cardSet[GameEnum.Orientation.All].cardList.Except(new List<Card> { card }).ToList();

    public CardSet()
    {
        globalCardList.Clear();
        Enumerable.Range(0, 18).ToList().ForEach(x => globalCardList.Add(new List<Card>()));
    }
    public CardSet(List<SingleRowInfo> singleRowInfos, List<Card> cardList = null)
    {
        this.singleRowInfos = singleRowInfos;
        this.cardList = cardList;
    }
    public List<Card> this[int rank]
    {
        get
        {
            return globalCardList[rank];
        }
        set
        {
            globalCardList[rank] = value;
        }
    }
    public CardSet this[params RegionTypes[] regions]
    {
        get
        {
            List<SingleRowInfo> targetRows = new List<SingleRowInfo>();
            if (regions.Contains(RegionTypes.Battle))
            {
                targetRows = singleRowInfos.Where(row =>
                 row.region == RegionTypes.Water ||
                 row.region == RegionTypes.Fire ||
                 row.region == RegionTypes.Wind ||
                 row.region == RegionTypes.Soil
                ).ToList();
            }
            else
            {
                targetRows = singleRowInfos.Where(row => regions.Contains(row.region)).ToList();
            }
            List<Card> filterCardList = cardList ?? globalCardList.SelectMany(x => x).ToList();
            filterCardList = filterCardList.Intersect(targetRows.SelectMany(x => x.ThisRowCards)).ToList();
            return new CardSet(targetRows, filterCardList);
        }
    }
    public CardSet this[Orientation orientation]
    {
        get
        {
            List<SingleRowInfo> targetRows = new List<SingleRowInfo>();
            switch (orientation)
            {
                case Orientation.Up:
                    targetRows = singleRowInfos.Where(row => row.orientation == orientation).ToList(); break;
                case Orientation.Down:
                    targetRows = singleRowInfos.Where(row => row.orientation == orientation).ToList(); break;
                case Orientation.My:
                    return this[AgainstInfo.isMyTurn ? Orientation.Down : Orientation.Up];
                case Orientation.Op:
                    return this[AgainstInfo.isMyTurn ? Orientation.Up : Orientation.Down];
                case Orientation.All:
                    targetRows = singleRowInfos; break;
            }
            List<Card> filterCardList = cardList ?? globalCardList.SelectMany(x => x).ToList();
            filterCardList = filterCardList.Intersect(targetRows.SelectMany(x => x.ThisRowCards)).ToList();
            return new CardSet(targetRows, filterCardList);
        }
    }
    //待补充
    public CardSet this[CardState cardState]
    {
        get
        {
            return new CardSet(singleRowInfos, cardList);
        }
    }
    //待补充
    public CardSet this[CardField cardField]
    {
        get
        {
            return new CardSet(singleRowInfos, cardList);
        }
    }
    /// <summary>
    /// 根据Tag检索卡牌
    /// </summary>
    /// <param name="tags"></param>
    /// <returns></returns>
    public CardSet this[params CardTag[] tags]
    {
        get
        {
            cardList = cardList ?? globalCardList.SelectMany(x => x).ToList();
            List<Card> filterCardList = cardList.Where(card => 
                tags.Any(tag => 
                    card.cardTag.Contains(tag.TransTag())))
                .ToList();
            return new CardSet(singleRowInfos, filterCardList);
        }
    }
    //待补充
    public CardSet this[CardFeature cardFeature]
    {
        get
        {
            List<Card> filterCardList= cardList;
            switch (cardFeature)
            {
                case CardFeature.Largest:
                    int largestPoint = cardList.Max(card => card.showPoint);
                    filterCardList = cardList.Where(card => card.showPoint == largestPoint).ToList();
                    break;
                case CardFeature.Lowest:
                    int lowestPoint = cardList.Min(card => card.showPoint);
                    filterCardList = cardList.Where(card => card.showPoint == lowestPoint).ToList();
                    break;
                default:
                    break;
            }
            return new CardSet(singleRowInfos, filterCardList);
        }
    }
    //待补充
    public CardSet this[params CardRank[] ranks]
    {
        get
        {
            return new CardSet(singleRowInfos, cardList);
        }
    }
    public void Add(Card card, int rank = -1)
    {
        if (singleRowInfos.Count != 1)
        {
            Debug.LogWarning("选择区域异常，数量为" + singleRowInfos.Count);
        }
        if (rank == -1)
        {
            rank = singleRowInfos[0].ThisRowCards.Count;
        }
        singleRowInfos[0].ThisRowCards.Insert(rank, card);
    }
    public void Remove(Card card)
    {
        if (singleRowInfos.Count != 1)
        {
            Debug.LogWarning("选择区域异常，数量为" + singleRowInfos.Count);
        }
        singleRowInfos[0].ThisRowCards.Remove(card);
    }
    public void Order()
    {
        singleRowInfos.ForEach(x => x.ThisRowCards = x.ThisRowCards.OrderBy(card => card.basePoint).ToList());
    }
}
