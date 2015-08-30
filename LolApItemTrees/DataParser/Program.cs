using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace DataParser
{
    /// <summary>
    /// This class parses all of the downloaded games and summarizes the champion statistics/build paths in a usable format.
    /// This is intended for one-off personal use and is not refactored for general use.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Intermediate representation of a purchased item.
        /// </summary>
        class ItemPurchased
        {
            /// <summary>
            /// Item id
            /// </summary>
            public int itemId;

            /// <summary>
            /// Build time in seconds
            /// </summary>
            public int itemTime;

            public override string ToString() {
                return string.Format("{0} @ {1}s", Utilities.ItemIdToName(itemId), itemTime);
            }
        }

        /// <summary>
        /// A champion in a single game
        /// </summary>
        class ChampionOverview
        {
            /// <summary>
            /// Champion ID
            /// </summary>
            public int ChampionId;

            /// <summary>
            /// Champion Lane
            /// </summary>
            public string Lane;

            /// <summary>
            /// Champion Role
            /// </summary>
            public string Role;

            /// <summary>
            /// All final items purchased, in order
            /// </summary>
            public List<ItemPurchased> itemsPurchased;

            /// <summary>
            /// Whether or not this champion won the game
            /// </summary>
            public bool WonGame;

            public override string ToString()
            {
                return Utilities.ChampionIdToName(ChampionId);
            }
        }

        /// <summary>
        /// A node in the item build tree
        /// </summary>
        class ItemNode
        {
            /// <summary>
            /// The node's parents
            /// </summary>
            public ItemNode parent;

            /// <summary>
            /// The node's children
            /// </summary>
            public Dictionary<int, ItemNode> children;

            /// <summary>
            /// The total number of wins for this node
            /// </summary>
            public int totalWins;

            /// <summary>
            /// The total number of times this node is hit
            /// </summary>
            public int totalOccurrences;

            /// <summary>
            /// The item id associated with this node
            /// </summary>
            public int itemId;

            /// <summary>
            /// The build time, summed over all times this node has been seen
            /// </summary>
            public long totalBuildTime;

            /// <summary>
            /// When each build of this node has completed, in thirty-second buckets
            /// </summary>
            public Dictionary<int, int> thirtySecondIntervals;

            /// <summary>
            /// For the pruning algorithm -- whether or not this node has been added to the tree yet
            /// </summary>
            public bool IsSelfPenalized;

            public override string ToString()
            {
                return Utilities.ItemIdToName(itemId);
            }

            /// <summary>
            /// Gets the sum of this node's and all children's nodes (recursively) total occurrences (the descendent score)
            /// </summary>
            /// <returns>The descendent score</returns>
            public long GetDescendentScore()
            {
                long curScore = totalOccurrences;
                foreach (ItemNode child in children.Values)
                {
                    curScore += child.GetDescendentScore();
                }
                return curScore;
            }

            /// <summary>
            /// Constructs a new item node
            /// </summary>
            /// <param name="itemId">Thie node's item ID</param>
            /// <param name="parent">The node's parent</param>
            public ItemNode(int itemId, ItemNode parent)
            {
                this.parent = parent;
                children = new Dictionary<int, ItemNode>();
                totalOccurrences = 0;
                this.itemId = itemId;
                thirtySecondIntervals = new Dictionary<int, int>();
                IsSelfPenalized = false;
            }

            /// <summary>
            /// Adds or updates a child to this node with the specified item ID
            /// </summary>
            /// <param name="itemId">The child's item id</param>
            /// <param name="timeInSec">The child's build time, in seconds</param>
            /// <param name="wonGame">Whether or not the child node won the game</param>
            /// <returns></returns>
            public ItemNode AddChild(int itemId, int timeInSec, bool wonGame) {
                ItemNode child;
                if (children.ContainsKey(itemId))
                {
                    child = children[itemId];
                    if (children[itemId].parent != this)
                    {
                        Debug.Assert(children[itemId].parent == this);
                    }
                }
                else
                {
                    child = new ItemNode(itemId, this);
                    children[itemId] = child;
                    if (children[itemId].parent != this)
                    {
                        Debug.Assert(children[itemId].parent == this);
                    }
                }

                child.totalOccurrences++;
                if (wonGame)
                {
                    child.totalWins++;
                }
                int thirtySecondBucket = timeInSec / 30;
                child.totalBuildTime += timeInSec;
                child.thirtySecondIntervals.GetOrAddDefault(thirtySecondBucket);
                child.thirtySecondIntervals[thirtySecondBucket]++;
                return child;
            }

            /// <summary>
            /// Recursively merges this node (and all children) with another node
            /// </summary>
            /// <param name="other">The node to merge with</param>
            public void RecursiveMerge(ItemNode other)
            {
                Debug.Assert(itemId == other.itemId);
                totalWins += other.totalWins;
                totalOccurrences += other.totalOccurrences;
                totalBuildTime += other.totalBuildTime;
                foreach (var kvp in other.thirtySecondIntervals)
                {
                    thirtySecondIntervals.GetOrAddDefault(kvp.Key);
                    thirtySecondIntervals[kvp.Key] += kvp.Value;
                }
                foreach (var kvp in other.children)
                {
                    if (children.ContainsKey(kvp.Key))
                    {
                        children[kvp.Key].RecursiveMerge(kvp.Value);
                    }
                    else
                    {
                        children[kvp.Key] = kvp.Value;
                        children[kvp.Key].parent = this;
                    }
                }
            }
        }

        /// <summary>
        /// The aggregate item builds for this champion.
        /// </summary>
        class ChampionItemBuilds
        {
            /// <summary>
            /// The root node in the item build tree
            /// </summary>
            public ItemNode RootObjectNode;

            /// <summary>
            /// A mapping of itemId to total number of builds
            /// </summary>
            public Dictionary<int, int> totalItemsBuilt;

            /// <summary>
            /// The total number of data points parsed for this champion
            /// </summary>
            public int DataPointsParsed;

            /// <summary>
            /// Constructs a new instance of ChampionItemBuilds
            /// </summary>
            public ChampionItemBuilds()
            {
                RootObjectNode = new ItemNode(0, null);
                totalItemsBuilt = new Dictionary<int, int>();
            }

            /// <summary>
            /// Merges two ChampionItemBuilds together
            /// </summary>
            /// <param name="toMerge">The ChampionItemBuild to merge with</param>
            public void MergeNewItemBuild(ChampionItemBuilds toMerge)
            {
                DataPointsParsed += toMerge.DataPointsParsed;
                foreach (var kvp in toMerge.totalItemsBuilt)
                {
                    totalItemsBuilt.GetOrAddDefault(kvp.Key);
                    totalItemsBuilt[kvp.Key] += kvp.Value;
                }

                RootObjectNode.RecursiveMerge(toMerge.RootObjectNode);
            }

            /// <summary>
            /// Determines whether or not this item already exist in the given ItemNode's path (detects duplicate items in a build path)
            /// </summary>
            /// <param name="leafNode">The leaf node of the item path to check</param>
            /// <param name="itemId">The item ID to check for existence</param>
            /// <returns>Whether or not the item exists</returns>
            private bool ItemAlreadyInPath(ItemNode leafNode, int itemId)
            {
                while (leafNode != RootObjectNode)
                {
                    if (leafNode.itemId == itemId)
                    {
                        return true;
                    }
                    leafNode = leafNode.parent;
                }
                return false;
            }

            /// <summary>
            /// Adds a given item path to this ChampionItemBuilds object
            /// </summary>
            /// <param name="itemPath">The item path to add</param>
            /// <param name="WonGame">Whether or not this item path resulted in a win</param>
            public void AddItemPath(List<ItemPurchased> itemPath, bool WonGame)
            {
                ItemNode thisNode = RootObjectNode;
                foreach (ItemPurchased itemPurchased in itemPath)
                {
                    if (ItemAlreadyInPath(thisNode, itemPurchased.itemId))
                    {
                        continue;
                    }

                    thisNode = thisNode.AddChild(itemPurchased.itemId, itemPurchased.itemTime, WonGame);
                    totalItemsBuilt.GetOrAddDefault(itemPurchased.itemId);
                    totalItemsBuilt[itemPurchased.itemId]++;
                }
            }

            /// <summary>
            /// Recursively resets the isSelfPenalized flag (for pruning the tree) in all nodes of the tree
            /// </summary>
            /// <param name="root">The root item node</param>
            private void ResetAllPenalizeValues(ItemNode root) {
                root.IsSelfPenalized = false;
                foreach (ItemNode child in root.children.Values)
                {
                    ResetAllPenalizeValues(child);
                }
            }

            /// <summary>
            /// The pruning algorithm -- returns a JsonItemNode containing the top N build paths
            /// </summary>
            /// <param name="n">The number of build paths to return</param>
            /// <returns>The JsonItemNode object</returns>
            public JsonItemNode GetTopNItemPaths(int n)
            {
                JsonItemNode outputRoot = new JsonItemNode{children = new List<JsonItemNode>(), itemId = 0, numberOfBuilds = 0, numberOfWins = 0, thirtySecondIntervals = null};

                // This is a mapping of item nodes to their descendent score -- this will be updated for each node as them and their children get added to the tree
                Dictionary<ItemNode, long> dynamicCounts = new Dictionary<ItemNode, long>();
                ResetAllPenalizeValues(RootObjectNode);

                while (n > 0)
                {
                    JsonItemNode currentOutputNode = outputRoot;
                    ItemNode currentInputNode = RootObjectNode;
                    while (currentInputNode.children.Count() > 0)
                    {
                        ItemNode nextInputNode = null;
                        foreach (ItemNode child in currentInputNode.children.Values)
                        {
                            // Populate the dynamic counts if it doesn't exist for this child
                            if (!dynamicCounts.ContainsKey(child))
                            {
                                dynamicCounts[child] = child.GetDescendentScore();
                            }
                            if (nextInputNode == null || dynamicCounts[child] > dynamicCounts[nextInputNode])
                            {
                                nextInputNode = child;
                            }
                        }

                        // If there are no more valid paths, return
                        if (dynamicCounts[nextInputNode] == 0)
                        {
                            break;
                        }

                        // Add the next node
                        JsonItemNode nextOutputNode = currentOutputNode.children.FirstOrDefault(child => child.itemId == nextInputNode.itemId);
                        if (nextOutputNode == null)
                        {
                            nextOutputNode = new JsonItemNode
                            {
                                children = new List<JsonItemNode>(),
                                itemId = nextInputNode.itemId,
                                numberOfBuilds = nextInputNode.totalOccurrences,
                                numberOfWins = nextInputNode.totalWins,
                                totalBuildTime = nextInputNode.totalBuildTime,
                                thirtySecondIntervals = new Dictionary<string, int>()
                            };
                            foreach (var kvp in nextInputNode.thirtySecondIntervals) {
                                nextOutputNode.thirtySecondIntervals[kvp.Key.ToString()] = kvp.Value;
                            }
                            currentOutputNode.children.Add(nextOutputNode);
                        }
                        currentInputNode = nextInputNode;
                        currentOutputNode = nextOutputNode;
                    }

                    // Update the dynamic counts by penalizing all new nodes in the build tree
                    long penalizeRunningSum = 0;
                    while (currentInputNode != RootObjectNode)
                    {
                        if (currentInputNode.IsSelfPenalized == false)
                        {
                            penalizeRunningSum += currentInputNode.totalOccurrences;
                            currentInputNode.IsSelfPenalized = true;
                        }

                        dynamicCounts[currentInputNode] -= penalizeRunningSum;
                        currentInputNode = currentInputNode.parent;
                    }
                    Debug.Assert(penalizeRunningSum != 0);

                    n--;
                }

                return outputRoot;
            }
        }

        /// <summary>
        /// Parse the input directory and recursively adds all leaf directories to the output list
        /// </summary>
        /// <param name="path">The path to the directory</param>
        /// <param name="list">The output list</param>
        private static void RecursiveParseDictionary(string path, List<string> list)
        {
            foreach (string s in Directory.EnumerateDirectories(path)) {
                RecursiveParseDictionary(s, list);
            }

            // Leaf node
            if (Directory.EnumerateDirectories(path).Count() == 0)
            {
                list.Add(path);
            }
        }

        /// <summary>
        /// The maximum number of threads to use
        /// </summary>
        private const int NumThreads = 8;

        /// <summary>
        /// The global list of all ChampionItemBuilds for patch 5.11
        /// </summary>
        private static Dictionary<int, ChampionItemBuilds> Global511Statistics;

        /// <summary>
        /// The global list of all ChampionItemBuilds for patch 5.14
        /// </summary>
        private static Dictionary<int, ChampionItemBuilds> Global514Statistics;

        /// <summary>
        /// The global mutex, required for updating either of the global statistic dictionary
        /// </summary>
        private static Mutex GlobalMutex;

        /// <summary>
        /// Patch versions
        /// </summary>
        private enum Version{
            Invalid,
            _511,
            _514,
        };

        /// <summary>
        /// Merge a dictionary of ChampionItemBuilds with the global dictionaries
        /// </summary>
        /// <param name="statsToMerge">The champion item builds to merge</param>
        /// <param name="v">The patch version of these builds</param>
        static void MergeWithGlobalList(Dictionary<int, ChampionItemBuilds> statsToMerge, Version v) {
            if (GlobalMutex.WaitOne())
            {
                Dictionary<int, ChampionItemBuilds> dict = null;
                switch (v)
                {
                    case Version._511:
                        dict = Global511Statistics;
                        break;
                    case Version._514:
                        dict = Global514Statistics;
                        break;
                    default:
                        Debug.Assert(false);
                        break;
                }
                foreach (var kvp in statsToMerge)
                {
                    // Add if it doesn't exist, merge if it does
                    if (!dict.ContainsKey(kvp.Key))
                    {
                        dict[kvp.Key] = kvp.Value;
                    }
                    else
                    {
                        dict[kvp.Key].MergeNewItemBuild(kvp.Value);
                    }
                }
                GlobalMutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Worker thread for parsing directories
        /// </summary>
        /// <param name="ListObj">The list of directories to parse</param>
        static void WorkerThread(object ListObj)
        {
            List<string> list = (List<string>)ListObj;

            ChampionOverview[] co = new ChampionOverview[10];
            Dictionary<int, ChampionItemBuilds> championStatistics;
            int ii = 0;

            foreach (string path in list)
            {
                // The local ChampionItemBuilds dictionary for this thread, for this directory
                championStatistics = new Dictionary<int, ChampionItemBuilds>();
                string[] sp = path.Split(new char[] { '\\' });
                string thisPatch = sp[sp.Count() - 3];

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                foreach (string s in Directory.EnumerateFiles(path))
                {
                    if (ii++ % 10 == 0)
                    {
                        Console.Out.WriteLine("{0}: {1} ms", ii++, stopwatch.ElapsedMilliseconds);
                        stopwatch.Restart();
                    }
                    // Read one game
                    using (StreamReader sr = new StreamReader(s))
                    {
                        RootObject ro = JsonConvert.DeserializeObject<RootObject>(sr.ReadToEnd());

                        // Load up all champions for this game
                        for (int i = 0; i < co.Length; i++)
                        {
                            Debug.Assert(ro.participants[i].participantId == i + 1);
                            co[i] = new ChampionOverview { ChampionId = ro.participants[i].championId, Role = ro.participants[i].timeline.role, Lane = ro.participants[i].timeline.lane, itemsPurchased = new List<ItemPurchased>(), WonGame = ro.participants[i].stats.winner };
                        }
                        // Iterate through all timeline frames, find all items purchased for all champions
                        for (int i = 0; i < ro.timeline.frames.Count(); i++)
                        {
                            if (ro.timeline.frames[i].events == null)
                            {
                                continue;
                            }

                            foreach (Event e in ro.timeline.frames[i].events)
                            {
                                // Only item purchases, and final items
                                if (!string.Equals(e.eventType, "ITEM_PURCHASED") || !Utilities.IsFinalItem(int.Parse(e.itemId.Value.ToString())))
                                {
                                    continue;
                                }

                                // We only track up to the first 5 items built per champion
                                if (co[e.participantId - 1].itemsPurchased.Count() >= 5)
                                {
                                    continue;
                                }

                                // Add this item
                                co[e.participantId - 1].itemsPurchased.Add(new ItemPurchased { itemId = Utilities.ConsolidateItemIds(e.itemId.Value), itemTime = e.timestamp / 1000 });
                            }
                        }
                    }

                    // Filter on the relevant champions
                    foreach (ChampionOverview overview in co)
                    {
                        if (string.Equals(overview.Lane, "BOTTOM") || string.Equals(overview.Role, "DUO_SUPPORT") || !Utilities.IsApChampion(overview.ChampionId))
                        {
                            continue;
                        }

                        // Track it in our local dictionary
                        ChampionItemBuilds thisChampionStats = championStatistics.GetOrAddDefault(overview.ChampionId);
                        thisChampionStats.DataPointsParsed++;
                        thisChampionStats.AddItemPath(overview.itemsPurchased, overview.WonGame);

                        // Also track it as part of our global statistics
                        thisChampionStats = championStatistics.GetOrAddDefault(999);
                        thisChampionStats.DataPointsParsed++;
                        thisChampionStats.AddItemPath(overview.itemsPurchased, overview.WonGame);
                    }
                }

                // Merge the local dictionary into the global list
                Version v = Version.Invalid;
                if (string.Equals(thisPatch, "5.11")) {
                    v = Version._511;
                } else if (string.Equals(thisPatch, "5.14")) {
                    v = Version._514;
                } else {
                    Debug.Assert(false);
                }
                MergeWithGlobalList(championStatistics, v);
            }
        }

        /// <summary>
        /// Builds the final JSON objectfrom the global champion statistics dictionary
        /// </summary>
        /// <param name="dict">The input dictionary</param>
        /// <param name="v">The patch version</param>
        static void BuildJsonObject(Dictionary<int, ChampionItemBuilds> dict, Version v)
        {
            // Build our JSON object
            JsonRoot root = new JsonRoot { patch = v == Version._514 ? "5.14" : "5.11", championStatistics = new Dictionary<string, ChampionStatistics>() };
            foreach (var child in dict)
            {
                // JSON Champion Statistics
                ChampionStatistics cis = new ChampionStatistics {
                    totalItemsBuilt = new Dictionary<string, int>(),
                    numberOfDataPoints = child.Value.DataPointsParsed, 
                    itemPaths = null };

                // Copy the total items built dictionary
                foreach (var kvp in child.Value.totalItemsBuilt)
                {
                    cis.totalItemsBuilt.Add(kvp.Key.ToString(), kvp.Value);
                }

                // Construct the output item tree, pruning down to 6 paths
                JsonItemNode rootNode = child.Value.GetTopNItemPaths(6);
                cis.itemPaths = rootNode;
                root.championStatistics[child.Key.ToString()] = cis;
            }

            // Write the output
            using (StreamWriter sw = new StreamWriter(string.Format(@"E:\output-{0}.json", v == Version._514 ? "5.14" : "5.11")))
            {
                sw.Write(JsonConvert.SerializeObject(root));
            }
        }

        /// <summary>
        /// The program main method
        /// </summary>
        /// <param name="args">No arguments</param>
        static void Main(string[] args)
        {
            List<string> fileInputs = new List<string>();
            // Build the list of input directories
            RecursiveParseDictionary(@"E:\loldata", fileInputs);
            Global511Statistics = new Dictionary<int, ChampionItemBuilds>();
            Global514Statistics = new Dictionary<int, ChampionItemBuilds>();
            GlobalMutex = new Mutex();

            List<string>[] threadPaths = new List<string>[NumThreads];
            int nextThread = 0;

            // Load balance the input directories across all available threads
            for (int i = 0; i < NumThreads; i++)
            {
                threadPaths[i] = new List<string>();
            }
            for (int i = 0; i < fileInputs.Count(); i++)
            {
                threadPaths[nextThread].Add(fileInputs[i]);
                nextThread = (nextThread + 1) % NumThreads;
            }

            // Construct and start our threads
            Thread[] t = new Thread[NumThreads];
            for (int i = 0; i < NumThreads; i++) {
                t[i] = new Thread(new ParameterizedThreadStart(WorkerThread));
                t[i].Start((object)threadPaths[i]);
            }

            // Wait for all threads to complete
            for (int i = 0; i < NumThreads; i++)
            {
                t[i].Join();
            }

            // Construct our final JSON output, one per patch
            BuildJsonObject(Global511Statistics, Version._511);
            BuildJsonObject(Global514Statistics, Version._514);

            // Dispose of our mutex
            GlobalMutex.Dispose();
        }
    }
}
