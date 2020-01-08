using CardModel;
using Command;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CardSpace
{
    public class Card1004 : Card
    {
        //[TriggerType.PlayCard]
        //public Func<Task> Step1 => async () => { print("test"); await Task.Delay(1000); };

        //[TriggerType.PlayCard]
        //public Func<Task> Step2 => async () =>
        //{
        //    await StateCommand.WaitForSelectLocation(this);
        //    await Deploy();
        //    await Task.Delay(100);
        //};
        //[TriggerType.Deploy]
        //public Func<Task> Step3 => async () =>
        //{
        //    await StateCommand.WaitForSelectLocation(this);
        //    GameSystem.PointSystem.Hurt(this, 5);
        //    await Task.Delay(100);
        //};

        public new void Init()
        {
            base.Init();

            cardEffect_Play = new List<Func<Task>>()
            {
                async () =>
                {
                    print("test");
                    await Task.Delay(1000);
                },
                async () =>
                {
                    await GameSystem.TransSystem.DisCard(this);
                    await GameSystem.SelectSystem.SelectUnite(this);
                    await Deploy();
                    await Task.Delay(100);
                }
            };
        }
    }
}