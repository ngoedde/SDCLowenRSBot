using RSBot.Core.Objects;

namespace RSBot.Core.Components.Condition.Item
{
    public interface IItemConditionAction
    {
        void Invoke(ItemCondition condition, InventoryItem? item);

        string EventName { get; }
    }
}
