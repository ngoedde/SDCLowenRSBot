namespace RSBot.Inventory.Components.ItemConditions
{
    internal class ItemCondition
    {
        public string ItemCodeName { get; set; }

        public ItemConditionType Type { get; set; }

        public bool Repeat { get; set; }
    }

    internal enum ItemConditionType
    {
        AtTrainplace,
        ToTrainplace,
    }
}
