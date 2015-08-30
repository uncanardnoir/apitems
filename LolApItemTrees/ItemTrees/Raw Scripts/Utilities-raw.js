﻿// Convert an integer to a string, but ensure that it has at least 2 digits (2 -> "02")
function EnsureLeadingZero(i) {
    return ("0" + i).slice(-2);
}

// Convert a dictionary (where values unsorted) into a sorted array
function DictionaryToSortedArray(obj) {
    var tuples = [];
    for (var key in obj) tuples.push([key, obj[key]]);
    tuples.sort(function (a, b) { return a[1] < b[1] ? 1 : a[1] > b[1] ? -1 : 0 });
    return tuples;
}

// Get hash parameters, as an array
function getHashParameters() {
    var result = {};
    if (window.location.hash) {
        window.location.hash
        .substr(1)
            .split("&")
            .forEach(function (item) {
                tmp = item.split("=");
                result[tmp[0]] = tmp[1];
            });
    }
    return result;
}

// Set hash parameters, using certain global variables
function setHashParameters() {
    var hash = "leftId=" + currentLeftChamp + "&leftPatch=" + currentLeftPatch + "&rightId=" + currentRightChamp + "&rightPatch=" + currentRightPatch;
    window.location.hash = hash;
}

// Calculate the similarity score of 2 dictionaries
function calculateSimilarityScore(dict1, dict2) {
    // Calculate vector norms
    var ss1 = 0.0;
    for (var i in dict1) {
        ss1 += dict1[i] * dict1[i];
    }
    ss1 = Math.sqrt(ss1);
    var ss2 = 0.0;
    for (var i in dict2) {
        ss2 += dict2[i] * dict2[i];
    }
    ss2 = Math.sqrt(ss2);
    // calculate dot product
    var dp = 0.0;
    for (var i in dict1) {
        if (i in dict2) {
            dp += (dict1[i] / ss1) * (dict2[i] / ss2);
        }
    }
    return 100 - Math.acos(dp) * 100 / Math.PI;
}

// Convert champ ID to string
function champIdToName(id) {
    switch (id) {
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
        default: return "Unknown";
    }
}

// Convert item ID to string
function itemIdToName(id) {
    switch (id)
    {
        case 3725: return "Enchantment: Cinderhulk";
        case 3724: return "Magus (5.11)/Runeglaive (5.14)";
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
        case 3716: return "Enchantment: Magus";
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
        case 3708: return "Magus (5.11)/Runeglaive (5.14)";
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
        default: return "Unknown";
    }
}

// Get a champion's title
function getChampTitle(id) {
    switch (id) {
        case 266: return "The Darkin Blade";
        case 412: return "The Chain Warden";
        case 23: return "The Barbarian King";
        case 79: return "The Rabble Rouser";
        case 69: return "The Serpent's Embrace";
        case 13: return "The Rogue Mage";
        case 78: return "The Iron Ambassador";
        case 14: return "The Undead Juggernaut";
        case 1: return "The Dark Child";
        case 111: return "The Titan of the Depths";
        case 43: return "The Enlightened One";
        case 99: return "The Lady of Luminosity";
        case 103: return "The Nine-Tailed Fox";
        case 2: return "The Berserker";
        case 112: return "The Machine Herald";
        case 34: return "The Cryophoenix";
        case 86: return "The Might of Demacia";
        case 27: return "The Mad Chemist";
        case 127: return "The Ice Witch";
        case 57: return "The Twisted Treant";
        case 25: return "Fallen Angel";
        case 28: return "The Widowmaker";
        case 105: return "The Tidal Trickster";
        case 74: return "The Revered Inventor";
        case 238: return "The Master of Shadows";
        case 68: return "The Mechanized Menace";
        case 37: return "Maven of the Strings";
        case 82: return "The Master of Metal";
        case 96: return "The Mouth of the Abyss";
        case 55: return "The Sinister Blade";
        case 117: return "The Fae Sorceress";
        case 22: return "The Frost Archer";
        case 30: return "The Deathsinger";
        case 12: return "The Minotaur";
        case 122: return "The Hand of Noxus";
        case 67: return "The Night Hunter";
        case 77: return "The Spirit Walker";
        case 110: return "The Arrow of Retribution";
        case 89: return "The Radiant Dawn";
        case 126: return "The Defender of Tomorrow";
        case 134: return "The Dark Sovereign";
        case 80: return "The Artisan of War";
        case 92: return "The Exile";
        case 121: return "The Voidreaver";
        case 42: return "The Daring Bombardier";
        case 51: return "The Sheriff of Piltover";
        case 268: return "The Emperor of the Sands";
        case 76: return "The Bestial Huntress";
        case 3: return "The Sentinel's Sorrow";
        case 85: return "The Heart of the Tempest";
        case 45: return "The Tiny Master of Evil";
        case 432: return "The Wandering Caretaker";
        case 150: return "The Missing Link";
        case 104: return "The Outlaw";
        case 90: return "The Prophet of the Void";
        case 254: return "The Piltover Enforcer";
        case 10: return "The Judicator";
        case 39: return "The Will of the Blades";
        case 64: return "The Blind Monk";
        case 60: return "The Spider Queen";
        case 106: return "The Thunder's Roar";
        case 20: return "The Yeti Rider";
        case 4: return "The Card Master";
        case 24: return "Grandmaster at Arms";
        case 102: return "The Half-Dragon";
        case 429: return "The Spear of Vengeance";
        case 36: return "The Madman of Zaun";
        case 223: return "The River King";
        case 63: return "The Burning Vengeance";
        case 131: return "Scorn of the Moon";
        case 113: return "The Winter's Wrath";
        case 8: return "The Crimson Reaper";
        case 154: return "The Secret Weapon";
        case 421: return "The Void Burrower";
        case 133: return "Demacia's Wings";
        case 84: return "The Fist of Shadow";
        case 18: return "The Yordle Gunner";
        case 120: return "The Shadow of War";
        case 15: return "The Battle Mistress";
        case 236: return "The Purifier";
        case 107: return "The Pridestalker";
        case 19: return "The Blood Hunter";
        case 72: return "The Crystal Vanguard";
        case 54: return "Shard of the Monolith";
        case 157: return "The Unforgiven";
        case 101: return "The Magus Ascendant";
        case 17: return "The Swift Scout";
        case 75: return "The Curator of the Sands";
        case 58: return "The Butcher of the Sands";
        case 119: return "The Glorious Executioner";
        case 35: return "The Demon Jester";
        case 50: return "The Master Tactician";
        case 115: return "The Hexplosives Expert";
        case 91: return "The Blade's Shadow";
        case 40: return "The Storm's Fury";
        case 245: return "The Boy Who Shattered Time";
        case 61: return "The Lady of Clockwork";
        case 114: return "The Grand Duelist";
        case 9: return "The Harbinger of Doom";
        case 33: return "The Armordillo";
        case 31: return "The Terror of the Void";
        case 7: return "The Deceiver";
        case 16: return "The Starchild";
        case 26: return "The Chronokeeper";
        case 56: return "The Eternal Nightmare";
        case 222: return "The Loose Cannon";
        case 83: return "The Gravedigger";
        case 6: return "The Headsman's Pride";
        case 21: return "The Bounty Hunter";
        case 62: return "The Monkey King";
        case 53: return "The Great Steam Golem";
        case 98: return "Eye of Twilight";
        case 201: return "The Heart of the Freljord";
        case 5: return "The Seneschal of Demacia";
        case 29: return "The Plague Rat";
        case 11: return "The Wuju Bladesman";
        case 44: return "The Gem Knight";
        case 32: return "The Sad Mummy";
        case 41: return "The Saltwater Scourge";
        case 48: return "The Troll King";
        case 38: return "The Void Walker";
        case 161: return "The Eye of the Void";
        case 143: return "Rise of the Thorns";
        case 267: return "The Tidecaller";
        case 59: return "The Exemplar of Demacia";
        case 81: return "The Prodigal Explorer";
        default: return "";
    }
}

// Get the HTML for an item's tooltip
function tooltipItem(id) {
    var innerHtml = '<a href="';

    switch (id) {
        case 3720:
            innerHtml += "http://na.leagueoflegends.com/en/news/game-updates/patch/patch-512-notes#patch-Enchantment</div></div></div>Runeglaive";
            break;
        case 3116:
            innerHtml += "http://na.leagueoflegends.com/en/news/game-updates/patch/patch-513-notes#patch-Rylais-Crystal-Scepter";
            break;
        case 3115:
            innerHtml += "http://na.leagueoflegends.com/en/news/game-updates/patch/patch-513-notes#patch-Nashors-Tooth";
            break;
        case 3285:
            innerHtml += "http://na.leagueoflegends.com/en/news/game-updates/patch/patch-513-notes#patch-Ludens-Echo";
            break;
        case 3089:
            innerHtml += "http://na.leagueoflegends.com/en/news/game-updates/patch/patch-513-notes#patch-Rabadons-Deathcap";
            break;
        case 3165:
            innerHtml += "http://na.leagueoflegends.com/en/news/game-updates/patch/patch-513-notes#patch-Morellonomicon";
            break;
        case 3157:
            innerHtml += "http://na.leagueoflegends.com/en/news/game-updates/patch/patch-513-notes#patch-Zhonyas-Hourglass";
            break;
        case 3151:
            innerHtml += "http://na.leagueoflegends.com/en/news/game-updates/patch/patch-513-notes#patch-Liandrys-Torment";
            break;
        case 3027:
            innerHtml += "http://na.leagueoflegends.com/en/news/game-updates/patch/patch-513-notes#patch-Rod-of-Ages";
            break;
        case 3003:
            innerHtml += "http://na.leagueoflegends.com/en/news/game-updates/patch/patch-513-notes#patch-Archangels-Staff";
            break;
        default:
            innerHtml += "#";
    }

    innerHtml += '" class="item-tooltip" rel="tooltip" data-html="true" title="';

    switch (id) {
        case 3720:
            innerHtml += "<div><span class=&quot;attribute-tooltip&quot;>RECIPE</span>:<span class=&quot;attribute-after-tooltip&quot;>Tier 2 Jungle Item + Sheen + 200 gold (2250 gold total)</span></div><div><span class=&quot;attribute-tooltip&quot;>ABILITY POWER</span>:<span class=&quot;attribute-after-tooltip&quot;>40</span></div><div><span class=&quot;attribute-tooltip&quot;>COOLDOWN REDUCTION</span>:<span class=&quot;attribute-after-tooltip&quot;>10%</span></div><div><span class=&quot;attribute-tooltip&quot;>MANA</span>:<span class=&quot;attribute-after-tooltip&quot;>200</span></div><div><span class=&quot;attribute-tooltip&quot;>UNIQUE PASSIVE</span>:<span class=&quot;attribute-after-tooltip&quot;>Spellblade - After using an ability, the next basic attack is converted into magic damage and deals 75% Base Attack Damage (+0.3 ability power) bonus damage on hit in an Area-of-Effect around the target and <strong>restores 8% of your missing Mana</strong></span></div>";
            break;
        case 3716:
            innerHtml += "<div><span class=&quot;attribute-tooltip&quot;>RECIPE</span>:<span class=&quot;attribute-after-tooltip&quot;>Fiendish Codex + any upgraded Hunter's Machete + 580 gold</span></div><div><span class=&quot;attribute-tooltip&quot;>TOTAL COST</span>:<span class=&quot;attribute-after-tooltip&quot;>2250 gold</span></div><div><span class=&quot;attribute-tooltip&quot;>ABILITY POWER</span>:<span class=&quot;attribute-after-tooltip&quot;>80</span></div><div><span class=&quot;attribute-tooltip&quot;>COOLDOWN REDUCTION</span>:<span class=&quot;attribute-after-tooltip&quot;>20%</span></div>";
            break;
        case 3115:
            innerHtml += "<div><span class=&quot;attribute-tooltip&quot;>COST</span>:<span class=&quot;attribute-before-tooltip&quot;>2920 gold</span><span>⇒</span><span class=&quot;attribute-after-tooltip&quot;>3000 gold</span></div><div><span class=&quot;attribute-tooltip&quot;>ATTACK SPEED</span>:<span class=&quot;attribute-before-tooltip&quot;>50%</span><span>⇒</span><span class=&quot;attribute-after-tooltip&quot;>40%</span></div><div><span class=&quot;attribute-tooltip&quot;>ABILITY POWER</span>:<span class=&quot;attribute-before-tooltip&quot;>60</span><span>⇒</span><span class=&quot;attribute-after-tooltip&quot;>80</span></div>";
            break;
        case 3116:
            innerHtml += "<div><span class=&quot;attribute-tooltip&quot;>COST</span>:<span class=&quot;attribute-before-tooltip&quot;>2900 gold</span><span>⇒</span><span class=&quot;attribute-after-tooltip&quot;>3000 gold</span></div><div><span class=&quot;attribute-tooltip&quot;>ABILITY POWER</span>:<span class=&quot;attribute-before-tooltip&quot;>100</span><span>⇒</span><span class=&quot;attribute-after-tooltip&quot;>100 (unchanged)</span></div><div><span class=&quot;attribute-tooltip&quot;>HEALTH</span>:<span class=&quot;attribute-before-tooltip&quot;>400</span><span>⇒</span><span class=&quot;attribute-after-tooltip&quot;>400 (unchanged)</span></div><div><span class=&quot;attribute-tooltip&quot;>RECIPE CHANGE</span>:<span class=&quot;attribute-before-tooltip&quot;>Giant's Belt + Blasting Wand + Amplifying Tome + 605 gold</span><span>⇒</span><span class=&quot;attribute-after-tooltip&quot;>Giant's Belt + Needlessly Large Rod + Amplifying Tome + 315 gold</span></div><div><span class=&quot;attribute-tooltip&quot;>SINGLE TARGET SLOW</span><span class=&quot;attribute-before-tooltip&quot;>35%</span><span>⇒</span><span class=&quot;attribute-after-tooltip&quot;>40%</span></div><div><span class=&quot;attribute-tooltip&quot;>AREA OF EFFECT SLOW (INSTANT)</span>:<span class=&quot;attribute-before-tooltip&quot;>15%</span><span>⇒</span><span class=&quot;attribute-after-tooltip&quot;>40%</span></div><div><span class=&quot;attribute-tooltip&quot;>DAMAGE OVER TIME OR MULTI-HIT SLOW</span>:<span class=&quot;attribute-before-tooltip&quot;>15% for 1.5 seconds</span><span>⇒</span><span class=&quot;attribute-after-tooltip&quot;>20% for 1 second</span></div><div><span class=&quot;attribute-tooltip&quot;><span class=&quot;attribute-new&quot;>new</span>I CHOOSE YOU</span>:<span class=&quot;attribute-after-tooltip&quot;>Summoned minions (e.g. Mordekaiser's <strong>R - Children of the Grave</strong>, Annie's <strong>R - Summon: Tibbers</strong>) now slow on-hit 20% for 1 second</span></div><div><span class=&quot;attribute-tooltip&quot;><span class=&quot;attribute-new&quot;>new</span>CRYSTAL CLEAR</span>:<span class=&quot;attribute-after-tooltip&quot;>New particle added</span></div>";
            break;
        case 3285:
            innerHtml += "<div><span class=&quot;attribute-tooltip&quot;>COST</span>:<span class=&quot;attribute-before-tooltip&quot;>3300 gold</span><span>⇒</span><span class=&quot;attribute-after-tooltip&quot;>3000 gold</span></div><div><span class=&quot;attribute-tooltip&quot;>ABILITY POWER</span>:<span class=&quot;attribute-before-tooltip&quot;>120</span><span>⇒</span><span class=&quot;attribute-after-tooltip&quot;>100</span></div><div><span class=&quot;attribute-tooltip&quot;>MOVEMENT SPEED</span>:<span class=&quot;attribute-before-tooltip&quot;>7%</span><span>⇒</span><span class=&quot;attribute-after-tooltip&quot;>10%</span></div>";
            break;
        case 3089:
            innerHtml += "<div><span class=&quot;attribute-tooltip&quot;>TOTAL COST</span>:<span class=&quot;attribute-before-tooltip&quot;>3300 gold</span><span>⇒</span><span class=&quot;attribute-after-tooltip&quot;>3500 gold</span></div><div><span class=&quot;attribute-tooltip&quot;>RECIPE</span>:<span class=&quot;attribute-before-tooltip&quot;>Needlessly Large Rod + Blasting Wand + 840 gold</span><span>⇒</span><span class=&quot;attribute-after-tooltip&quot;>Needlessly Large Rod + Blasting Wand + Amplifying Tome + 935 gold</span></div><div><span class=&quot;attribute-tooltip&quot;>ABILITY POWER</span>:<span class=&quot;attribute-before-tooltip&quot;>120</span><span>⇒</span><span class=&quot;attribute-after-tooltip&quot;>still 120</span></div><div><span class=&quot;attribute-tooltip&quot;>ABILITY POWER BONUS</span><span class=&quot;attribute-before-tooltip&quot;>30%</span><span>⇒</span><span class=&quot;attribute-after-tooltip&quot;>35%</span></div>";
            break;
        case 3165:
            innerHtml += "<div><span class=&quot;attribute-tooltip&quot;>TOTAL COST</span>:<span class=&quot;attribute-after-tooltip&quot;>Unchanged</span></div><div><span class=&quot;attribute-tooltip&quot;>RECIPE</span>:<span class=&quot;attribute-before-tooltip&quot;>Fiendish Codex + Forbidden Idol + 880 gold</span><span>⇒</span><span class=&quot;attribute-after-tooltip&quot;>Fiendish Codex + Forbidden Idol + Amplifying Tome + 445 gold</span></div>";
            break;
        case 3157:
            innerHtml += "<div><span class=&quot;attribute-tooltip&quot;>COST</span>:<span class=&quot;attribute-before-tooltip&quot;>3300 gold</span><span>⇒</span><span class=&quot;attribute-after-tooltip&quot;>3000 gold</span></div><div><span class=&quot;attribute-tooltip&quot;>ABILITY POWER</span>:<span class=&quot;attribute-before-tooltip&quot;>120</span><span>⇒</span><span class=&quot;attribute-after-tooltip&quot;>100</span></div>";
            break;
        case 3151:
            innerHtml += "<div><span class=&quot;attribute-tooltip&quot;>TOTAL COST</span><span class=&quot;attribute-before-tooltip&quot;>2900 gold</span><span>⇒</span><span class=&quot;attribute-after-tooltip&quot;>3000 gold</span></div><div><span class=&quot;attribute-tooltip&quot;>RECIPE</span><span class=&quot;attribute-before-tooltip&quot;>Haunting Guise + Amplifying Tome + 980 gold</span><span>⇒</span><span class=&quot;attribute-after-tooltip&quot;>Haunting Guise + Blasting Wand + 650 gold</span></div><div><span class=&quot;attribute-tooltip&quot;>ABILITY POWER</span><span class=&quot;attribute-before-tooltip&quot;>50</span><span>⇒</span><span class=&quot;attribute-after-tooltip&quot;>80</span></div>";
            break;
        case 3027:
            innerHtml += "<div><span class=&quot;attribute-tooltip&quot;>COST</span>:<span class=&quot;attribute-before-tooltip&quot;>2800 gold</span><span>⇒</span><span class=&quot;attribute-after-tooltip&quot;>2700 gold</span></div><div><span class=&quot;attribute-tooltip&quot;>BASE HEALTH</span>:<span class=&quot;attribute-before-tooltip&quot;>450</span><span>⇒</span><span class=&quot;attribute-after-tooltip&quot;>300</span></div><div><span class=&quot;attribute-tooltip&quot;>BASE MANA</span>:<span class=&quot;attribute-before-tooltip&quot;>450</span><span>⇒</span><span class=&quot;attribute-after-tooltip&quot;>400</span></div><div><span class=&quot;attribute-tooltip&quot;>HEALTH GROWTH</span>:<span class=&quot;attribute-after-tooltip&quot;>Unchanged</span></div><div><span class=&quot;attribute-tooltip&quot;>MANA GROWTH</span>:<span class=&quot;attribute-before-tooltip&quot;>20 per minute (200 mana after 10 minutes)</span><span>⇒</span><span class=&quot;attribute-after-tooltip&quot;>40 per minute (400 mana after 10 minutes)</span></div><div><span class=&quot;attribute-tooltip&quot;>ABILITY POWER GROWTH</span>:<span class=&quot;attribute-before-tooltip&quot;>2 per minute (20 ability power after 10 minutes)</span><span>⇒</span><span class=&quot;attribute-after-tooltip&quot;>4 per minute (40 ability power after 10 minutes)</span></div>";
            break;
        case 3003:
            innerHtml += "<div><span class=&quot;attribute-tooltip&quot;>TOTAL COST</span>:<span class=&quot;attribute-before-tooltip&quot;>2700 gold</span><span>⇒</span><span class=&quot;attribute-after-tooltip&quot;>3000 gold</span></div><div><span class=&quot;attribute-tooltip&quot;>RECIPE CHANGE</span>:<span class=&quot;attribute-before-tooltip&quot;>Tear of the Goddess + Blasting Wand + 1120 gold</span><span>⇒</span><span class=&quot;attribute-after-tooltip&quot;>Tear of the Goddess + Needlessly Large Rod + 1030 gold</span></div><div><span class=&quot;attribute-tooltip&quot;>ABILITY POWER</span>:<span class=&quot;attribute-before-tooltip&quot;>60</span><span>⇒</span><span class=&quot;attribute-after-tooltip&quot;>80</span></div>";
            break;
    }

    innerHtml += '"><img src="Icons/';
    innerHtml += id;
    innerHtml += '.png" style="width:25px;height:25px;"/>';
    innerHtml += itemIdToName(id);
    innerHtml += '</a>';
    return innerHtml;
}

// Get the HTML for a champion tooltip
function champHtml(id) {
    return '<img src="Icons/c' + id + '.png" style="width:25px;height:25px;"/>' + champIdToName(id);
}

// Get the HTML for an item tree link
function itemTree(leftId, leftPatch, rightId, rightPatch) {
    var innerHtml = '<a href="ItemTrees#leftId=' + leftId + '&leftPatch=' + leftPatch + '&rightId=' + rightId + '&rightPatch=' + rightPatch + '">Item Trees: ';
    innerHtml += champHtml(leftId);
    innerHtml += ' (' + leftPatch + ') vs ';
    innerHtml += champHtml(rightId);
    innerHtml += ' (' + rightPatch + ')</a>';
    return innerHtml;
}