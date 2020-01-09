using Control;
using Extension;
using GameAttribute;
using GameEnum;
using Info;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Thread;
using UnityEngine;
using UnityEngine.UI;

namespace CardModel
{
    public class Card : MonoBehaviour
    {
        public int CardId;
        public int point;
        public Texture2D icon;
        public float MoveSpeed = 0.1f;
        public Region property;
        public Territory territory;
        public Vector3 TargetPos;
        public Quaternion TargetRot;

        public bool IsCover = false;
        public bool IsLocked = false;

        public bool IsInit = false;
        public bool IsGray = false;
        /// <summary>
        /// 卡牌是否能自由操控
        /// </summary>
        public bool isFree = false;
        public bool isCanSee = false;
        public bool IsMoveStepOver = true;
        public bool IsPrePrepareToPlay = false;
        public bool IsAutoMove => this != AgainstInfo.PlayerPlayCard;

        public List<Card> Row => RowsInfo.GetRow(this);
        public Network.NetInfoModel.Location Location => RowsInfo.GetLocation(this);


        public Text PointText => transform.GetChild(0).GetChild(0).GetComponent<Text>();
        public string CardName => Command.CardInspector.CardLibraryCommand.GetCardStandardInfo(CardId).cardName;
        public string CardIntroduction => Command.CardInspector.CardLibraryCommand.GetCardStandardInfo(CardId).describe;

        [TriggerType.PlayCard]
        public List<Func<Task>> cardEffect_Play = new List<Func<Task>>();
        [TriggerType.Deploy]
        public List<Func<Task>> cardEffect_Deploy = new List<Func<Task>>();
        public List<Func<Task>> cardEffect_Dead = new List<Func<Task>>();
        public void Init()
        {
            IsInit = true;
            PointText.text = point.ToString();
        }
        public void SetMoveTarget(Vector3 TargetPosition, Vector3 TargetEulers)
        {
            TargetPos = TargetPosition;
            TargetRot = Quaternion.Euler(TargetEulers + new Vector3(0, 0, isCanSee ? 0 : 180));
            if (IsInit)
            {
                transform.position = TargetPos;
                transform.rotation = TargetRot;
                IsInit = false;
            }
        }
        public void SetCardSee(bool isCanSee)
        {
            this.isCanSee = isCanSee;
        }
        public void RefreshState()
        {
            Material material = GetComponent<Renderer>().material;
            if (AgainstInfo.PlayerFocusCard == this)
            {
                material.SetFloat("_IsFocus", 1);
                material.SetFloat("_IsRed", 0);
            }
            else if (AgainstInfo.OpponentFocusCard == this)
            {
                material.SetFloat("_IsFocus", 1);
                material.SetFloat("_IsRed", 1);
            }
            else
            {
                material.SetFloat("_IsFocus", 0);
            }
            material.SetFloat("_IsTemp", IsGray ? 0 : 1);
            transform.position = Vector3.Lerp(transform.position, TargetPos, MoveSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, TargetRot, Time.deltaTime * 10);
            PointText.text = point.ToString();
        }
        public async Task TriggerAsync<T>()
        {
           // CardEffectStackControl.Trigger<T>(this);
            await CardEffectStackControl.Trigger_NewAsync<T>(this);
        }
        public async Task Hurt(int point)
        {
            this.point = Math.Max(this.point - point, 0);
            MainThread.Run(() =>
            {
                Command.EffectCommand.ParticlePlay(1, transform.position);
            });
            Command.EffectCommand.AudioEffectPlay(1);
            await Task.Delay(100);
        }
        public async Task Boost(int point)
        {
            this.point += point;
            Command.EffectCommand.ParticlePlay(0, transform.position);
            Command.EffectCommand.AudioEffectPlay(1);
            await Task.Delay(1000);
        }
        //临时移动测试
        public async Task MoveTo(RegionTypes region, Orientation orientation, int rank = 0)
        {
            List<Card> OriginRow = RowsInfo.GetRow(this);
            List<Card> TargetRow = AgainstInfo.cardSet[orientation][region].cardList;
            OriginRow.Remove(this);
            TargetRow.Insert(rank, this);
            IsMoveStepOver = false;
            await Task.Delay(1000);
            IsMoveStepOver = true;
        }
        public async Task MoveTo(SingleRowInfo singleRowInfo, int Index = 0)
        {
            List<Card> OriginRow = RowsInfo.GetRow(this);
            List<Card> TargetRow = singleRowInfo.ThisRowCards;
            OriginRow.Remove(this);
            TargetRow.Insert(Index, this);
            MoveSpeed = 0.1f;
            IsMoveStepOver = false;
            await Task.Delay(1000);
            IsMoveStepOver = true;
            MoveSpeed = 0.1f;
            Command.EffectCommand.AudioEffectPlay(1);
        }
        //需要整理
        public async Task RemoveFromBattle(Orientation orientation, int Index = 0)
        {
            SingleRowInfo grave = AgainstInfo.cardSet[orientation][RegionTypes.Grave].singleRowInfos[0];
            IsMoveStepOver = false;
            List<Card> OriginRow = RowsInfo.GetRow(this);
            List<Card> TargetRow = grave.ThisRowCards;
            OriginRow.Remove(this);
            TargetRow.Insert(Index, this);
            this.SetCardSee(false);
            MoveSpeed = 0.1f;
            await Task.Delay(100);
            IsMoveStepOver = true;
            MoveSpeed = 0.1f;
            Command.EffectCommand.AudioEffectPlay(1);
        }
        public async Task Deploy()
        {
            await MoveTo(AgainstInfo.SelectRegion, AgainstInfo.SelectLocation);
        }
        [Button]
        public async Task MoveTest(RegionTypes Region, Orientation orientation, int Index)
        {
            await MoveTo(Region, orientation, Index);
        }

        public int this[CardField property]
        {
            get
            {
                var s = GetType().GetFields().Where(field =>
                {
                    Attribute attribute = field.GetCustomAttribute(typeof(CardProperty));
                    return attribute != null && ((CardProperty)attribute).cardProperty == property;
                }).ToList();
                return s.Any() ? (int)s[0].GetValue(this) : 0;
            }
            set
            {
                GetType().GetFields().First(field => ((CardProperty)field.GetCustomAttribute(typeof(CardProperty))).cardProperty == property).SetValue(this, value);
            }
        }

    }

}
