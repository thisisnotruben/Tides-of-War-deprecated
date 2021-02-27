using Godot;
using Game.Actor;
using Game.Database;
using Game.Quest;
namespace Game.Ui
{
	public class MerchantController : GameMenu
	{
		private readonly InventoryModel store = new InventoryModel();
		private Control mainContent;
		private Label header;
		private Button buySellButton;
		private PopupController popup;
		private ItemInfoMerchantController storeItemInfo;
		private SlotGridController storeSlots;
		private InventoryModel _playerSpellBook;
		public InventoryModel playerSpellBook
		{
			get { return _playerSpellBook; }
			set
			{
				_playerSpellBook = value;
				storeItemInfo.playerSpellBook = value;
			}
		}
		public InventoryModel playerInventory;
		private Npc _merchant;
		public Npc merchant
		{
			get { return _merchant; }
			set
			{
				_merchant = value;
				// resets store
				store.Clear();
				storeSlots.ClearSlots();
				buySellButton.Text = "Sell";
				storeItemInfo.Visible = popup.Visible = buySellButton.Pressed = false;
				mainContent.Show();
				if (value != null)
				{
					DisplayItems(Globals.contentDB.GetData(value.Name).merchandise);
				}
			}
		}

		public override void _Ready()
		{
			mainContent = GetChild<Control>(0);
			header = mainContent.GetChild<Label>(0);
			storeSlots = mainContent.GetNode<SlotGridController>("c/SlotGrid");
			buySellButton = mainContent.GetChild<Button>(2);
			buySellButton.Connect("toggled", this, nameof(OnBuySellToggled));
			foreach (SlotController slot in storeSlots.GetSlots())
			{
				slot.button.Connect("pressed", this, nameof(OnStoreSlotSelected),
					new Godot.Collections.Array() { slot.GetIndex() });
			}

			storeItemInfo = GetChild<ItemInfoMerchantController>(1);
			storeItemInfo.backBttn.Connect("pressed", this, nameof(OnItemStoreInfoBackPressed));
			storeItemInfo.Connect(nameof(ItemInfoMerchantController.OnTransaction), this, nameof(OnTransaction));
			storeItemInfo.itemList = store;

			popup = GetChild<PopupController>(2);
			popup.Connect("hide", this, nameof(OnHide));
			popup.okayBttn.Connect("pressed", this, nameof(OnHide));
			popup.repairBackBttn.Connect("pressed", this, nameof(OnHide));
		}
		private void OnTransaction(string commodityName, int goldAmount, bool bought)
		{
			// called from ItemInfo when player buys/sells
			bool isSpell = Globals.spellDB.HasData(commodityName);
			if (bought)
			{
				(isSpell ? playerSpellBook : playerInventory).AddCommodity(commodityName);
			}
			else
			{
				(isSpell ? playerSpellBook : playerInventory).RemoveCommodity(commodityName);
				CheckHudSlots(isSpell ? playerSpellBook : playerInventory, commodityName);
			}

			QuestMaster.CheckQuests(commodityName,
				isSpell
					? QuestDB.QuestType.LEARN
					: QuestDB.QuestType.COLLECT,
				bought
			);

			// add/sub gold
			player.gold += goldAmount;
			header.Text = "Gold: " + player.gold;
		}
		private void DisplayItems(params string[] commodityNames)
		{
			storeItemInfo.isBuying = !buySellButton.Pressed;

			// add commodities to merchant model/view
			int i;
			for (i = 0; i < commodityNames.Length; i++)
			{
				store.AddCommodity(commodityNames[i]);
			}
			for (i = 0; i < store.count; i++)
			{
				// cannot be in same loop due to inplace-sorting && stacking
				storeSlots.DisplaySlot(i, store.GetCommodity(i), store.GetCommodityStack(i));
			}
		}
		private void OnDraw() { header.Text = "Gold: " + (player?.gold ?? 0); }
		private void OnHide() { popup.Hide(); }
		private void OnItemStoreInfoBackPressed() { mainContent.Show(); }
		private void OnStoreSlotSelected(int slotIndex)
		{
			// don't want to click on an empty slot
			if (slotIndex >= store.count)
			{
				return;
			}

			// called from the slots in the merchant view
			string CommodityName = store.GetCommodity(slotIndex);
			bool isSpell = Globals.spellDB.HasData(CommodityName),
				alreadyHave = false;

			if (isSpell)
			{
				PlaySound(NameDB.UI.CLICK1);
				PlaySound(NameDB.UI.SPELL_SELECT);
				alreadyHave = playerSpellBook.HasItem(CommodityName);
			}
			else
			{
				Globals.soundPlayer.PlaySound(Globals.itemDB.GetData(CommodityName).material, true);
			}

			// show item details and switch view
			mainContent.Hide();
			storeItemInfo.selectedSlotIdx = slotIndex;
			storeItemInfo.Display(CommodityName, true, !buySellButton.Pressed, alreadyHave);
		}
		private void OnBuySellToggled(bool buttonPressed)
		{
			PlaySound(NameDB.UI.CLICK1);
			buySellButton.Text = buttonPressed ? "Buy" : "Sell";
			storeSlots.ClearSlots();
			store.Clear();
			if (player != null && merchant != null)
			{
				DisplayItems(
					buttonPressed
						? playerInventory.GetCommodities().ToArray()
						: Globals.contentDB.GetData(merchant.Name).merchandise
				);
			}
		}
	}
}