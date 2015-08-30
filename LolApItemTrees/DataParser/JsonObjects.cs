using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser
{
    /// <summary>
    /// These are the input objects downloaded from the Riot API
    /// </summary>
    #region JsonInputObject
    public class CreepsPerMinDeltas
    {
        public double zeroToTen { get; set; }
        public double tenToTwenty { get; set; }
        public double twentyToThirty { get; set; }
        public double thirtyToEnd { get; set; }
    }

    public class XpPerMinDeltas
    {
        public double zeroToTen { get; set; }
        public double tenToTwenty { get; set; }
        public double twentyToThirty { get; set; }
        public double thirtyToEnd { get; set; }
    }

    public class GoldPerMinDeltas
    {
        public double zeroToTen { get; set; }
        public double tenToTwenty { get; set; }
        public double twentyToThirty { get; set; }
        public double thirtyToEnd { get; set; }
    }

    public class CsDiffPerMinDeltas
    {
        public double zeroToTen { get; set; }
        public double tenToTwenty { get; set; }
        public double twentyToThirty { get; set; }
        public double thirtyToEnd { get; set; }
    }

    public class XpDiffPerMinDeltas
    {
        public double zeroToTen { get; set; }
        public double tenToTwenty { get; set; }
        public double twentyToThirty { get; set; }
        public double thirtyToEnd { get; set; }
    }

    public class DamageTakenPerMinDeltas
    {
        public double zeroToTen { get; set; }
        public double tenToTwenty { get; set; }
        public double twentyToThirty { get; set; }
        public double thirtyToEnd { get; set; }
    }

    public class DamageTakenDiffPerMinDeltas
    {
        public double zeroToTen { get; set; }
        public double tenToTwenty { get; set; }
        public double twentyToThirty { get; set; }
        public double thirtyToEnd { get; set; }
    }

    public class Timeline
    {
        public CreepsPerMinDeltas creepsPerMinDeltas { get; set; }
        public XpPerMinDeltas xpPerMinDeltas { get; set; }
        public GoldPerMinDeltas goldPerMinDeltas { get; set; }
        public CsDiffPerMinDeltas csDiffPerMinDeltas { get; set; }
        public XpDiffPerMinDeltas xpDiffPerMinDeltas { get; set; }
        public DamageTakenPerMinDeltas damageTakenPerMinDeltas { get; set; }
        public DamageTakenDiffPerMinDeltas damageTakenDiffPerMinDeltas { get; set; }
        public string role { get; set; }
        public string lane { get; set; }
    }

    public class Mastery
    {
        public int masteryId { get; set; }
        public int rank { get; set; }
    }

    public class Stats
    {
        public bool winner { get; set; }
        public int champLevel { get; set; }
        public int item0 { get; set; }
        public int item1 { get; set; }
        public int item2 { get; set; }
        public int item3 { get; set; }
        public int item4 { get; set; }
        public int item5 { get; set; }
        public int item6 { get; set; }
        public int kills { get; set; }
        public int doubleKills { get; set; }
        public int tripleKills { get; set; }
        public int quadraKills { get; set; }
        public int pentaKills { get; set; }
        public int unrealKills { get; set; }
        public int largestKillingSpree { get; set; }
        public int deaths { get; set; }
        public int assists { get; set; }
        public int totalDamageDealt { get; set; }
        public int totalDamageDealtToChampions { get; set; }
        public int totalDamageTaken { get; set; }
        public int largestCriticalStrike { get; set; }
        public int totalHeal { get; set; }
        public int minionsKilled { get; set; }
        public int neutralMinionsKilled { get; set; }
        public int neutralMinionsKilledTeamJungle { get; set; }
        public int neutralMinionsKilledEnemyJungle { get; set; }
        public int goldEarned { get; set; }
        public int goldSpent { get; set; }
        public int combatPlayerScore { get; set; }
        public int objectivePlayerScore { get; set; }
        public int totalPlayerScore { get; set; }
        public int totalScoreRank { get; set; }
        public int magicDamageDealtToChampions { get; set; }
        public int physicalDamageDealtToChampions { get; set; }
        public int trueDamageDealtToChampions { get; set; }
        public int visionWardsBoughtInGame { get; set; }
        public int sightWardsBoughtInGame { get; set; }
        public int magicDamageDealt { get; set; }
        public int physicalDamageDealt { get; set; }
        public int trueDamageDealt { get; set; }
        public int magicDamageTaken { get; set; }
        public int physicalDamageTaken { get; set; }
        public int trueDamageTaken { get; set; }
        public bool firstBloodKill { get; set; }
        public bool firstBloodAssist { get; set; }
        public bool firstTowerKill { get; set; }
        public bool firstTowerAssist { get; set; }
        public bool firstInhibitorKill { get; set; }
        public bool firstInhibitorAssist { get; set; }
        public int inhibitorKills { get; set; }
        public int towerKills { get; set; }
        public int wardsPlaced { get; set; }
        public int wardsKilled { get; set; }
        public int largestMultiKill { get; set; }
        public int killingSprees { get; set; }
        public int totalUnitsHealed { get; set; }
        public int totalTimeCrowdControlDealt { get; set; }
    }

    public class Rune
    {
        public int runeId { get; set; }
        public int rank { get; set; }
    }

    public class Participant
    {
        public int teamId { get; set; }
        public int spell1Id { get; set; }
        public int spell2Id { get; set; }
        public int championId { get; set; }
        public string highestAchievedSeasonTier { get; set; }
        public Timeline timeline { get; set; }
        public List<Mastery> masteries { get; set; }
        public Stats stats { get; set; }
        public int participantId { get; set; }
        public List<Rune> runes { get; set; }
    }

    public class Player
    {
        public int summonerId { get; set; }
        public string summonerName { get; set; }
        public string matchHistoryUri { get; set; }
        public int profileIcon { get; set; }
    }

    public class ParticipantIdentity
    {
        public int participantId { get; set; }
        public Player player { get; set; }
    }

    public class Ban
    {
        public int championId { get; set; }
        public int pickTurn { get; set; }
    }

    public class Team
    {
        public int teamId { get; set; }
        public bool winner { get; set; }
        public bool firstBlood { get; set; }
        public bool firstTower { get; set; }
        public bool firstInhibitor { get; set; }
        public bool firstBaron { get; set; }
        public bool firstDragon { get; set; }
        public int towerKills { get; set; }
        public int inhibitorKills { get; set; }
        public int baronKills { get; set; }
        public int dragonKills { get; set; }
        public int vilemawKills { get; set; }
        public int dominionVictoryScore { get; set; }
        public List<Ban> bans { get; set; }
    }

    public class Position
    {
        public int x { get; set; }
        public int y { get; set; }
    }

    public class ParticipantFrame
    {
        public int participantId { get; set; }
        public Position position { get; set; }
        public int currentGold { get; set; }
        public int totalGold { get; set; }
        public int level { get; set; }
        public int xp { get; set; }
        public int minionsKilled { get; set; }
        public int jungleMinionsKilled { get; set; }
        public int dominionScore { get; set; }
        public int teamScore { get; set; }
    }

    public class Position11
    {
        public int x { get; set; }
        public int y { get; set; }
    }

    public class Event
    {
        public string eventType { get; set; }
        public int timestamp { get; set; }
        public int skillSlot { get; set; }
        public int participantId { get; set; }
        public string levelUpType { get; set; }
        public int? itemId { get; set; }
        public int? creatorId { get; set; }
        public string wardType { get; set; }
        public int? killerId { get; set; }
        public int? victimId { get; set; }
        public List<int?> assistingParticipantIds { get; set; }
        public Position11 position { get; set; }
        public int? itemBefore { get; set; }
        public int? itemAfter { get; set; }
        public int? teamId { get; set; }
        public string laneType { get; set; }
        public string buildingType { get; set; }
        public string towerType { get; set; }
        public string monsterType { get; set; }
    }

    public class Frame
    {
        public Dictionary<string, ParticipantFrame> participantFrames { get; set; }
        public int timestamp { get; set; }
        public List<Event> events { get; set; }
    }

    public class Timeline2
    {
        public List<Frame> frames { get; set; }
        public int frameInterval { get; set; }
    }

    public class RootObject
    {
        public long matchId { get; set; }
        public string region { get; set; }
        public string platformId { get; set; }
        public string matchMode { get; set; }
        public string matchType { get; set; }
        public long matchCreation { get; set; }
        public int matchDuration { get; set; }
        public string queueType { get; set; }
        public int mapId { get; set; }
        public string season { get; set; }
        public string matchVersion { get; set; }
        public List<Participant> participants { get; set; }
        public List<ParticipantIdentity> participantIdentities { get; set; }
        public List<Team> teams { get; set; }
        public Timeline2 timeline { get; set; }
    }

    #endregion

    /// <summary>
    /// These are the objects we will output after parsing the data.
    /// </summary>
    #region JsonOutputObject

    /// <summary>
    /// A node in the item build tree
    /// </summary>
    class JsonItemNode
    {
        public int itemId;
        public long totalBuildTime;
        public int numberOfBuilds;
        public int numberOfWins;
        public Dictionary<string, int> thirtySecondIntervals;
        public List<JsonItemNode> children;
    }

    /// <summary>
    /// Statistics on a single champion
    /// </summary>
    class ChampionStatistics
    {
        /// <summary>
        /// The root node of the item tree
        /// </summary>
        public JsonItemNode itemPaths;

        /// <summary>
        /// All items built, as a mapping of item Ids to number of builds
        /// </summary>
        public Dictionary<string, int> totalItemsBuilt;

        /// <summary>
        /// The total number of games parsed with this champion
        /// </summary>
        public int numberOfDataPoints;
    }

    /// <summary>
    /// The root output object
    /// </summary>
    class JsonRoot
    {
        /// <summary>
        /// The dictionary of champion to statistics mappings
        /// </summary>
        public Dictionary<string, ChampionStatistics> championStatistics;

        /// <summary>
        /// The patch these statistics are gathered from
        /// </summary>
        public string patch;
    }
    #endregion

}
