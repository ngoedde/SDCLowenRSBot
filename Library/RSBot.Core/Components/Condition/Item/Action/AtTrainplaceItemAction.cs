using RSBot.Core.Objects;

namespace RSBot.Core.Components.Condition.Item.Action
{
    internal class AtTrainplaceItemAction : IItemConditionAction
    {
        public string EventName => "OnEnterGame";

        private int _itemTicksLeft = 0;

        private int _lastTick = 0;

        public void Invoke(ItemCondition condition, InventoryItem? item)
        {
            _lastTick = 0;

            if (item != null)
            {
                item.Use();

                if (condition.Repeat)
                {
                    _itemTicksLeft = item.Record.DecayTime;
                }
            }


            item?.Use();
        }
    }
}
