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

        public static List<ItemCondition> ItemConditions { get; private set; }

        public static void Initialize()
        {
            RegisterItemConditionActions();

            EventManager.SubscribeEvent("OnInvokeEvent", OnInvokeEvent);
            EventManager.SubscribeEvent("OnLoadPlayerConfig", LoadConditions);
        }
        
        public static void LoadConditions()
        {
            try
            {
                ItemConditions = PlayerConfig.GetObject("RSBot.ItemConditions", new List<ItemCondition>());
            }
            catch (Exception ex)
            {
                ItemConditions = new List<ItemCondition>();

                Log.Warn($"Item conditions loading failed: {ex.Message}");
            }
        }

        public static void SaveConditions()
        {
            PlayerConfig.SetObject("RSBot.ItemConditions", ItemConditions);
        }
        
        public static IEnumerable<ItemCondition> GetItemConditions(string itemCodeName)
        {
            return ItemConditions?.Where(x => x != null && x.ItemCodeName == itemCodeName);
        }

        public static void RemoveItemCondition(string itemCodeName, string eventName)
        {
            var elementToRemove =
                ItemConditions.FirstOrDefault(i => i.EventName == eventName && i.ItemCodeName == itemCodeName);

            if (elementToRemove != null)
                ItemConditions.Remove(elementToRemove);
        }

        public static void AddItemCondition(ItemCondition condition)
        {
            var existing = ItemConditions.FirstOrDefault(c =>
                c.EventName == condition.EventName && c.ItemCodeName == condition.ItemCodeName);

            if (existing == null)
                ItemConditions.Add(condition);
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

        private static void RegisterItemConditionActions()
        {
            ItemActions = new List<IItemConditionAction>();

            var type = typeof(IItemConditionAction);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsAbstract && !p.IsInterface).ToArray();

            foreach (var handler in types)
            {
                var instance = (IItemConditionAction)Activator.CreateInstance(handler);

                ItemActions.Add(instance);
            }
        }
    }
}
