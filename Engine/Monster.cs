using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Monster : LivingCreature
    {
        public int ID { get; set; }
        public string Name { get; set; }
  
        public int MaximumDamage { get; set; }
        public int RewardExperiencePoints { get; set; }
        public int RewardGold { get; set; }

        public List<LootItem> LootTable { get; set; }

        public Monster(int id, string name, int MaxDamage, int RewardExPoints, int rewardGold, int MaxHitPoints, int currentHitPoints)
            :base(MaxHitPoints,currentHitPoints)
        {
            this.ID = id;
            this.MaximumDamage = MaxDamage;
            this.Name = name;
            this.RewardExperiencePoints = RewardExPoints;
            this.RewardGold = rewardGold;

            LootTable = new List<LootItem>();

        }
    }
}