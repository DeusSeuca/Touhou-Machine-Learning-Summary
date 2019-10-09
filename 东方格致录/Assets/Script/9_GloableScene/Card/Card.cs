using Control;
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

namespace CardSpace
{
    public class Card : MonoBehaviour
    {
        public int CardId;
        public int CardPoint;
        public Texture2D icon;
        public bool IsMove;
        public bool IsMoveStepOver = true;
        public bool IsAvailable = true;
        public bool IsLocked = true;
        public bool IsCover = true;
        public float MoveSpeed = 0.1f;

        public bool IsGray;
        public bool IsCanSee;
        public bool IsPrePrepareToPlay;
        bool IsInit;
        public Region property;
        //生效范围
        public Territory territory;
        
        /// <summary>
        /// 限制卡牌被打出
        /// </summary>
        public bool IsLimit = true;
        public bool IsAutoMove => this != AgainstInfo.PlayerPlayCard;
        public List<Card> Row => RowsInfo.GetRow(this);
        //public SingleRowInfo Region => RowsInfo.GetRow(this);
        public Network.NetInfoModel.Location Location => RowsInfo.GetLocation(this);
        public Vector3 TargetPos;
        public Quaternion TargetRot;

        public Text PointText => transform.GetChild(0).GetChild(0).GetComponent<Text>();
        public string CardName => Command.CardLibraryCommand.GetCardStandardInfo(CardId).cardName;
        public string CardIntroduction => Command.CardLibraryCommand.GetCardStandardInfo(CardId).describe;
        public void Init()
        {
            IsInit = true;
            PointText.text = CardPoint.ToString();
        }
        public void SetMoveTarget(Vector3 TargetPosition, Vector3 TargetEulers)
        {
            TargetPos = TargetPosition;
            TargetRot = Quaternion.Euler(TargetEulers + new Vector3(0, 0, IsCanSee ? 0 : 180));
            if (IsInit)
            {
                transform.position = TargetPos;
                transform.rotation = TargetRot;
                IsInit = false;
            }
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
            if (IsGray)
            {
                material.SetFloat("_IsTemp", 0);
            }
            else
            {
                material.SetFloat("_IsTemp", 1);
            }
            transform.position = Vector3.Lerp(transform.position, TargetPos, MoveSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, TargetRot, Time.deltaTime * 10);
            PointText.text = CardPoint.ToString();
        }
        public void Trigger<T>()
        {
            List<Func<Task>> Steps = new List<Func<Task>>();
            List<PropertyInfo> tasks = GetType().GetProperties().Where(x =>
                x.GetCustomAttributes(true).Any() && x.GetCustomAttributes(true)[0].GetType() == typeof(T)).ToList();
            tasks.Reverse();
            tasks.Select(x => x.GetValue(this)).Cast<Func<Task>>().ToList().ForEach(CardEffectStackControl.TaskStack.Push);
            _ = CardEffectStackControl.Run();
        }
        public async Task Hurt(int point)
        {
            CardPoint = Math.Max(CardPoint - point, 0);
            MainThread.Run(() =>
            {
                Command.EffectCommand.ParticlePlay(1, transform.position);
            });
            Command.EffectCommand.AudioEffectPlay(1);
            await Task.Delay(100);
        }
        public async Task Boost(int point)
        {
            CardPoint += point;
            Command.EffectCommand.ParticlePlay(0, transform.position);
            Command.EffectCommand.AudioEffectPlay(1);
            await Task.Delay(1000);
        }
        public async Task MoveTo(RegionTypes Region, bool IsOnPlayerPart = true, int Index = 0)
        {
            List<Card> OriginRow = RowsInfo.GetRow(this);
            List<Card> TargetRow = IsOnPlayerPart ? Info.RowsInfo.GetMyCardList(Region) : Info.RowsInfo.GetOpCardList(Region);
            OriginRow.Remove(this);
            TargetRow.Insert(Index, this);
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
        public async Task Deploy()
        {
            Debug.Log("开始部署");
            await MoveTo(AgainstInfo.SelectRegion, AgainstInfo.SelectLocation);

        }
        [Button]
        public async Task MoveTest(RegionTypes Region, bool IsOnPlayerPart, int Index)
        {
            await MoveTo(Region, IsOnPlayerPart, Index);
        }

        public int this[CardField property]
        {
            get
            {
                var s = GetType().GetFields().Where(field =>
                {
                  Attribute attribute=  field.GetCustomAttribute(typeof(CardProperty));
                  return  attribute != null && ((CardProperty)attribute).cardProperty == property;
                }).ToList();
                return s .Count()==0? 0: (int)s[0].GetValue(this);
            }
            set
            {
                GetType().GetFields().First(field => ((CardProperty)field.GetCustomAttribute(typeof(CardProperty))).cardProperty == property).SetValue(this, value);
            }
        }

    }

}
