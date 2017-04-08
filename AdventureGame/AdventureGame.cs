
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.IO;
using Engine;

namespace AdventureGame
{
    public partial class AdventureGame : Form
    {
        int[] levelarr = { 20,50,80,130,180,250,330,410};
        private Player Player1;
        private Monster currentMonster;

        public AdventureGame()
        {
         
            InitializeComponent();

            Player1 = new Player(10, 10, 20, 0, 1);
            MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
            rtbMessages.TextChanged += rtbMessages_TextChanged;
            
            


          

            lblExperience.Text = Player1.ExperiencePoints.ToString();
            lblGold.Text = Player1.Gold.ToString();
            lblHitPoints.Text = Player1.CurrentHitPoints.ToString();
            lblLevel.Text = Player1.Level.ToString();
        }


            private void btnNorth_Click(object sender, EventArgs e)
        {
            MoveTo(Player1.CurrentLocation.LocationToNorth);
        }


        private void BlockNav()
        {
            btnNorth.Visible = false;
            btnSouth.Visible = false;
            btnEast.Visible = false;
            btnWest.Visible = false;
        }

        private void unBlockNav()
        {
            btnNorth.Visible = true;
            btnSouth.Visible = true;
            btnEast.Visible = true;
            btnWest.Visible = true;
        }
        private void btnEast_Click(object sender, EventArgs e)
        {
            MoveTo(Player1.CurrentLocation.LocationToEast);
        }

        private void btnSouth_Click(object sender, EventArgs e)
        {
            MoveTo(Player1.CurrentLocation.LocationToSouth);
        }

        private void btnWest_Click(object sender, EventArgs e)
        {
            MoveTo(Player1.CurrentLocation.LocationToWest);
        }

        private void MoveTo(Location newLocation)
            
        {
            

            List<Image> images = new List<Image>();
            Bitmap image1 = global::AdventureGame.Properties.Resources.moyross;
            
            //Bitmap image1 = (Bitmap)Image.FromFile(@"Engine\resources\Home.jpg");
            images.Add(image1);
            Bitmap image2 = global::AdventureGame.Properties.Resources.moyrosssquare;
            //Bitmap image2 = (Bitmap)Image.FromFile(@"C:\Users\Mark\Desktop\GameImgs\TownSquareStatue.jpg");
            images.Add(image2);
            Bitmap image3 = global::AdventureGame.Properties.Resources.AlchemistHut;
            images.Add(image3);
            Bitmap image4 = global::AdventureGame.Properties.Resources.AlchemistGarden;
            images.Add(image4);
            Bitmap image5 = global::AdventureGame.Properties.Resources.FarmHouse;
            images.Add(image5);
            Bitmap image6 = global::AdventureGame.Properties.Resources.Vegetables;
            images.Add(image6);
            Bitmap image7 = global::AdventureGame.Properties.Resources.oldShotgun;
            images.Add(image7);
            Bitmap image8 = global::AdventureGame.Properties.Resources.estate2;

            images.Add(image8);


            List<Image> weapons = new List<Image>();
            Bitmap weapon0 = global::AdventureGame.Properties.Resources.oldShotgun;
            weapons.Add(weapon0);
            //Does the location have any required items
            if (!Player1.HasRequiredItemToEnterThisLocation(newLocation))
            {
                rtbMessages.Text += "You must have a " + newLocation.ItemRequiredToEnter.Name + " to enter this location." + Environment.NewLine;
                return;
            }

            //if (Player1.CurrentLocation.ID == 2)
            //{
            //    NoMain.Visible = true;
            //    YesMain.Visible = true;

            //}
            //else
            //{

            //    NoMain.Visible = false;
            //    YesMain.Visible = false;
            //};

            // Update the player's current location
            Player1.CurrentLocation = newLocation;

            // Show/hide available movement buttons
            btnNorth.Visible = (newLocation.LocationToNorth != null);
            btnEast.Visible = (newLocation.LocationToEast != null);
            btnSouth.Visible = (newLocation.LocationToSouth != null);
            btnWest.Visible = (newLocation.LocationToWest != null);

            // Display current location name and description
            rtbLocation.Text = newLocation.Name + Environment.NewLine;
            rtbLocation.Text += newLocation.Description + Environment.NewLine;
            ImageBox.Image = images[newLocation.LocId];

            // Completely heal the player
            Player1.CurrentHitPoints = Player1.MaximumHitPoints;

            // Update Hit Points in UI
            lblHitPoints.Text = Player1.CurrentHitPoints.ToString();


            if(newLocation.LocId == 1)
            {
                  NoMain.Visible = true;
                  YesMain.Visible = true;

                  BlockNav();


            }
            if(newLocation.LocId == 6)
            {
                DialogResult dialogResult = MessageBox.Show("Sure", "Do you want to take a chance and rob the shotgun", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Player1.Inventory.Add(new InventoryItem(World.ItemByID(World.ITEM_ID_SAWD_OFF), 1));
                wpnImage.Image = weapon0;
                wpnImage.Visible = true;
                }
                else if (dialogResult == DialogResult.No)
                {
                    //do something else
                }
              

                btnNorth.Visible = true;
                btnEast.Visible = false;
                btnSouth.Visible = true;
                btnWest.Visible = false;

                NoMain.Visible = false ;
                YesMain.Visible = false;
            }


            // Does the location have a quest?
            if (newLocation.QuestAvailableHere != null)
            {
                // See if the player already has the quest, and if they've completed it
                bool playerAlreadyHasQuest = Player1.HasThisQuest(newLocation.QuestAvailableHere);
                bool playerAlreadyCompletedQuest = Player1.CompletedThisQuest(newLocation.QuestAvailableHere);

                // See if the player already has the quest
                if (playerAlreadyHasQuest)
                {
                    // If the player has not completed the quest yet
                    if (!playerAlreadyCompletedQuest)
                    {
                        // See if the player has all the items needed to complete the quest
                        bool playerHasAllItemsToCompleteQuest = Player1.HasAllQuestCompletionItems(newLocation.QuestAvailableHere);

                        // The player has all items required to complete the quest
                        if (playerHasAllItemsToCompleteQuest)
                        {
                            // Display message
                            rtbMessages.Text += Environment.NewLine;
                            rtbMessages.Text += "You complete the '" + newLocation.QuestAvailableHere.Name + "' quest." + Environment.NewLine;

                            // Remove quest items from inventory
                            Player1.RemoveQuestCompletionItems(newLocation.QuestAvailableHere);

                            // Give quest rewards
                            rtbMessages.Text += "You receive: " + Environment.NewLine;
                            rtbMessages.Text += newLocation.QuestAvailableHere.RewardExperiencePoints.ToString() + " experience points" + Environment.NewLine;
                            rtbMessages.Text += newLocation.QuestAvailableHere.RewardGold.ToString() + " gold" + Environment.NewLine;
                            rtbMessages.Text += newLocation.QuestAvailableHere.RewardItem.Name + Environment.NewLine;
                            rtbMessages.Text += Environment.NewLine;

                            Player1.ExperiencePoints += newLocation.QuestAvailableHere.RewardExperiencePoints;
                            Player1.Gold += newLocation.QuestAvailableHere.RewardGold;

                            // Add the reward item to the player's inventory
                            Player1.AddItemToInventory(newLocation.QuestAvailableHere.RewardItem);

                            // Mark the quest as completed
                            Player1.MarkQuestCompleted(newLocation.QuestAvailableHere);
                        }
                    }
                }
                else
                {
                    // The player does not already have the quest

                    // Display the messages
                    rtbMessages.Text += "You receive the " + newLocation.QuestAvailableHere.Name + " quest." + Environment.NewLine;
                    rtbMessages.Text += newLocation.QuestAvailableHere.Description + Environment.NewLine;
                    rtbMessages.Text += "To complete it, return with:" + Environment.NewLine;
                    foreach (QuestCompletionItem qci in newLocation.QuestAvailableHere.QuestCompletionItems)
                    {
                        if (qci.Quantity == 1)
                        {
                            rtbMessages.Text += qci.Quantity.ToString() + " " + qci.Details.Name + Environment.NewLine;
                        }
                        else
                        {
                            rtbMessages.Text += qci.Quantity.ToString() + " " + qci.Details.NamePlural + Environment.NewLine;
                        }
                    }
                    rtbMessages.Text += Environment.NewLine;

                    // Add the quest to the player's quest list
                    Player1.Quests.Add(new PlayerQuest(newLocation.QuestAvailableHere));
                }
            }

            // Does the location have a monster?
            if (newLocation.MonsterLivingHere != null)
            {
                rtbMessages.Text += "You see a " + newLocation.MonsterLivingHere.Name + Environment.NewLine;

                // Make a new monster, using the values from the standard monster in the World.Monster list
                Monster standardMonster = World.MonsterByID(newLocation.MonsterLivingHere.ID);

                currentMonster = new Monster(standardMonster.ID, standardMonster.Name, standardMonster.MaximumDamage,
                    standardMonster.RewardExperiencePoints, standardMonster.RewardGold, standardMonster.CurrentHitPoints, standardMonster.MaximumHitPoints);

                foreach (LootItem lootItem in standardMonster.LootTable)
                {
                   currentMonster.LootTable.Add(lootItem);
                }

                cboWeapons.Visible = true;
                cboPotions.Visible = true;
                wpnImage.Visible = true;
                btnUseWeapon.Visible = true;
                btnUsePotion.Visible = true;
            }
            else
            {
                currentMonster = null;

                cboWeapons.Visible = false;
                cboPotions.Visible = false;
                btnUseWeapon.Visible = false;
                btnUsePotion.Visible = false;
            }

            // Refresh player's inventory list
            UpdateInventoryListInUI();

            // Refresh player's quest list
            UpdateQuestListInUI();

            // Refresh player's weapons combobox
            UpdateWeaponListInUI();

            // Refresh player's potions combobox
            UpdatePotionListInUI();

        }

        private void UpdateInventoryListInUI()
        {
            dgvInventory.RowHeadersVisible = false;

            dgvInventory.ColumnCount = 2;
            dgvInventory.Columns[0].Name = "Name";
            dgvInventory.Columns[0].Width = 197;
            dgvInventory.Columns[1].Name = "Quantity";

            dgvInventory.Rows.Clear();

            foreach (InventoryItem inventoryItem in Player1.Inventory)
            {
                if (inventoryItem.Quantity > 0)
                {
                    dgvInventory.Rows.Add(new[] { inventoryItem.Details.Name, inventoryItem.Quantity.ToString() });
                }
            }
        }

        private void UpdateQuestListInUI()
        {
            dgvQuests.RowHeadersVisible = false;

            dgvQuests.ColumnCount = 2;
            dgvQuests.Columns[0].Name = "Name";
            dgvQuests.Columns[0].Width = 197;
            dgvQuests.Columns[1].Name = "Done?";

            dgvQuests.Rows.Clear();

            foreach (PlayerQuest playerQuest in Player1.Quests)
            {
                dgvQuests.Rows.Add(new[] { playerQuest.Details.Name, playerQuest.IsCompleted.ToString() });
            }
        }

        private void UpdateWeaponListInUI()
        {
            List<Weapon> weapons = new List<Weapon>();

            foreach (InventoryItem inventoryItem in Player1.Inventory)
            {
                if (inventoryItem.Details is Weapon)
                {
                    if (inventoryItem.Quantity > 0)
                    {
                        weapons.Add((Weapon)inventoryItem.Details);
                    }
                }
            }

            if (weapons.Count == 0)
            {
                // The player doesn't have any weapons, so hide the weapon combobox and "Use" button
                cboWeapons.Visible = false;
                btnUseWeapon.Visible = false;
            }
            else
            {
                cboWeapons.DataSource = weapons;
                cboWeapons.DisplayMember = "Name";
                cboWeapons.ValueMember = "ID";

                cboWeapons.SelectedIndex = 0;
            }
        }

        public void LevelUp()
        {
         

            if(Player1.ExperiencePoints > levelarr[Player1.Level-1])
            {
                Player1.Level +=1;
                Player1.MaximumHitPoints += 5;
                Player1.CurrentHitPoints = Player1.MaximumHitPoints;
                lblLevel.Text = Player1.Level.ToString();
                lblHitPoints.Text = Player1.CurrentHitPoints.ToString();
            }
        }

        private void UpdatePotionListInUI()
        {
            List<HealingPotion> healingPotions = new List<HealingPotion>();

            foreach (InventoryItem inventoryItem in Player1.Inventory)
            {
                if (inventoryItem.Details is HealingPotion)
                {
                    if (inventoryItem.Quantity > 0)
                    {
                        healingPotions.Add((HealingPotion)inventoryItem.Details);
                    }
                }
            }

            if (healingPotions.Count == 0)
            {
                // The player doesn't have any potions, so hide the potion combobox and "Use" button
                cboPotions.Visible = false;
                btnUsePotion.Visible = false;
            }
            else
            {
                cboPotions.DataSource = healingPotions;
                cboPotions.DisplayMember = "Name";
                cboPotions.ValueMember = "ID";

                cboPotions.SelectedIndex = 0;
            }
        }

        private void btnUseWeapon_Click(object sender, EventArgs e)
        {
            // Get the currently selected weapon from the cboWeapons ComboBox
            Weapon currentWeapon = (Weapon)cboWeapons.SelectedItem;

            // Determine the amount of damage to do to the monster
            int damageToMonster = RandomNumberGenerator.NumberBetween(currentWeapon.MinimumDamage, currentWeapon.MaximumDamage);

            // Apply the damage to the monster's CurrentHitPoints
            currentMonster.CurrentHitPoints -= damageToMonster;

            // Display message
            rtbMessages.Text += "You hit the " + currentMonster.Name + " for " + damageToMonster.ToString() + " points." + Environment.NewLine;

            // Check if the monster is dead
            if (currentMonster.CurrentHitPoints <= 0)
            {
                // Monster is dead
                rtbMessages.Text += Environment.NewLine;
                rtbMessages.Text += "You defeated the " + currentMonster.Name + Environment.NewLine;

                // Give player experience points for killing the monster
                Player1.ExperiencePoints += currentMonster.RewardExperiencePoints;
                rtbMessages.Text += "You receive " + currentMonster.RewardExperiencePoints.ToString() + " experience points" + Environment.NewLine;

                // Give player gold for killing the monster 
                Player1.Gold += currentMonster.RewardGold;
                rtbMessages.Text += "You receive " + currentMonster.RewardGold.ToString() + " gold" + Environment.NewLine;

                // Get random loot items from the monster
                List<InventoryItem> lootedItems = new List<InventoryItem>();

                // Add items to the lootedItems list, comparing a random number to the drop percentage
                foreach (LootItem lootItem in currentMonster.LootTable)
                {
                    if (RandomNumberGenerator.NumberBetween(1, 100) <= lootItem.DropPercentage)
                    {
                        lootedItems.Add(new InventoryItem(lootItem.Details, 1));
                    }
                }

                // If no items were randomly selected, then add the default loot item(s).
                if (lootedItems.Count == 0)
                {
                    foreach (LootItem lootItem in currentMonster.LootTable)
                    {
                        if (lootItem.IsDefaultItem)
                        {
                            lootedItems.Add(new InventoryItem(lootItem.Details, 1));
                        }
                    }
                }

                // Add the looted items to the player's inventory
                foreach (InventoryItem inventoryItem in lootedItems)
                {
                    Player1.AddItemToInventory(inventoryItem.Details);

                    if (inventoryItem.Quantity == 1)
                    {
                        rtbMessages.Text += "You loot " + inventoryItem.Quantity.ToString() + " " + inventoryItem.Details.Name + Environment.NewLine;
                    }
                    else
                    {
                        rtbMessages.Text += "You loot " + inventoryItem.Quantity.ToString() + " " + inventoryItem.Details.NamePlural + Environment.NewLine;
                    }
                }

                // Refresh player information and inventory controls
                lblHitPoints.Text = Player1.CurrentHitPoints.ToString();
                lblGold.Text = Player1.Gold.ToString();
                lblExperience.Text = Player1.ExperiencePoints.ToString();
                lblLevel.Text = Player1.Level.ToString();

                UpdateInventoryListInUI();
                UpdateWeaponListInUI();
                UpdatePotionListInUI();

                // Add a blank line to the messages box, just for appearance.
                rtbMessages.Text += Environment.NewLine;

                // Move player to current location (to heal player and create a new monster to fight)
                MoveTo(Player1.CurrentLocation);
            }
            else
            {
                // Monster is still alive

                // Determine the amount of damage the monster does to the player
                int damageToPlayer = RandomNumberGenerator.NumberBetween(0, currentMonster.MaximumDamage);

                // Display message
                rtbMessages.Text += "The " + currentMonster.Name + " did " + damageToPlayer.ToString() + " points of damage." + Environment.NewLine;
                
                // Subtract damage from player
                Player1.CurrentHitPoints -= damageToPlayer;

                // Refresh player data in UI
                lblHitPoints.Text = Player1.CurrentHitPoints.ToString();

                LevelUp();

                if (Player1.CurrentHitPoints <= 0)
                {
                    // Display message
                    rtbMessages.Text += "The " + currentMonster.Name + " killed you." + Environment.NewLine;

                    // Move player to "Home"
                    MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
                }
            }
        }
        private void btnUsePotion_Click(object sender, EventArgs e)
        {
            // Get the currently selected potion from the combobox
            HealingPotion potion = (HealingPotion)cboPotions.SelectedItem;

            // Add healing amount to the player's current hit points
            Player1.CurrentHitPoints = (Player1.CurrentHitPoints + potion.AmountToHeal);

            // CurrentHitPoints cannot exceed player's MaximumHitPoints
            if (Player1.CurrentHitPoints > Player1.MaximumHitPoints)
            {
                Player1.CurrentHitPoints = Player1.MaximumHitPoints;
            }

            // Remove the potion from the player's inventory
            foreach (InventoryItem ii in Player1.Inventory)
            {
                if (ii.Details.ID == potion.ID)
                {
                    ii.Quantity--;
                    break;
                }
            }

            // Display message
            rtbMessages.Text += "You drink a " + potion.Name + Environment.NewLine;

            // Monster gets their turn to attack

            // Determine the amount of damage the monster does to the player
            int damageToPlayer = RandomNumberGenerator.NumberBetween(0, currentMonster.MaximumDamage);

            // Display message
            rtbMessages.Text += "The " + currentMonster.Name + " did " + damageToPlayer.ToString() + " points of damage." + Environment.NewLine;

            // Subtract damage from player
            Player1.CurrentHitPoints -= damageToPlayer;

            if (Player1.CurrentHitPoints <= 0)
            {
                // Display message
                rtbMessages.Text += "The " + currentMonster.Name + " killed you." + Environment.NewLine;

                // Move player to "Home"
                MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
            }

            // Refresh player data in UI
            lblHitPoints.Text = Player1.CurrentHitPoints.ToString();
            UpdateInventoryListInUI();
            UpdatePotionListInUI();
            
        }


        private void dgvInventory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvQuests_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


    

        private void rtbMessages_TextChanged(object sender, EventArgs e)
        {
            // set the current caret position to the end
            rtbMessages.SelectionStart = rtbMessages.Text.Length;
            // scroll it automatically
            rtbMessages.ScrollToCaret();
          

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    Bitmap image1 = (Bitmap)Image.FromFile(@"C:\Users\Mark\Desktop\GameImgs\Home.jpg", true);

            //    TextureBrush texture = new TextureBrush(image1);
            //    texture.WrapMode = System.Drawing.Drawing2D.WrapMode.Tile;
            //    Graphics formGraphics = this.CreateGraphics();
            //    formGraphics.FillEllipse(texture,
            //        new RectangleF(90.0F, 110.0F, 100, 100));
            //    formGraphics.Dispose();

            //}
            //catch (System.IO.FileNotFoundException)
            //{
            //    MessageBox.Show("There was an error opening the bitmap." +
            //        "Please check the path.");
            //}
        }

        

     

        private void YesMain_Click(object sender, EventArgs e)
        {
            MoveTo(World.LocationByID(World.LOCATION_ID_BURNED_CAR));
          
        }

        private void NoMain_Click(object sender, EventArgs e)
        {
            btnEast.Visible = true;
            btnWest.Visible = true;
            btnSouth.Visible = true;

            NoMain.Visible = false;
            YesMain.Visible = false;
        }
    }


    }


























//private void MoveTo(Location newLocation)
//{
//    //Does the location have any required items
//    if (newLocation.ItemRequiredToEnter != null)
//    {
//        // See if the player has the required item in their inventory
//        bool playerHasRequiredItem = false;

//        foreach (InventoryItem ii in Player1.Inventory)
//        {
//            if (ii.Details.ID == newLocation.ItemRequiredToEnter.ID)
//            {
//                // We found the required item
//                playerHasRequiredItem = true;
//                break; // Exit out of the foreach loop
//            }
//        }

//        if (!playerHasRequiredItem)
//        {
//            // We didn't find the required item in their inventory, so display a message and stop trying to move
//            rtbMessages.Text += "You must have a " + newLocation.ItemRequiredToEnter.Name + " to enter this location." + Environment.NewLine;
//            return;
//        }
//    }

//    // Update the player's current location
//    Player1.CurrentLocation = newLocation;

//    // Show/hide available movement buttons
//    btnNorth.Visible = (newLocation.LocationToNorth != null);
//    btnEast.Visible = (newLocation.LocationToEast != null);
//    btnSouth.Visible = (newLocation.LocationToSouth != null);
//    btnWest.Visible = (newLocation.LocationToWest != null);

//    // Display current location name and description
//    rtbLocation.Text = newLocation.Name + Environment.NewLine;
//    rtbLocation.Text += newLocation.Description + Environment.NewLine;

//    // Completely heal the player
//    Player1.CurrentHitPoints = Player1.MaximumHitPoints;

//    // Update Hit Points in UI
//    lblHitPoints.Text = Player1.CurrentHitPoints.ToString();

//    // Does the location have a quest?
//    if (newLocation.QuestAvailableHere != null)
//    {
//        // See if the player already has the quest, and if they've completed it
//        bool playerAlreadyHasQuest = false;
//        bool playerAlreadyCompletedQuest = false;

//        foreach (PlayerQuest playerQuest in Player1.Quests)
//        {
//            if (playerQuest.Details.ID == newLocation.QuestAvailableHere.ID)
//            {
//                playerAlreadyHasQuest = true;

//                if (playerQuest.IsCompleted)
//                {
//                    playerAlreadyCompletedQuest = true;
//                }
//            }
//        }

//        // See if the player already has the quest
//        if (playerAlreadyHasQuest)
//        {
//            // If the player has not completed the quest yet
//            if (!playerAlreadyCompletedQuest)
//            {
//                // See if the player has all the items needed to complete the quest
//                bool playerHasAllItemsToCompleteQuest = true;

//                foreach (QuestCompletionItem qci in newLocation.QuestAvailableHere.QuestCompletionItems)
//                {
//                    bool foundItemInPlayersInventory = false;

//                    // Check each item in the player's inventory, to see if they have it, and enough of it
//                    foreach (InventoryItem ii in Player1.Inventory)
//                    {
//                        // The player has this item in their inventory
//                        if (ii.Details.ID == qci.Details.ID)
//                        {
//                            foundItemInPlayersInventory = true;

//                            if (ii.Quantity < qci.Quantity)
//                            {
//                                // The player does not have enough of this item to complete the quest
//                                playerHasAllItemsToCompleteQuest = false;

//                                // There is no reason to continue checking for the other quest completion items
//                                break;
//                            }

//                            // We found the item, so don't check the rest of the player's inventory
//                            break;
//                        }
//                    }

//                    // If we didn't find the required item, set our variable and stop looking for other items
//                    if (!foundItemInPlayersInventory)
//                    {
//                        // The player does not have this item in their inventory
//                        playerHasAllItemsToCompleteQuest = false;

//                        // There is no reason to continue checking for the other quest completion items
//                        break;
//                    }
//                }

//                // The player has all items required to complete the quest
//                if (playerHasAllItemsToCompleteQuest)
//                {
//                    // Display message
//                    rtbMessages.Text += Environment.NewLine;
//                    rtbMessages.Text += "You complete the '" + newLocation.QuestAvailableHere.Name + "' quest." + Environment.NewLine;

//                    // Remove quest items from inventory
//                    foreach (QuestCompletionItem qci in newLocation.QuestAvailableHere.QuestCompletionItems)
//                    {
//                        foreach (InventoryItem ii in Player1.Inventory)
//                        {
//                            if (ii.Details.ID == qci.Details.ID)
//                            {
//                                // Subtract the quantity from the player's inventory that was needed to complete the quest
//                                ii.Quantity -= qci.Quantity;
//                                break;
//                            }
//                        }
//                    }

//                    // Give quest rewards
//                    rtbMessages.Text += "You receive: " + Environment.NewLine;
//                    rtbMessages.Text += newLocation.QuestAvailableHere.RewardExperiencePoints.ToString() + " experience points" + Environment.NewLine;
//                    rtbMessages.Text += newLocation.QuestAvailableHere.RewardGold.ToString() + " gold" + Environment.NewLine;
//                    rtbMessages.Text += newLocation.QuestAvailableHere.RewardItem.Name + Environment.NewLine;
//                    rtbMessages.Text += Environment.NewLine;

//                    Player1.ExperiencePoints += newLocation.QuestAvailableHere.RewardExperiencePoints;
//                    Player1.Gold += newLocation.QuestAvailableHere.RewardGold;

//                    // Add the reward item to the player's inventory
//                    bool addedItemToPlayerInventory = false;

//                    foreach (InventoryItem ii in Player1.Inventory)
//                    {
//                        if (ii.Details.ID == newLocation.QuestAvailableHere.RewardItem.ID)
//                        {
//                            // They have the item in their inventory, so increase the quantity by one
//                            ii.Quantity++;

//                            addedItemToPlayerInventory = true;

//                            break;
//                        }
//                    }

//                    // They didn't have the item, so add it to their inventory, with a quantity of 1
//                    if (!addedItemToPlayerInventory)
//                    {
//                        Player1.Inventory.Add(new InventoryItem(newLocation.QuestAvailableHere.RewardItem, 1));
//                    }

//                    // Mark the quest as completed
//                    // Find the quest in the player's quest list
//                    foreach (PlayerQuest pq in Player1.Quests)
//                    {
//                        if (pq.Details.ID == newLocation.QuestAvailableHere.ID)
//                        {
//                            // Mark it as completed
//                            pq.IsCompleted = true;

//                            break;
//                        }
//                    }
//                }
//            }
//        }
//        else
//        {
//            // The player does not already have the quest

//            // Display the messages
//            rtbMessages.Text += "You receive the " + newLocation.QuestAvailableHere.Name + " quest." + Environment.NewLine;
//            rtbMessages.Text += newLocation.QuestAvailableHere.Description + Environment.NewLine;
//            rtbMessages.Text += "To complete it, return with:" + Environment.NewLine;
//            foreach (QuestCompletionItem qci in newLocation.QuestAvailableHere.QuestCompletionItems)
//            {
//                if (qci.Quantity == 1)
//                {
//                    rtbMessages.Text += qci.Quantity.ToString() + " " + qci.Details.Name + Environment.NewLine;
//                }
//                else
//                {
//                    rtbMessages.Text += qci.Quantity.ToString() + " " + qci.Details.NamePlural + Environment.NewLine;
//                }
//            }
//            rtbMessages.Text += Environment.NewLine;

//            // Add the quest to the player's quest list
//            Player1.Quests.Add(new PlayerQuest(newLocation.QuestAvailableHere));
//        }
//    }

//    // Does the location have a monster?
//    if (newLocation.MonsterLivingHere != null)
//    {
//        rtbMessages.Text += "You see a " + newLocation.MonsterLivingHere.Name + Environment.NewLine;

//        // Make a new monster, using the values from the standard monster in the World.Monster list
//        Monster standardMonster = World.MonsterByID(newLocation.MonsterLivingHere.ID);

//        currentMonster = new Monster(standardMonster.ID, standardMonster.Name, standardMonster.MaximumDamage,
//            standardMonster.RewardExperiencePoints, standardMonster.RewardGold, standardMonster.CurrentHitPoints, standardMonster.MaximumHitPoints);

//        foreach (LootItem lootItem in standardMonster.LootTable)
//        {
//            currentMonster.LootTable.Add(lootItem);
//        }

//        cboWeapons.Visible = true;
//        cboPotions.Visible = true;
//        //btnUseWeapon.Visible = true;
//        btnUsePotion.Visible = true;
//    }
//    else
//    {
//        currentMonster = null;

//        cboWeapons.Visible = false;
//        cboPotions.Visible = false;
//        //btnUseWeapon.Visible = false;
//        btnUsePotion.Visible = false;
//    }

//    // Refresh player's inventory list
//    dgvInventory.RowHeadersVisible = false;

//    dgvInventory.ColumnCount = 2;
//    dgvInventory.Columns[0].Name = "Name";
//    dgvInventory.Columns[0].Width = 197;
//    dgvInventory.Columns[1].Name = "Quantity";

//    dgvInventory.Rows.Clear();

//    foreach (InventoryItem inventoryItem in Player1.Inventory)
//    {
//        if (inventoryItem.Quantity > 0)
//        {
//            dgvInventory.Rows.Add(new[] { inventoryItem.Details.Name, inventoryItem.Quantity.ToString() });
//        }
//    }

//    // Refresh player's quest list
//    dgvQuests.RowHeadersVisible = false;

//    dgvQuests.ColumnCount = 2;
//    dgvQuests.Columns[0].Name = "Name";
//    dgvQuests.Columns[0].Width = 197;
//    dgvQuests.Columns[1].Name = "Done?";

//    dgvQuests.Rows.Clear();

//    foreach (PlayerQuest playerQuest in Player1.Quests)
//    {
//        dgvQuests.Rows.Add(new[] { playerQuest.Details.Name, playerQuest.IsCompleted.ToString() });
//    }

//    // Refresh player's weapons combobox
//    List<Weapon> weapons = new List<Weapon>();

//    foreach (InventoryItem inventoryItem in Player1.Inventory)
//    {
//        if (inventoryItem.Details is Weapon)
//        {
//            if (inventoryItem.Quantity > 0)
//            {
//                weapons.Add((Weapon)inventoryItem.Details);
//            }
//        }
//    }

//    if (weapons.Count == 0)
//    {
//        // The player doesn't have any weapons, so hide the weapon combobox and "Use" button
//        cboWeapons.Visible = false;
//        //btnUseWeapon.Visible = false;
//    }
//    else
//    {
//        cboWeapons.DataSource = weapons;
//        cboWeapons.DisplayMember = "Name";
//        cboWeapons.ValueMember = "ID";

//        cboWeapons.SelectedIndex = 0;
//    }

//    // Refresh player's potions combobox
//    List<HealingPotion> healingPotions = new List<HealingPotion>();

//    foreach (InventoryItem inventoryItem in Player1.Inventory)
//    {
//        if (inventoryItem.Details is HealingPotion)
//        {
//            if (inventoryItem.Quantity > 0)
//            {
//                healingPotions.Add((HealingPotion)inventoryItem.Details);
//            }
//        }
//    }

//    if (healingPotions.Count == 0)
//    {
//        // The player doesn't have any potions, so hide the potion combobox and "Use" button
//        cboPotions.Visible = false;
//        btnUsePotion.Visible = false;
//    }
//    else
//    {
//        cboPotions.DataSource = healingPotions;
//        cboPotions.DisplayMember = "Name";
//        cboPotions.ValueMember = "ID";

//        cboPotions.SelectedIndex = 0;
//    }
//}

//private void btnUseWeapon_Click(object sender, EventArgs e)
//{

//}

//private void btnUsePotion_Click(object sender, EventArgs e)
//{

//}

//private void btnNorth_Click(object sender, EventArgs e)
//{

//}

//private void btnSouth_Click(object sender, EventArgs e)
//{

//}

//private void btnWest_Click(object sender, EventArgs e)
//{

//}

//private void btnEast_Click(object sender, EventArgs e)
//{

//}

//private void dgvQuests_CellContentClick(object sender, DataGridViewCellEventArgs e)
//{

//}

//private void dgvInventory_CellContentClick(object sender, DataGridViewCellEventArgs e)
//{

//}