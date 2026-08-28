using System;
using System.Collections.Generic;

namespace HallownestWayfinder
{
    public interface IGameState
    {
        string SceneName { get; }
        bool AtBench { get; }
        string RespawnScene { get; }
        int RelicCount { get; }
        int CompletedPantheons { get; }
        int GrubsCollected { get; }
        int GrubSceneCount { get; }
        int CharmsOwned { get; }
        int MaxHealthBase { get; }
        int SoulReserveMaximum { get; }
        int NailUpgrades { get; }
        int CurrentEssence { get; }
        int SpentEssence { get; }

        bool GetBool(string field);
        int GetInt(string field);
        bool HasVisitedScene(string scene);
        bool HasRescuedGrub(string scene);
    }

    /// <summary>
    /// Immutable view of the expensive save collections. Dynamic PlayerData fields
    /// are memoized, so every named bool/int is read at most once per refresh.
    /// </summary>
    public sealed class PlayerDataGameState : IGameState
    {
        private readonly PlayerData _player;
        private readonly HashSet<string> _visitedScenes;
        private readonly HashSet<string> _rescuedGrubScenes;
        private readonly Dictionary<string, bool> _bools =
            new Dictionary<string, bool>(StringComparer.Ordinal);
        private readonly Dictionary<string, int> _ints =
            new Dictionary<string, int>(StringComparer.Ordinal);

        private PlayerDataGameState(PlayerData player, string sceneName)
        {
            _player = player;
            SceneName = sceneName;
            _visitedScenes = player.scenesVisited == null
                ? new HashSet<string>(StringComparer.Ordinal)
                : new HashSet<string>(player.scenesVisited, StringComparer.Ordinal);
            _rescuedGrubScenes = player.scenesGrubRescued == null
                ? new HashSet<string>(StringComparer.Ordinal)
                : new HashSet<string>(player.scenesGrubRescued, StringComparer.Ordinal);

            AtBench = player.atBench;
            RespawnScene = player.respawnScene ?? string.Empty;
            RelicCount = player.trinket1 + player.trinket2 + player.trinket3 + player.trinket4;
            CompletedPantheons = CountCompletedPantheons(player);
            GrubsCollected = player.grubsCollected;
            GrubSceneCount = player.scenesGrubRescued?.Count ?? 0;
            CharmsOwned = player.charmsOwned;
            MaxHealthBase = player.maxHealthBase;
            SoulReserveMaximum = player.MPReserveMax;
            NailUpgrades = player.nailSmithUpgrades;
            CurrentEssence = player.dreamOrbs;
            SpentEssence = player.dreamOrbsSpent;
        }

        public string SceneName { get; }
        public bool AtBench { get; }
        public string RespawnScene { get; }
        public int RelicCount { get; }
        public int CompletedPantheons { get; }
        public int GrubsCollected { get; }
        public int GrubSceneCount { get; }
        public int CharmsOwned { get; }
        public int MaxHealthBase { get; }
        public int SoulReserveMaximum { get; }
        public int NailUpgrades { get; }
        public int CurrentEssence { get; }
        public int SpentEssence { get; }

        public static bool TryCapture(out PlayerDataGameState? state)
        {
            PlayerData? player = PlayerData.instance;
            GameManager? game = GameManager.instance;
            if (player == null || game == null)
            {
                state = null;
                return false;
            }

            state = new PlayerDataGameState(player, game.sceneName ?? string.Empty);
            return true;
        }

        public bool GetBool(string field)
        {
            if (!_bools.TryGetValue(field, out bool value))
            {
                value = _player.GetBool(field);
                _bools[field] = value;
            }
            return value;
        }

        public int GetInt(string field)
        {
            if (!_ints.TryGetValue(field, out int value))
            {
                value = _player.GetInt(field);
                _ints[field] = value;
            }
            return value;
        }

        public bool HasVisitedScene(string scene) => _visitedScenes.Contains(scene);
        public bool HasRescuedGrub(string scene) => _rescuedGrubScenes.Contains(scene);

        private static int CountCompletedPantheons(PlayerData player)
        {
            int completed = 0;
            if (player.bossDoorStateTier1.completed) completed++;
            if (player.bossDoorStateTier2.completed) completed++;
            if (player.bossDoorStateTier3.completed) completed++;
            if (player.bossDoorStateTier4.completed) completed++;
            return completed;
        }
    }
}
