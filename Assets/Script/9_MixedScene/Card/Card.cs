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
        public int basePoint;
        public int changePoint;
        public int showPoint => basePoint + changePoint;
        public Texture2D icon;
        public float MoveSpeed = 0.1f;
        public Region property;
        public Territory territory;
        public string cardTag;
        public Territory belong => Info.AgainstInfo.cardSet[GameEnum.Orientation.Down].cardList.Contains(this) ? Territory.My : Territory.Op;
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
        public bool isMoveStepOver = true;
        public bool isPrepareToPlay = false;
        public bool IsAutoMove => this != AgainstInfo.PlayerPlayCard;

        public List<Card> Row => RowsInfo.GetRow(this);
        public Network.NetInfoModel.Location Location => RowsInfo.GetLocation(this);


        public Text PointText => transform.GetChild(0).GetChild(0).GetComponent<Text>();
        public string CardName => Command.CardInspector.CardLibraryCommand.GetCardStandardInfo(CardId).cardName;
        public string CardIntroduction => Command.CardInspector.CardLibraryCommand.GetCardStandardInfo(CardId).describe;


        public Dictionary<TriggerTime, Dictionary<TriggerType, List<Func<TriggerInfo, Task>>>> cardEffect = new Dictionary<TriggerTime, Dictionary<TriggerType, List<Func<TriggerInfo, Task>>>>();

        private void Update() => RefreshCardUi();

        public void RefreshCardUi()
        {
            PointText.text = (basePoint + changePoint).ToString();
            if (changePoint > 0)
            {
                PointText.color = Color.green;
            }
            else if (changePoint < 0)
            {
                PointText.color = Color.red;
            }
            else
            {
                PointText.color = Color.black;
            }
        }
        public virtual void Init()
        {
            IsInit = true;

            foreach (TriggerTime tirggerTime in Enum.GetValues(typeof(TriggerTime)))
            {
                cardEffect[tirggerTime] = new Dictionary<TriggerType, List<Func<TriggerInfo, Task>>>();
                foreach (TriggerType triggerType in Enum.GetValues(typeof(TriggerType)))
                {
                    cardEffect[tirggerTime][triggerType] = new List<Func<TriggerInfo, Task>>();
                }
            }
            cardEffect[TriggerTime.When][TriggerType.Gain] = new List<Func<TriggerInfo, Task>>()
            {
                async (triggerInfo) =>
                {
                    Debug.Log("执行增益操作");
                    //await Command.CardCommand.Gain(this,triggerInfo.point);
                    await Command.CardCommand.Gain(triggerInfo);
                }
            };
            cardEffect[TriggerTime.When][TriggerType.Hurt] = new List<Func<TriggerInfo, Task>>()
            {
                async (triggerInfo) =>
                {
                    Debug.Log("执行受伤操作");
                    await Command.CardCommand.Hurt(this,triggerInfo.point);
                }
            };
            cardEffect[TriggerTime.When][TriggerType.Cure] = new List<Func<TriggerInfo, Task>>()
            {
                async (triggerInfo) =>
                {
                    Debug.Log("执行治愈操作");
                    await Command.CardCommand.Cure(this);
                }
            };
            cardEffect[TriggerTime.Before][TriggerType.Banish] = new List<Func<TriggerInfo, Task>>()
            {
                async (triggerInfo) =>
                {
                    //await Task.Delay(1000);
                    //Debug.LogWarning("我被遗弃了"+name);
                }
            };
            cardEffect[TriggerTime.When][TriggerType.Banish] = new List<Func<TriggerInfo, Task>>()
            {
                async (triggerInfo) =>
                {
                  await Command.CardCommand.BanishCard(this);
                }
            };
            cardEffect[TriggerTime.After][TriggerType.RoundEnd] = new List<Func<TriggerInfo, Task>>()
            {
                async (triggerInfo) =>
                {
                    if (Info.AgainstInfo.cardSet[RegionTypes.Battle].cardList.Contains(this))
                    {
                        Debug.Log("移除啦");
                        await Command.CardCommand.RemoveFromBattle(this);
                    }
                }
            };
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
            PointText.text = basePoint.ToString();
        }
        public async Task Hurt(int point)
        {
            this.basePoint = Math.Max(this.basePoint - point, 0);
            MainThread.Run(() =>
            {
                //Command.EffectCommand.ParticlePlay(1, transform.position);
            });
            Command.EffectCommand.AudioEffectPlay(1);
            await Task.Delay(100);
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
            set => GetType().GetFields().First(field => ((CardProperty)field.GetCustomAttribute(typeof(CardProperty))).cardProperty == property).SetValue(this, value);
        }

    }
}
