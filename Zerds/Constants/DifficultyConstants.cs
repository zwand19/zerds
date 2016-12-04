using Zerds.Enums;

namespace Zerds.Constants
{
    public static class DifficultyConstants
    {
        public static float HealthFactor => Get(1.0f, 1.25f, 1.4f);
        public static float ManaFactor => Get(1.0f, 1.25f, 1.4f);
        public static float SpeedFactor => Get(1.0f, 1.1f, 1.18f);
        public static int LevelLength = 90;

        private static float Get(float easy, float medium, float hard)
        {
            return GameSettings.Difficulty == Difficulty.Easy ? easy : GameSettings.Difficulty == Difficulty.Medium ? medium : hard;
        }

        private static int Get(int easy, int medium, int hard)
        {
            return GameSettings.Difficulty == Difficulty.Easy ? easy : GameSettings.Difficulty == Difficulty.Medium ? medium : hard;
        }
    }
}
