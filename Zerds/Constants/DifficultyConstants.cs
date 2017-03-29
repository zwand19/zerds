using Zerds.Enums;

namespace Zerds.Constants
{
    public static class DifficultyConstants
    {
        public static float HealthFactor => Get(1.0f, 1.25f, 1.4f, 1.2f, 1.5f, 1.9f);
        public static float ManaFactor => Get(1.0f, 1.25f, 1.4f, 1.1f, 1.2f, 1.32f);
        public static float SpeedFactor => Get(1.0f, 1.1f, 1.18f, 1.05f, 1.1f, 1.16f);

        public static int RevivalTeammatePenalty = 5;
        public static int RevivalSelfPenalty = 10;

        private static float Get(float easy, float medium, float hard, float twoPlayers, float threePlayers, float fourPlayers)
        {
            switch (Globals.GameState.Zerds.Count)
            {
                case 2:
                    easy *= twoPlayers;
                    medium *= twoPlayers;
                    hard *= twoPlayers;
                    break;
                case 3:
                    easy *= threePlayers;
                    medium *= threePlayers;
                    hard *= threePlayers;
                    break;
                case 4:
                    easy *= fourPlayers;
                    medium *= fourPlayers;
                    hard *= fourPlayers;
                    break;
            }
            return GameSettings.Difficulty == Difficulty.Easy ? easy : GameSettings.Difficulty == Difficulty.Medium ? medium : hard;
        }

        private static int Get(int easy, int medium, int hard)
        {
            return GameSettings.Difficulty == Difficulty.Easy ? easy : GameSettings.Difficulty == Difficulty.Medium ? medium : hard;
        }
    }
}
