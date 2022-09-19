using System.Collections.Generic;
using RSBot.Core.Objects;


namespace RSBot.Core.Components.Condition.Item.Action
{
    public abstract class BasicItemAction : IItemConditionAction
    {
        private const int CooldownDuration = 300 * 1000; //in milliseconds (5 minutes)
        public virtual string EventName { get; }

        private Dictionary<string, RecurringItemConditionTimer> _timers = new();

        public void Invoke(ItemCondition condition, InventoryItem? item)
        {
            if (item == null)
                return;

            //Don't repeat after event was triggered
            if (!condition.Repeat)
            {
                item.Use();

                return;
            }

            var timerKey = condition.EventName + ":" + condition.ItemCodeName;

            if (!_timers.ContainsKey(timerKey))
            {
                var timer = new RecurringItemConditionTimer(item, CooldownDuration);
                _timers.Add(item.Record.CodeName, timer);

                timer.Start();
            }

            //Use for the first time, independent from the timer
            item.Use();
        }
    }
}