using CardModel;
using Command;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static Info.AgainstInfo;
using GameEnum;
using Thread;

namespace CardSpace
{
    public class Card1004 : Card
    {
        //��ʼ��һ�ſ���1004
        public override void Init()
        {
            base.Init();
            //�������ʱ������Ч��
            cardEffect[TriggerTime.When][TriggerType.Play] = new List<Func<TriggerInfo, Task>>()
            {
                async (triggerInfo) =>
                {
                    //ѡ��һ��λ��
                    await GameSystem.SelectSystem.SelectLocation(this);
                    //�������λ��
                    await GameSystem.TransSystem.DeployCard(TriggerInfo.Build(this,this));
                }
            };
            //��������ʱ������Ч��
            cardEffect[TriggerTime.When][TriggerType.Deploy] = new List<Func<TriggerInfo, Task>>()
            {
                async (triggerInfo) =>
                {
                    //ѡ��һ����λ
                    await GameSystem.SelectSystem.SelectUnite(this,cardSet[Orientation.My][RegionTypes.Battle].cardList,3);
                    if (SelectUnits.Count>0)
                    {
                            //await GameSystem.PointSystem.Gain(TriggerInfo.Build(this,SelectUnits,1));
                        await GameSystem.TransSystem.BanishCard(TriggerInfo.Build(this,SelectUnits));

                        //Debug.Log("δѡ���Ŀ�괥��Ч��");
                        //for (int i = 1; i < 10; i++)
                        //{
                        //    await GameSystem.PointSystem.Gain(TriggerInfo.Build(this,SelectUnits,1));
                        //   // await Task.Delay(2000);
                        //}
                    }
                },
                async (triggerInfo) =>
                {
                    //await GameSystem.SelectSystem.SelectUnite(this,cardSet[Orientation.My][RegionTypes.Battle].cardList,1);
                }
            };
        }
    }
}