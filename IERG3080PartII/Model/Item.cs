using System;
using System.Collections.Generic;
using System.Text;

namespace IERG3080PartII.Model {
    public static class ItemFactory {
        public static string randomItem() {
            Random rnd = new Random();

            if (rnd.Next(0, 101) < 33) {
                return "HPPotion";
            }
            else if (rnd.Next(0, 101) < 66) {
                return "MPPotion";
            }
            else {
                return "PowerUpPotion";
            }
        }
    }

    public abstract class Item 
    {

    }

    public abstract class Potion : Item 
    {
        public abstract int onUse();
    }

    public class HealthPotion : Potion
    {
        private static int healthBoost = 30;

        public override int onUse()
        {
            return healthBoost;
        }
    }

    public class MPPotion : Potion
    {
        private static int MPBoost = 20;

        public override int onUse()
        {
            return MPBoost;
        }
    }

    public class PowerupPotion : Potion 
    {
        // details needed
        public override int onUse()
        {
            throw new NotImplementedException(); // details here
        }
    }

}
