using System.Timers;
using RSBot.Core.Objects;


namespace RSBot.Core.Components.Condition.Item.Action
{
    public abstract class BasicItemAction : IItemConditionAction
    {
        private const int CooldownDuration = 300; //in seconds (5 minutes)
        public virtual string EventName { get; }

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
            
            var _timer = new Timer(CooldownDuration * 1000);
            _timer.Elapsed += _timer_Elapsed;
            
            //Use for the first time
            item.Use();
        }

        private void _timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            //if (_item == null)
            //    return;

            ////The item might have changed the slot since last time, so let's try to fetch the item again
            //var actualItem = Game.Player.Inventory.GetItem(_item.Record.CodeName);

            //if (actualItem == null)
            //    return;

            //_item = actualItem;
            //_item.Use();
        }
    }
}