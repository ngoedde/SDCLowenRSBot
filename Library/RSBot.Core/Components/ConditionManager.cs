using System;
using System.Collections.Generic;
using System.Linq;
using RSBot.Core.Components.Condition.Item;
using RSBot.Core.Event;

namespace RSBot.Core.Components
{
    public static class ConditionManager
    {
        public static List<IItemConditionAction> ItemActions { get; private set; }

        public static List<ItemCondition> ItemConditions { get; set; }

        public static void Initialize()
        {
            ItemConditions = new List<ItemCondition>();
         
            RegisterItemConditionActions();
        }

        private static void RegisterItemConditionActions()
        {
            ItemActions = new List<IItemConditionAction>();

            var type = typeof(IItemConditionAction);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface).ToArray();

            foreach (var handler in types)
            {
                var instance = (IItemConditionAction)Activator.CreateInstance(handler);

                ItemActions.Add(instance);
            }
        }

        private static void EventProxy(IItemConditionAction action, ItemCondition condition)
        {
            var inventoryItem = Game.Player.Inventory.GetItem(condition.ItemCodeName);

            action.Invoke(condition, inventoryItem);
        }

        public static void RegisterItemCondition(ItemCondition condition)
        {
            ItemConditions.Add(condition);

            EventManager.SubscribeEvent("OnInvokeEvent", OnInvokeEvent);
        }

        private static void OnInvokeEvent(string eventName)
        {
            var actionTrigger = ItemActions.FirstOrDefault(a => a.EventName == eventName);

            if (actionTrigger == null)
                return;

            var conditions = ItemConditions.Where(c => c.EventName == eventName);

            foreach (var condition in conditions)
                actionTrigger.Invoke(condition, Game.Player.Inventory.GetItem(condition.ItemCodeName));
        }
    }
}
