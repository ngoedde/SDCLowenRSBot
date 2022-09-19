using RSBot.Core;
using RSBot.Core.Client.ReferenceObjects;
using RSBot.Core.Event;
using RSBot.Core.Extensions;
using RSBot.Core.Network;
using RSBot.Core.Objects;
using SDUI;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using RSBot.Core.Components;
using RSBot.Core.Components.Condition.Item;

namespace RSBot.Inventory.Views
{
    [System.ComponentModel.ToolboxItem(false)]
    public partial class Main : UserControl
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        private object _lock;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        private int _selectedIndex = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Main"/> class.
        /// </summary>
        public Main()
        {
            _lock = new object();
            InitializeComponent();
            SubscribeEvents();

            listViewMain.SmallImageList = Core.Extensions.ListViewExtensions.StaticItemsImageList;

            var backColor = ColorScheme.BorderColor.Determine().Alpha(85);
            buttonInventory.ForeColor = backColor.Determine();
            buttonInventory.Color = backColor;
        }

        /// <summary>
        /// Subscribes the events.
        /// </summary>
        private void SubscribeEvents()
        {
            EventManager.SubscribeEvent("OnLoadCharacter", UpdateInventoryList);

            EventManager.SubscribeEvent("OnUpdateInventoryItem", new Action<byte>(OnUpdateInventoryItem));
            EventManager.SubscribeEvent("OnUseItem", new Action<byte>(OnUpdateInventoryItem));
            EventManager.SubscribeEvent("OnInventoryUpdate", UpdateInventoryList);
        }

        /// <summary>
        /// Calling when update inventory item
        /// </summary>
        /// <param name="slot"></param>
        private void OnUpdateInventoryItem(byte slot)
        {
            var key = slot.ToString();
            if (!listViewMain.Items.ContainsKey(key))
                return;

            var inventoryItem = Game.Player.Inventory.GetItemAt(slot);
            if (inventoryItem == null)
                return;

            var listViewItem = listViewMain.Items[key];

            var name = inventoryItem.Record.GetRealName();
            if (inventoryItem.OptLevel > 0)
                name += " (+" + inventoryItem.OptLevel + ")";

            listViewItem.SubItems[0].Text = name;
            listViewItem.SubItems[1].Text = inventoryItem.Amount.ToString();

            if (inventoryItem.Record.IsEquip)
                listViewItem.SubItems[2].Text = inventoryItem.Record.GetRarityName();

            listViewItem.LoadItemImageAsync(inventoryItem.Record);
        }

        /// <summary>
        /// Updates the inventory list.
        /// </summary>
        public void UpdateInventoryList()
        {
            if (Parent == null)
                return;

            if (!Parent.Visible)
                return;

            if (Game.Player == null)
                return;

            listViewMain.BeginUpdate();
            listViewMain.Items.Clear();

            switch (_selectedIndex)
            {
                case 0:

                    var itemsPlayer = Game.Player.Inventory.GetNormalPartItems();
                    foreach (var item in itemsPlayer)
                        AddItem(item);

                    lblFreeSlots.Text = Game.Player.Inventory.Count + "/" + Game.Player.Inventory.Capacity;

                    break;

                case 1:

                    foreach (var item in Game.Player.Inventory.GetEquippedPartItems())
                        AddItem(item);

                    lblFreeSlots.Text = "0";

                    break;

                case 2:

                    foreach (var item in Game.Player.Avatars)
                        AddItem(item);

                    lblFreeSlots.Text = "0";

                    break;

                case 3:

                    if (!Game.Player.HasActiveAbilityPet)
                    {
                        listViewMain.EndUpdate();
                        return;
                    }

                    foreach (var item in Game.Player.AbilityPet.Inventory)
                        AddItem(item);

                    lblFreeSlots.Text = Game.Player.AbilityPet.Inventory.Count + "/" + Game.Player.AbilityPet.Inventory.Capacity;

                    break;

                case 4:

                    if (Game.Player.Storage == null)
                    {
                        listViewMain.EndUpdate();
                        return;
                    }

                    foreach (var item in Game.Player.Storage)
                        AddItem(item);

                    lblFreeSlots.Text = Game.Player.Storage.Count + "/" + Game.Player.Storage.Capacity;

                    break;

                case 5:

                    if (Game.Player.GuildStorage == null)
                    {
                        listViewMain.EndUpdate();
                        return;
                    }

                    foreach (var item in Game.Player.GuildStorage)
                        AddItem(item);

                    lblFreeSlots.Text = Game.Player.GuildStorage.Count + "/" + Game.Player.GuildStorage.Capacity;

                    break;

                case 6:

                    if (Game.Player.JobTransport == null)
                    {
                        listViewMain.EndUpdate();
                        return;
                    }

                    foreach (var item in Game.Player.JobTransport.Inventory)
                        AddItem(item);

                    lblFreeSlots.Text = Game.Player.JobTransport.Inventory.Count + "/" + Game.Player.JobTransport.Inventory.Capacity;

                    break;

                case 7:

                    if (Game.Player.Job2SpecialtyBag == null)
                    {
                        listViewMain.EndUpdate();
                        return;
                    }

                    foreach (var item in Game.Player.Job2SpecialtyBag)
                        AddItem(item);

                    lblFreeSlots.Text = Game.Player.Job2SpecialtyBag.Count + "/" + Game.Player.Job2SpecialtyBag.Capacity;

                    break;

                case 8:

                    if (Game.Player.Job2 == null)
                    {
                        listViewMain.EndUpdate();
                        return;
                    }

                    foreach (var item in Game.Player.Job2)
                        AddItem(item);

                    lblFreeSlots.Text = Game.Player.Job2.Count + "/" + Game.Player.Job2.Capacity;

                    break;

                case 9:

                    if (!Game.Player.HasActiveFellowPet)
                    {
                        listViewMain.EndUpdate();
                        return;
                    }

                    foreach (var item in Game.Player.Fellow.Inventory)
                        AddItem(item);

                    lblFreeSlots.Text = Game.Player.Fellow.Inventory.Count + "/" + Game.Player.Fellow.Inventory.Capacity;

                    break;
            }

            listViewMain.EndUpdate();
        }

        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <param name="item">The item.</param>
        private void AddItem(InventoryItem item)
        {
            var name = item.Record.GetRealName();
            if (item.OptLevel > 0)
                name += " (+" + item.OptLevel + ")";

            var lvItem = listViewMain.Items.Add(item.Slot.ToString(), name, 0);
            lvItem.Tag = item;
            lvItem.SubItems.Add(item.Amount.ToString());

            if (item.Record.IsEquip)
                lvItem.SubItems.Add(item.Record.GetRarityName());

            lvItem.LoadItemImageAsync(item.Record);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Main_Load(object sender, System.EventArgs e)
        {
            var tab = Parent as TabPage;
            tab.VisibleChanged += Parent_VisibleChanged;
        }

        /// <summary>
        /// Handles the visible changed event of the parent.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Parent_VisibleChanged(object sender, System.EventArgs e)
        {
            if (!Visible)
                listViewMain.Items.Clear();
            else
                UpdateInventoryList();
        }

        /// <summary>
        /// Handles the Click event of the btnReload control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void buttonUseItem_Click(object sender, System.EventArgs e)
        {
            if (listViewMain.SelectedIndices.Count != 1)
                return;

            var inventoryItem = GetSelectedItem();
            inventoryItem?.Use();
        }

        /// <summary>
        /// Handles the mouse double click event of the listviewmain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void listViewMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listViewMain.SelectedItems.Count <= 0)
                return;

#if !DEBUG
            if(GlobalConfig.Get<bool>("RSBot.DebugEnvironment") == false)
                return;
#endif
            var inventoryItem = GetSelectedItem();
            if (inventoryItem == null)
                return;

            var itemForm = new ItemProperties(inventoryItem);
            itemForm.Show();
        }


        /// <summary>
        /// Handles the selected index changed event of the button's control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ButtonSwitcher(object sender, EventArgs e)
        {
            var button = sender as SDUI.Controls.Button;
            if (_selectedIndex == button.TabIndex)
                return;

            _selectedIndex = button.TabIndex;

            foreach (var control in topPanel.Controls.OfType<SDUI.Controls.Button>())
            {
                if (control.TabIndex > 9)
                    continue;

                control.Color = Color.Transparent;

                if (control == button)
                {
                    var backColor = ColorScheme.BorderColor.Determine().Alpha(85);
                    control.ForeColor = backColor.Determine();
                    control.Color = backColor;
                }

                control.Invalidate();
            }

            UpdateInventoryList();
        }

        private void dropToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewMain.SelectedIndices.Count != 1)
                return;

            var inventoryItem = GetSelectedItem();
            if (inventoryItem == null)
                return;

            var cos = _selectedIndex == 3;
            inventoryItem?.Drop(cos, Game.Player.AbilityPet.UniqueId);
        }

        private void LoadItemCondition(string itemCodeName)
        {
            var conditions = ConditionManager.GetItemConditions(itemCodeName);

            afterTrainingPlaceToolStripMenuItem.Checked = false;
            beforeTrainingPlaceToolStripMenuItem.Checked = false;
            afterTeleportationToolStripMenuItem.Checked = false;
            repeatAfterFinishToolStripMenuItem.Checked = false;

            foreach (var condition in conditions)
            {
                if (condition.EventName == "OnFinishWalkScript")
                    afterTrainingPlaceToolStripMenuItem.Checked = true;
                else if (condition.EventName == "OnFinishTownScript")
                    beforeTrainingPlaceToolStripMenuItem.Checked = true;
                else if (condition.EventName == "OnLoadCharacter")
                    afterTeleportationToolStripMenuItem.Checked = true;

                repeatAfterFinishToolStripMenuItem.Checked = condition.Repeat;
            }
        }

        private void contextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (listViewMain.SelectedIndices.Count != 1)
            {
                e.Cancel = true;
                return;
            }

            var inventoryItem = GetSelectedItem();
            if (inventoryItem == null)
                return;

            LoadItemCondition(inventoryItem.Record.CodeName);

            if (_selectedIndex != 0)
            {
                useToolStripMenuItem.Visible = false;
                moveToLastDeathPositionToolStripMenuItem.Visible = false;
                moveToLastRecallPositionToolStripMenuItem.Visible = false;
                moveToPetToolStripMenuItem.Visible = false;
                moveToPlayerToolStripMenuItem.Visible = _selectedIndex == 3;
                selectMapLocationToolStripMenuItem.Visible = false;
                return;
            }

            bool isReverseScroll = inventoryItem.Equals(new TypeIdFilter(3, 3, 3, 3));
            useToolStripMenuItem.Visible = !isReverseScroll;
            useToolStripMenuItem.Enabled = inventoryItem.Record.CanUse != ObjectUseType.No;
            moveToLastDeathPositionToolStripMenuItem.Visible = isReverseScroll;
            moveToLastRecallPositionToolStripMenuItem.Visible = isReverseScroll;
            selectMapLocationToolStripMenuItem.Visible = isReverseScroll;
            dropToolStripMenuItem.Visible = inventoryItem.Record.CanDrop != ObjectDropType.No;

            moveToPetToolStripMenuItem.Visible = Game.Player.AbilityPet != null && _selectedIndex != 3;
            moveToPlayerToolStripMenuItem.Visible = _selectedIndex == 3;
            
            if (isReverseScroll)
            {
                var tagItem = selectMapLocationToolStripMenuItem.Tag as InventoryItem;
                if(tagItem != inventoryItem)
                {
                    selectMapLocationToolStripMenuItem.Tag = inventoryItem;
                    selectMapLocationToolStripMenuItem.DropDownItems.Clear();

                    foreach (var item in Game.ReferenceManager.OptionalTeleports)
                    {
                        var mapName = Game.ReferenceManager.GetTranslation(item.Value.RegionID.ToString());

                        var menuItem = new ToolStripMenuItem { Text = mapName };

                        menuItem.Click += (itemSender, itemEvent) =>
                        {
                            inventoryItem.UseTo(7, item.Value.ID);
                        };

                        selectMapLocationToolStripMenuItem.DropDownItems.Add(menuItem);
                    }
                }
            }
        }

        private void moveToLastRecallPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewMain.SelectedIndices.Count != 1)
                return;

            var inventoryItem = GetSelectedItem();
            
            inventoryItem?.UseTo(2);
        }

        private void moveToLastDeathPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewMain.SelectedIndices.Count != 1)
                return;

            var inventoryItem = GetSelectedItem();

            inventoryItem?.UseTo(3);
        }

        private void moveToPetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewMain.SelectedIndices.Count != 1)
                return;
            
            var inventoryItem = GetSelectedItem();
            if (inventoryItem == null)
                return;

            if (Game.Player.AbilityPet != null)
                return;

            var freeSlot = Game.Player.AbilityPet.Inventory.GetFreeSlot();
            if (freeSlot == 0xFF)
                return;

            var packet = new Packet(0x7034);
            packet.WriteByte(InventoryOperation.SP_MOVE_ITEM_PC_PET);
            packet.WriteUInt(Game.Player.AbilityPet.UniqueId);
            packet.WriteByte(inventoryItem.Slot);
            packet.WriteByte(freeSlot);
            PacketManager.SendPacket(packet, PacketDestination.Server);
        }

        private void moveToPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewMain.SelectedIndices.Count != 1)
                return;
            
            var inventoryItem = GetSelectedItem();
            if (inventoryItem == null)
                return;

            if (Game.Player.AbilityPet == null)
                return;

            var freeSlot = Game.Player.Inventory.GetFreeSlot();
            if (freeSlot == 0xFF)
                return;

            var packet = new Packet(0x7034);
            packet.WriteByte(InventoryOperation.SP_MOVE_ITEM_PET_PC);
            packet.WriteUInt(Game.Player.AbilityPet.UniqueId);
            packet.WriteByte(inventoryItem.Slot);
            packet.WriteByte(freeSlot);
            PacketManager.SendPacket(packet, PacketDestination.Server);
        }

        private InventoryItem? GetSelectedItem()
        {
            var listViewItem = listViewMain.SelectedItems[0];
            return listViewItem.Tag is not InventoryItem inventoryItem ? null : inventoryItem;
        }

        private void afterTrainingPlaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var inventoryItem = GetSelectedItem();
            if (inventoryItem == null)
                return;

            if (afterTrainingPlaceToolStripMenuItem.Checked)
                ConditionManager.RemoveItemCondition(inventoryItem.Record.CodeName, "OnFinishWalkScript");
            else
            {
                var condition = new ItemCondition
                {
                    EventName = "OnFinishWalkScript",
                    ItemCodeName = inventoryItem.Record.CodeName,
                    Repeat = repeatAfterFinishToolStripMenuItem.Checked,
                };

                ConditionManager.AddItemCondition(condition);
            }

            afterTrainingPlaceToolStripMenuItem.Checked = !afterTrainingPlaceToolStripMenuItem.Checked;

            ConditionManager.SaveConditions();
        }

        private void beforeTrainingPlaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var inventoryItem = GetSelectedItem();
            if (inventoryItem == null)
                return;

            if (beforeTrainingPlaceToolStripMenuItem.Checked)
                ConditionManager.RemoveItemCondition(inventoryItem.Record.CodeName, "OnFinishTownScript");
            else
            {
                var condition = new ItemCondition
                {
                    EventName = "OnFinishTownScript",
                    ItemCodeName = inventoryItem.Record.CodeName,
                    Repeat = repeatAfterFinishToolStripMenuItem.Checked,
                };

                ConditionManager.AddItemCondition(condition);
            }

            beforeTrainingPlaceToolStripMenuItem.Checked = !beforeTrainingPlaceToolStripMenuItem.Checked;

            ConditionManager.SaveConditions();
        }

        private void repeatAfterFinishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var inventoryItem = GetSelectedItem();
            if (inventoryItem == null)
                return;

            var conditions = ConditionManager.GetItemConditions(inventoryItem.Record.CodeName);
            if (conditions?.Count() == 0)
            {
                repeatAfterFinishToolStripMenuItem.Checked = false;

                return;
            }

            foreach (var condition in conditions)
            {
                condition.Repeat = !repeatAfterFinishToolStripMenuItem.Checked;
            }

            repeatAfterFinishToolStripMenuItem.Checked = !repeatAfterFinishToolStripMenuItem.Checked;

            ConditionManager.SaveConditions();
        }

        private void afterTeleportationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var inventoryItem = GetSelectedItem();
            if (inventoryItem == null)
                return;

            if (afterTeleportationToolStripMenuItem.Checked)
                ConditionManager.RemoveItemCondition(inventoryItem.Record.CodeName, "OnLoadCharacter");
            else
            {
                var condition = new ItemCondition
                {
                    EventName = "OnLoadCharacter",
                    ItemCodeName = inventoryItem.Record.CodeName,
                    Repeat = repeatAfterFinishToolStripMenuItem.Checked,
                };

                ConditionManager.AddItemCondition(condition);
            }

            afterTeleportationToolStripMenuItem.Checked = !afterTeleportationToolStripMenuItem.Checked;

            ConditionManager.SaveConditions();
        }
    }
}