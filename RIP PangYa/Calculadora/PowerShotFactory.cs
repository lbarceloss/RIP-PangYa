namespace Calculadora
{

    public enum ShotTypeEnum
    {
        DUNK,
        TOMAHAWK,
        SPIKE,
        COBRA
    };

    public enum PowerShotFactoryEnum
    {
        NO_POWER_SHOT,
        ONE_POWER_SHOT,
        TWO_POWER_SHOT,
        ITEM_15_POWER_SHOT
    }

    public static class PowerShotFactory
    {

        public const int NO_POWER_SHOT = 0;
        public const int ONE_POWER_SHOT = 1;
        public const int TWO_POWER_SHOT = 2;
        public const int ITEM_15_POWER_SHOT = 3;

        public static int GetPowerShotFactory(int powershot)
        {
            int power_shot_factory = 0;

            switch (powershot)
            {
                case ONE_POWER_SHOT:
                    power_shot_factory = 10;
                    break;
                case TWO_POWER_SHOT:
                    power_shot_factory = 20;
                    break;
                case ITEM_15_POWER_SHOT:
                    power_shot_factory = 15;
                    break;
            }
            return power_shot_factory;
        }

        public static int GetIntValueFromShotType(ShotTypeEnum type)
        {
            int value = 0;
            switch (type)
            {
                case ShotTypeEnum.DUNK:
                    value = 0;
                    break;
                case ShotTypeEnum.TOMAHAWK:
                    value = 1;
                    break;
                case ShotTypeEnum.SPIKE:
                    value = 2;
                    break;
                case ShotTypeEnum.COBRA:
                    value = 3;
                    break;
            }
            return value;
        }

        public static int GetIntValueFromShotTypePassingAString(string type)
        {
            int value = 0;
            switch (type)
            {
                case "DUNK":
                    value = 0;
                    break;
                case "TOMAHAWK":
                    value = 1;
                    break;
                case "SPIKE":
                    value = 2;
                    break;
                case "COBRA":
                    value = 3;
                    break;
            }
            return value;
        }
    }
}
