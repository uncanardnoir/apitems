using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser
{
    public static class Extensions
    {
        /// <summary>
        /// Gets a value from the dictionary, or adds and returns a default value if it doesn't exist
        /// </summary>
        /// <typeparam name="T">The key type of the dictionary</typeparam>
        /// <typeparam name="U">The value type of the dictionary</typeparam>
        /// <param name="dict">The dictionary</param>
        /// <param name="key">The key</param>
        /// <returns>The value</returns>
        public static U GetOrAddDefault<T, U>(this Dictionary<T, U> dict, T key) where U : new()
        {
            if (dict.ContainsKey(key))
            {
                return dict[key];
            }
            else
            {
                U value = new U();
                dict[key] = value;
                return value;
            }
        }
    }

    /// <summary>
    /// Utilities used by the data parser and analyzer
    /// </summary>
    class Utilities
    {
        /// <summary>
        /// Calculates the similarity score of two champions, by calculating the angle between their vectors
        /// </summary>
        /// <param name="d1">The first champion build dictionary</param>
        /// <param name="n1">The number of data points of the first champion</param>
        /// <param name="d2">The second champion build dictionary</param>
        /// <param name="n2">The number of data points of the second champion</param>
        /// <returns></returns>
        public static double CalculateSimilarityScore(Dictionary<string, int> d1, int n1, Dictionary<string, int> d2, int n2)
        {
            // calculate vector norms
            double length1 = 0.0;
            foreach (var value in d1.Values)
            {
                length1 += (double)(value) / n1 * value / n1;
            }
            length1 = Math.Sqrt(length1);
            double length2 = 0.0;
            foreach (var value in d2.Values)
            {
                length2 += (double)(value) / n2 * value / n2;
            }
            length2 = Math.Sqrt(length2);
            // calculate dot product
            double dp = 0.0;
            foreach (var key in d1.Keys)
            {
                if (d2.ContainsKey(key))
                {
                    dp += ((double)d1[key] / n1 / length1) * ((double)d2[key] / n2 / length2);
                }
            }
            return dp >= 1 ? 100 : 100 - Math.Acos(dp) * 100 / Math.PI;
        }

        /// <summary>
        /// Determines whether or not a champion is an AP mage
        /// </summary>
        /// <param name="champId">The champion ID</param>
        /// <returns>Whether or not the champion is an AP mage</returns>
        public static bool IsApChampion(int champId)
        {
            switch (champId)
            {
                case 1: // Annie
                case 3: // Galio
                case 4: // Twisted Fate
                case 7: // Leblanc
                case 8: // Vladimir
                case 9: // Fiddlesticks
                case 10: // Kayle
                case 13: // Ryze
                case 25: // Morgana
                case 26: // Zilean
                case 30: // Karthus
                case 31: // Cho'gath
                case 34: // Anivia
                case 38: // Kassadin
                case 43: // Karma
                case 45: // Veigar
                case 50: // Swain
                case 55: // Katarina
                case 60: // Elise
                case 61: // Orianna
                case 63: // Brand
                case 68: // Rumble
                case 69: // Cassiopeia
                case 74: // Heimerdinger
                case 76: // Nidalee
                case 79: // Gragas
                case 81: // Ezreal
                case 82: // Mordekaiser
                case 84: // Akali
                case 85: // Kennen
                case 90: // Malzahar
                case 96: // Kog'Maw
                case 99: // Lux
                case 101: // Xerath
                case 103: // Ahri
                case 105: // Fizz
                case 112: // Viktor
                case 115: // Ziggs
                case 117: // Lulu
                case 127: // Lissandra
                case 131: // Diana
                case 134: // Syndra
                case 143: // Zyra
                case 161: // Vel'koz
                case 245: // Ekko
                case 268: // Azir
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Gets the name of a champion
        /// </summary>
        /// <param name="champId">The champion ID</param>
        /// <returns>The champion name</returns>
        public static string ChampionIdToName(int champId) {
            switch (champId)
            {
                case 1: return "Annie";
                case 2: return "Olaf";
                case 3: return "Galio";
                case 4: return "Twisted Fate";
                case 5: return "Xin Zhao";
                case 6: return "Urgot";
                case 7: return "Leblanc";
                case 8: return "Vladimir";
                case 9: return "Fiddlesticks";
                case 10: return "Kayle";
                case 11: return "Master Yi";
                case 12: return "Alistar";
                case 13: return "Ryze";
                case 14: return "Sion";
                case 15: return "Sivir";
                case 16: return "Soraka";
                case 17: return "Teemo";
                case 18: return "Tristana";
                case 19: return "Warwick";
                case 20: return "Nunu";
                case 21: return "Miss Fortune";
                case 22: return "Ashe";
                case 23: return "Tryndamere";
                case 24: return "Jax";
                case 25: return "Morgana";
                case 26: return "Zilean";
                case 27: return "Singed";
                case 28: return "Evelynn";
                case 29: return "Twitch";
                case 30: return "Karthus";
                case 31: return "Cho'gath";
                case 32: return "Amumu";
                case 33: return "Rammus";
                case 34: return "Anivia";
                case 35: return "Shaco";
                case 36: return "Dr. Mundo";
                case 37: return "Sona";
                case 38: return "Kassadin";
                case 39: return "Irelia";
                case 40: return "Janna";
                case 41: return "Gangplank";
                case 42: return "Corki";
                case 43: return "Karma";
                case 44: return "Taric";
                case 45: return "Veigar";
                case 48: return "Trundle";
                case 50: return "Swain";
                case 51: return "Caitlyn";
                case 53: return "Blitzcrank";
                case 54: return "Malphite";
                case 55: return "Katarina";
                case 56: return "Nocturne";
                case 57: return "Maokai";
                case 58: return "Renekton";
                case 59: return "Jarvan IV";
                case 60: return "Elise";
                case 61: return "Orianna";
                case 62: return "Wukong";
                case 63: return "Brand";
                case 64: return "Lee Sin";
                case 67: return "Vayne";
                case 68: return "Rumble";
                case 69: return "Cassiopeia";
                case 72: return "Skarner";
                case 74: return "Heimerdinger";
                case 75: return "Nasus";
                case 76: return "Nidalee";
                case 77: return "Udyr";
                case 78: return "Poppy";
                case 79: return "Gragas";
                case 80: return "Pantheon";
                case 81: return "Ezreal";
                case 82: return "Mordekaiser";
                case 83: return "Yorick";
                case 84: return "Akali";
                case 85: return "Kennen";
                case 86: return "Garen";
                case 89: return "Leona";
                case 90: return "Malzahar";
                case 91: return "Talon";
                case 92: return "Riven";
                case 96: return "Kog'Maw";
                case 98: return "Shen";
                case 99: return "Lux";
                case 101: return "Xerath";
                case 102: return "Shyvana";
                case 103: return "Ahri";
                case 104: return "Graves";
                case 105: return "Fizz";
                case 106: return "Volibear";
                case 107: return "Rengar";
                case 110: return "Varus";
                case 111: return "Nautilus";
                case 112: return "Viktor";
                case 113: return "Sejuani";
                case 114: return "Fiora";
                case 115: return "Ziggs";
                case 117: return "Lulu";
                case 119: return "Draven";
                case 120: return "Hecarim";
                case 121: return "Kha'zix";
                case 122: return "Darius";
                case 126: return "Jayce";
                case 127: return "Lissandra";
                case 131: return "Diana";
                case 133: return "Quinn";
                case 134: return "Syndra";
                case 143: return "Zyra";
                case 150: return "Gnar";
                case 154: return "Zac";
                case 157: return "Yasuo";
                case 161: return "Vel'koz";
                case 201: return "Braum";
                case 222: return "Jinx";
                case 223: return "Tahm Kench";
                case 236: return "Lucian";
                case 238: return "Zed";
                case 245: return "Ekko";
                case 254: return "Vi";
                case 266: return "Aatrox";
                case 267: return "Nami";
                case 268: return "Azir";
                case 412: return "Thresh";
                case 421: return "Rek'Sai";
                case 429: return "Kalista";
                case 432: return "Bard";
                case 999: return "All Mages";
                default:
                    Debug.Assert(false);
                    return null;
            }
        }

        /// <summary>
        /// Consolidates items which can have multiple IDs into the same ID (such as Runeglaive)
        /// </summary>
        /// <param name="id">The item ID</param>
        /// <returns>Thte consolidated ID</returns>
        public static int ConsolidateItemIds(int id)
        {
            switch (id)
            {
                case 3040:
                case 3048: // Seraph's Embrace:
                    return 3040;
                case 3032:
                case 3290: // Twin Shadows
                    return 3032;
                case 3708:
                case 3716:
                case 3720:
                case 3724: // Runeglaive
                    return 3708;
                case 3725: // Cinderhulk
                case 3721: // Cinderhulk
                case 3717: // Cinderhulk
                case 3709: // Cinderhulk
                    return 3725;
                default:
                    return id;
            }
        }

        /// <summary>
        /// Whether or not this is a final AP item
        /// </summary>
        /// <param name="id">The item ID</param>
        /// <returns>Whether or not this is a final AP item</returns>
        public static bool IsFinalApItem(int id)
        {
            switch (id)
            {
                case 3089: // Rabadon's Deathcap
                case 3157: // Zhonya's Hourglass
                case 3285: // Luden's Echo
                case 3116: // Rylai's Crystal Scepter
                case 3048: // Seraph's Embrace
                case 3027: // Rod of Ages
                case 3151: // Liandry's Torment
                case 3135: // Void Staff
                case 3115: // Nashor's Tooth
                case 3152: // Will of the Ancients
                case 3165: // Morellonomicon
                case 3174: // Athene's Unholy Grail
                case 3720: // Enchantment: Runeglaive
                case 3708: // Enchantment: Runeglaive
                case 3716: // Enchantment: Runeglaive
                case 3290: // Twin Shadows
                case 3102: // Banshee's Veil
                case 3100: // Lich Bane
                case 3001: // Abyssal Scepter
                case 3023: // Twin Shadows
                case 3198: // Perfect Hex Core
                case 3040: // Seraph's Embrace
                case 3041: // Mejai's Soulstealer
                case 3065: // Spirit Visage
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Whether or not this is a final item
        /// </summary>
        /// <param name="id">The item ID</param>
        /// <returns>Whether or not this is a final item</returns>
        public static bool IsFinalItem(int id)
        {
            switch (id)
            {
                case 3089: // Rabadon's Deathcap
                case 3087: // Statikk Shiv
                case 3720: // Enchantment: Runeglaive
                case 3085: // Runaan's Hurricane (Ranged Only)
                case 3084: // Overlord's Bloodmail
                case 3083: // Warmog's Armor
                case 3285: // Luden's Echo
                case 3924: // Flesheater (Melee Only)
                case 3290: // Twin Shadows
                case 3091: // Wit's End
                case 3090: // Wooglet's Witchcap
                case 3092: // Frost Queen's Claim
                case 3716: // Enchantment: Runeglaive
                case 3911: // Martyr's Gambit
                case 3742: // Dead Man's Plate
                case 3110: // Frozen Heart
                case 3112: // Orb of Winter
                case 3106: // Madred's Razors
                case 3102: // Banshee's Veil
                case 3100: // Lich Bane
                case 3800: // Righteous Glory
                case 3504: // Ardent Censer
                case 3708: // Enchantment: Runeglaive
                case 3508: // Essence Reaver
                case 3146: // Hextech Gunblade
                case 3156: // Maw of Malmortius
                case 3154: // Wriggle's Lantern
                case 3153: // Blade of the Ruined King
                case 3152: // Will of the Ancients
                case 3151: // Liandry's Torment
                case 3139: // Mercurial Scimitar
                case 3135: // Void Staff
                case 3137: // Dervish Blade
                case 3348: // Hextech Sweeper
                case 3001: // Abyssal Scepter
                case 3143: // Randuin's Omen
                case 3142: // Youmuu's Ghostblade
                case 3401: // Face of the Mountain
                case 3141: // Sword of the Occult
                case 3124: // Guinsoo's Rageblade
                case 3027: // Rod of Ages
                case 3025: // Iceborn Gauntlet
                case 3026: // Guardian Angel
                case 3512: // Zz'Rot Portal
                case 3035: // Last Whisper
                case 3031: // Infinity Edge
                case 3222: // Mikael's Crucible
                case 3115: // Nashor's Tooth
                case 3116: // Rylai's Crystal Scepter
                case 3022: // Frozen Mallet
                case 3023: // Twin Shadows
                //case 3048: // Seraph's Embrace
                case 3198: // Perfect Hex Core
                case 3050: // Zeke's Harbinger
                case 3190: // Locket of the Iron Solari
                case 3056: // Ohmwrecker
                case 3187: // Hextech Sweeper
                case 3184: // Entropy
                case 3185: // The Lightbringer
                //case 3040: // Seraph's Embrace
                case 3180: // Odyn's Veil
                case 3041: // Mejai's Soulstealer
                case 3181: // Sanguine Blade
                //case 3042: // Muramana
                //case 3043: // Muramana
                case 3046: // Phantom Dancer
                case 3069: // Talisman of Ascension
                case 3174: // Athene's Unholy Grail
                case 3071: // The Black Cleaver
                case 3172: // Zephyr
                case 3078: // Trinity Force
                case 3074: // Ravenous Hydra (Melee Only)
                case 3170: // Moonflair Spellblade
                case 3075: // Thornmail
                case 3652: // Typhoon Claws
                case 3072: // The Bloodthirster
                case 2045: // Ruby Sightstone
                case 3157: // Zhonya's Hourglass
                case 3159: // Grez's Spectral Lantern
                case 3060: // Banner of Command
                case 3165: // Morellonomicon
                case 3065: // Spirit Visage
                case 3068: // Sunfire Cape
                case 3003: // Archangel's Staff
                case 3004: // Manamune
                case 3725: // Cinderhulk
                case 3721: // Cinderhulk
                case 3717: // Cinderhulk
                case 3709: // Cinderhulk
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Gets the name of an item
        /// </summary>
        /// <param name="id">The item ID</param>
        /// <returns>The item name</returns>
        public static string ItemIdToName(int id)
        {
            switch (id)
            {
                case 3725: return "Enchantment: Cinderhulk";
                case 3724: return "Enchantment: Runeglaive";
                case 3089: return "Rabadon's Deathcap";
                case 2009: return "Total Biscuit of Rejuvenation";
                case 3723: return "Enchantment: Warrior";
                case 3722: return "Enchantment: Devourer";
                case 3087: return "Statikk Shiv";
                case 3721: return "Enchantment: Cinderhulk";
                case 3086: return "Zeal";
                case 2004: return "Mana Potion";
                case 3720: return "Enchantment: Runeglaive";
                case 3085: return "Runaan's Hurricane (Ranged Only)";
                case 1319: return "Enchantment: Homeguard";
                case 3084: return "Overlord's Bloodmail";
                case 1318: return "Enchantment: Distortion";
                case 3083: return "Warmog's Armor";
                case 2003: return "Health Potion";
                case 1317: return "Enchantment: Captain";
                case 3082: return "Warden's Mail";
                case 1316: return "Enchantment: Alacrity";
                case 1315: return "Enchantment: Furor";
                case 1314: return "Enchantment: Homeguard";
                case 1313: return "Enchantment: Distortion";
                case 1312: return "Enchantment: Captain";
                case 3285: return "Luden's Echo";
                case 1311: return "Enchantment: Alacrity";
                case 1310: return "Enchantment: Furor";
                case 3726: return "Enchantment: Devourer";
                case 3924: return "Flesheater (Melee Only)";
                case 2010: return "Total Biscuit of Rejuvenation";
                case 3711: return "Poacher's Knife";
                case 3098: return "Frostfang";
                case 3714: return "Enchantment: Warrior";
                case 3713: return "Ranger's Trailblazer";
                case 1329: return "Enchantment: Homeguard";
                case 3290: return "Twin Shadows";
                case 1328: return "Enchantment: Distortion";
                case 3710: return "Enchantment: Devourer";
                case 3097: return "Targon's Brace";
                case 3096: return "Nomad's Medallion";
                case 3091: return "Wit's End";
                case 1325: return "Enchantment: Furor";
                case 3719: return "Enchantment: Warrior";
                case 3090: return "Wooglet's Witchcap";
                case 1324: return "Enchantment: Homeguard";
                case 3093: return "Avarice Blade";
                case 1327: return "Enchantment: Captain";
                case 3092: return "Frost Queen's Claim";
                case 1326: return "Enchantment: Alacrity";
                case 3716: return "Enchantment: Runeglaive";
                case 1321: return "Enchantment: Alacrity";
                case 3715: return "Skirmisher's Sabre";
                case 1320: return "Enchantment: Furor";
                case 3718: return "Enchantment: Devourer";
                case 1323: return "Enchantment: Distortion";
                case 3717: return "Enchantment: Cinderhulk";
                case 1322: return "Enchantment: Captain";
                case 1330: return "Enchantment: Furor";
                case 3911: return "Martyr's Gambit";
                case 3742: return "Dead Man's Plate";
                case 3599: return "The Black Spear";
                case 3745: return "Puppeteer";
                case 3744: return "Staff of Flowing Water";
                case 1075: return "Doran's Blade (Showdown)";
                case 1074: return "Doran's Shield (Showdown)";
                case 3844: return "Murksphere";
                case 1076: return "Doran's Ring (Showdown)";
                case 3841: return "Swindler's Orb";
                case 3840: return "Globe of Trust";
                case 1307: return "Enchantment: Captain";
                case 1306: return "Enchantment: Alacrity";
                case 1309: return "Enchantment: Homeguard";
                case 1308: return "Enchantment: Distortion";
                case 1301: return "Enchantment: Alacrity";
                case 1300: return "Enchantment: Furor";
                case 1063: return "Prospector's Ring";
                case 1303: return "Enchantment: Distortion";
                case 1062: return "Prospector's Blade";
                case 1302: return "Enchantment: Captain";
                case 1305: return "Enchantment: Furor";
                case 1304: return "Enchantment: Homeguard";
                case 1058: return "Needlessly Large Rod";
                case 1056: return "Doran's Ring";
                case 1057: return "Negatron Cloak";
                case 3930: return "Enchantment: Sated Devourer";
                case 3931: return "Enchantment: Sated Devourer";
                case 3932: return "Enchantment: Sated Devourer";
                case 3933: return "Enchantment: Sated Devourer";
                case 3110: return "Frozen Heart";
                case 3111: return "Mercury's Treads";
                case 3240: return "Enchantment: Furor";
                case 3112: return "Orb of Winter";
                case 3241: return "Enchantment: Alacrity";
                case 3242: return "Enchantment: Captain";
                case 3243: return "Enchantment: Distortion";
                case 3244: return "Enchantment: Homeguard";
                case 3621: return "Offense Upgrade 1";
                case 3622: return "Offense Upgrade 2";
                case 3625: return "Defense Upgrade 2";
                case 3626: return "Defense Upgrade 3";
                case 3829: return "Trickster's Glass";
                case 3623: return "Offense Upgrade 3";
                case 3624: return "Defense Upgrade 1";
                case 3106: return "Madred's Razors";
                case 3108: return "Fiendish Codex";
                case 3102: return "Banshee's Veil";
                case 3105: return "Aegis of the Legion";
                case 3104: return "Lord Van Damm's Pillager";
                case 3616: return "Mercenary Upgrade 2";
                case 3100: return "Lich Bane";
                case 3617: return "Mercenary Upgrade 3";
                case 3101: return "Stinger";
                case 3611: return "Razorfin";
                case 3612: return "Ironback";
                case 3613: return "Plundercrab";
                case 3614: return "Ocklepod";
                case 3615: return "Mercenary Upgrade 1";
                case 3245: return "Enchantment: Teleport";
                case 3801: return "Crystalline Bracer";
                case 3706: return "Stalker's Blade";
                case 3707: return "Enchantment: Warrior";
                case 3800: return "Righteous Glory";
                case 3504: return "Ardent Censer";
                case 3708: return "Enchantment: Runeglaive";
                case 3709: return "Enchantment: Cinderhulk";
                case 3508: return "Essence Reaver";
                case 3361: return "Greater Stealth Totem (Trinket)";
                case 3362: return "Greater Vision Totem (Trinket)";
                case 3363: return "Farsight Orb (Trinket)";
                case 3364: return "Oracle's Lens (Trinket)";
                case 2140: return "Elixir of Wrath";
                case 2138: return "Elixir of Iron";
                case 2139: return "Elixir of Sorcery";
                case 2137: return "Elixir of Ruin";
                case 1004: return "Faerie Charm";
                case 1001: return "Boots of Speed";
                case 3146: return "Hextech Gunblade";
                case 1006: return "Rejuvenation Bead";
                case 3006: return "Berserker's Greaves";
                case 3003: return "Archangel's Staff";
                case 3004: return "Manamune";
                case 3009: return "Boots of Swiftness";
                case 3007: return "Archangel's Staff (Crystal Scar)";
                case 3008: return "Manamune (Crystal Scar)";
                case 3342: return "Scrying Orb (Trinket)";
                case 3341: return "Sweeping Lens (Trinket)";
                case 3340: return "Warding Totem (Trinket)";
                case 3010: return "Catalyst the Protector";
                case 3156: return "Maw of Malmortius";
                case 3155: return "Hexdrinker";
                case 3154: return "Wriggle's Lantern";
                case 3153: return "Blade of the Ruined King";
                case 3200: return "Prototype Hex Core";
                case 3152: return "Will of the Ancients";
                case 1011: return "Giant's Belt";
                case 3151: return "Liandry's Torment";
                case 3150: return "Mirage Blade";
                case 3139: return "Mercurial Scimitar";
                case 3135: return "Void Staff";
                case 3136: return "Haunting Guise";
                case 3137: return "Dervish Blade";
                case 3348: return "Hextech Sweeper";
                case 3345: return "Soul Anchor (Trinket)";
                case 3001: return "Abyssal Scepter";
                case 3143: return "Randuin's Omen";
                case 3142: return "Youmuu's Ghostblade";
                case 3145: return "Hextech Revolver";
                case 3401: return "Face of the Mountain";
                case 3144: return "Bilgewater Cutlass";
                case 3211: return "Spectre's Cowl";
                case 3141: return "Sword of the Occult";
                case 3140: return "Quicksilver Sash";
                case 3124: return "Guinsoo's Rageblade";
                case 3029: return "Rod of Ages (Crystal Scar)";
                case 3027: return "Rod of Ages";
                case 3028: return "Chalice of Harmony";
                case 3025: return "Iceborn Gauntlet";
                case 3026: return "Guardian Angel";
                case 3512: return "Zz'Rot Portal";
                case 3035: return "Last Whisper";
                case 3031: return "Infinity Edge";
                case 3222: return "Mikael's Crucible";
                case 3134: return "The Brutalizer";
                case 3113: return "Aether Wisp";
                case 3114: return "Forbidden Idol";
                case 3430: return "Rite of Ruin";
                case 3115: return "Nashor's Tooth";
                case 3431: return "Netherstride Grimoire";
                case 3116: return "Rylai's Crystal Scepter";
                case 3117: return "Boots of Mobility";
                case 3022: return "Frozen Mallet";
                case 3024: return "Glacial Shroud";
                case 3023: return "Twin Shadows";
                case 3020: return "Sorcerer's Shoes";
                case 3122: return "Wicked Hatchet";
                case 2053: return "Raptor Cloak";
                case 2054: return "Diet Poro-Snax";
                case 3048: return "Seraph's Embrace";
                case 3047: return "Ninja Tabi";
                case 2050: return "Explorer's Ward";
                case 2051: return "Guardian's Horn";
                case 2052: return "Poro-Snax";
                case 3434: return "Pox Arcana";
                case 1051: return "Brawler's Gloves";
                case 3197: return "The Hex Core mk-2";
                case 3433: return "Lost Chapter";
                case 3198: return "Perfect Hex Core";
                case 1054: return "Doran's Shield";
                case 3196: return "The Hex Core mk-1";
                case 1055: return "Doran's Blade";
                case 1052: return "Amplifying Tome";
                case 1053: return "Vampiric Scepter";
                case 3191: return "Seeker's Armguard";
                case 3050: return "Zeke's Harbinger";
                case 3190: return "Locket of the Iron Solari";
                case 3056: return "Ohmwrecker";
                case 2047: return "Oracle's Extract";
                case 3057: return "Sheen";
                case 2049: return "Sightstone";
                case 1341: return "Enchantment: Teleport";
                case 1340: return "Enchantment: Teleport";
                case 3301: return "Ancient Coin";
                case 3303: return "Spellthief's Edge";
                case 3302: return "Relic Shield";
                case 1037: return "Pickaxe";
                case 1036: return "Long Sword";
                case 1039: return "Hunter's Machete";
                case 1038: return "B. F. Sword";
                case 3187: return "Hextech Sweeper";
                case 1339: return "Enchantment: Teleport";
                case 1042: return "Dagger";
                case 3184: return "Entropy";
                case 1043: return "Recurve Bow";
                case 3185: return "The Lightbringer";
                case 1335: return "Enchantment: Teleport";
                case 3040: return "Seraph's Embrace";
                case 1336: return "Enchantment: Teleport";
                case 3180: return "Odyn's Veil";
                case 3041: return "Mejai's Soulstealer";
                case 1337: return "Enchantment: Teleport";
                case 3181: return "Sanguine Blade";
                case 3042: return "Muramana";
                case 1338: return "Enchantment: Teleport";
                case 3043: return "Muramana";
                case 1331: return "Enchantment: Alacrity";
                case 3044: return "Phage";
                case 1332: return "Enchantment: Captain";
                case 1333: return "Enchantment: Distortion";
                case 3046: return "Phantom Dancer";
                case 1334: return "Enchantment: Homeguard";
                case 3901: return "Fire at Will";
                case 3069: return "Talisman of Ascension";
                case 3903: return "Raise Morale";
                case 3902: return "Death's Daughter";
                case 1029: return "Cloth Armor";
                case 1028: return "Ruby Crystal";
                case 1027: return "Sapphire Crystal";
                case 3460: return "Golden Transcendence";
                case 1026: return "Blasting Wand";
                case 3070: return "Tear of the Goddess";
                case 3174: return "Athene's Unholy Grail";
                case 3071: return "The Black Cleaver";
                case 1033: return "Null-Magic Mantle";
                case 3751: return "Bami's Cinder";
                case 3172: return "Zephyr";
                case 1031: return "Chain Vest";
                case 3078: return "Trinity Force";
                case 3077: return "Tiamat (Melee Only)";
                case 3074: return "Ravenous Hydra (Melee Only)";
                case 3170: return "Moonflair Spellblade";
                case 3075: return "Thornmail";
                case 3652: return "Typhoon Claws";
                case 3072: return "The Bloodthirster";
                case 3073: return "Tear of the Goddess (Crystal Scar)";
                case 2041: return "Crystalline Flask";
                case 2044: return "Stealth Ward";
                case 2045: return "Ruby Sightstone";
                case 2043: return "Vision Ward";
                case 3158: return "Ionian Boots of Lucidity";
                case 3157: return "Zhonya's Hourglass";
                case 3159: return "Grez's Spectral Lantern";
                case 1018: return "Cloak of Agility";
                case 3060: return "Banner of Command";
                case 3165: return "Morellonomicon";
                case 3065: return "Spirit Visage";
                case 3067: return "Kindlegem";
                case 3068: return "Sunfire Cape";
                default:
                    Debug.Assert(false);
                    return "Unknown";
            }
        }
    }
}
