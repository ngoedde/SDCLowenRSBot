using RSBot.Core;
using RSBot.Core.Plugins;
using RSBot.General.Views;
using System.Windows.Forms;

namespace RSBot.General
{
    public class Bootstrap : IPlugin
    {
        /// <summary>
        /// Gets or sets the information of the plugin.
        /// </summary>
        /// <value>
        /// The information.
        /// </value>
        public PluginInfo Information => new()
        {
            DisplayAsTab = true,
            DisplayName = "General",
            InternalName = "RSBot.General",
            LoadIndex = 1,
            TabDisplayIndex = 0,
            RequireIngame = false
        };

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            Views.View.Instance = new();
            Views.View.PendingWindow = new();
            Views.View.AccountsWindow = new();
        }

        /// <summary>
        /// Gets the view that will be displayed as tab page.
        /// </summary>
        public Control GetView()
        {
            return Views.View.Instance;
        }

        /// <summary>
        /// Translate the plugin
        /// </summary>
        /// <param name="language">The language</param>
        public void Translate()
        {
            LanguageManager.Translate(GetView(), Kernel.Language);
            LanguageManager.Translate(Views.View.PendingWindow, Kernel.Language);
            LanguageManager.Translate(Views.View.AccountsWindow, Kernel.Language);
        }
    }
}