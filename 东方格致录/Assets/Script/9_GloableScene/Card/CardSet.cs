using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardModel;
using CardSpace;
using GameEnum;
using Info;
using UnityEngine;

public class CardSet
{
    //主体信息
    public static List<List<Card>> globalCardList = new List<List<Card>>();
    public List<SingleRowInfo> singleRowInfos = new List<SingleRowInfo>();
    //该
    private List<Card> rowCardList => singleRowInfos.SelectMany(x => x.ThisRowCards).ToList();
    public List<Card> cardList;

    public void Init()
    {
        if (cardList == null)
        {
            cardList = globalCardList.SelectMany(x => x).ToList();
        }
    }
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
            Init();
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
            cardList = cardList.Intersect(rowCardList).ToList();
            return new CardSet(targetRows, cardList);
        }
    }
    public CardSet this[Orientation orientation]
    {
        get
        {
            Init();
            List<SingleRowInfo> targetRows = new List<SingleRowInfo>();
            switch (orientation)
            {
                case Orientation.Up:
                    targetRows = singleRowInfos.Where(row => row.orientation == orientation).ToList(); break;
                case Orientation.Down:
                    targetRows = singleRowInfos.Where(row => row.orientation == orientation).ToList(); ; break;
                case Orientation.My:
                    return this[AgainstInfo.IsMyTurn ? Orientation.Down : Orientation.Up];
                case Orientation.Op:
                    return this[AgainstInfo.IsMyTurn ? Orientation.Up : Orientation.Down];
                case Orientation.All:
                    targetRows = singleRowInfos; break;
            }
            cardList = cardList.Intersect(rowCardList).ToList();
            return new CardSet(targetRows, cardList);
        }
    }
    public CardSet this[CardState cardState]
    {
        get
        {
            return new CardSet(singleRowInfos, cardList);
        }
    }
    public CardSet this[CardField cardField]
    {
        get
        {
            return new CardSet(singleRowInfos, cardList);
        }
    }
    public CardSet this[params Tag[] tags]
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
        singleRowInfos.ForEach(x => x.ThisRowCards = x.ThisRowCards.OrderBy(x => x.point).ToList());
    }
}
