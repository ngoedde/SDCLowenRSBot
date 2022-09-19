using System.Timers;
using RSBot.Core.Objects;

namespace RSBot.Core.Components.Condition.Item.Action
{
    internal class RecurringItemConditionTimer
    {
        public int Interval { get; init; } = 300 * 1000;
        
        public InventoryItem Item { get; private set; }

        private Timer _timer;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecurringItemConditionTimer"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="interval">The interval in seconds.</param>
        public RecurringItemConditionTimer(InventoryItem item, int interval = 300 * 1000)
        {
            Interval = interval;
            Item = item;

            _timer = new Timer(Interval)
            {
                AutoReset = true
            };

            _timer.Elapsed += _timer_Elapsed;
        }

        /// <summary>
        /// Starts the timer instance.
        /// </summary>
        public void Start()
        {
            _timer.Start();
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            _timer.Stop();
        }

        /// <summary>
        /// Will be triggered when the timer elapses
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="ElapsedEventArgs" /> instance containing the event data.
        /// </param>
        private void _timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            if (Item == null)
                return;

            //The item might have changed the slot since last time, so let's try to fetch the item again
            var actualItem = Game.Player.Inventory.GetItem(Item.Record.CodeName);

            if (actualItem == null)
                return;

            Item = actualItem;
            Item.Use();
        }

    }
}
