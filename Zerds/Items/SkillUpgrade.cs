using Zerds.Constants;
using Zerds.Data;

namespace Zerds.Items
{
    public class SkillUpgrade
    {
        public SkillType Type { get; set; }
        public int UpgradeAmount { get; set; }

        public SkillUpgrade(SkillType type, int upgradeAmount)
        {
            Type = type;
            UpgradeAmount = upgradeAmount;
        }

        public SkillUpgrade(SaveGameSkillUpgrade u)
        {
            Type = (SkillType)u.Type;
            UpgradeAmount = u.UpgradeAmount;
        }
    }
}
