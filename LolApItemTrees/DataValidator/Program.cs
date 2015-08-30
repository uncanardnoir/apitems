using DataParser;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Drawing.Imaging;

namespace DataValidator
{
    /// <summary>
    /// This program analyzes the output JSON data (from DataParser) and calculates various statistics.
    /// </summary>
    class Program
    {
        static string[] filenames = new string[] {
            @"C:\Users\Mike\Documents\Visual Studio 2013\Projects\DataMiner\ItemTrees\JSON\output-5.11.json",
            @"C:\Users\Mike\Documents\Visual Studio 2013\Projects\DataMiner\ItemTrees\JSON\output-5.14.json",
        };

        /// <summary>
        /// This program analyzes the output JSON data (from DataParser) and calculates various statistics.
        /// </summary>
        /// <param name="args">No input arguments</param>
        static void Main(string[] args)
        {
            // Read input from the filenames array and constructs the JSON objects
            JsonRoot _5_11_root;
            JsonRoot _5_14_root;
            using (StreamReader sr = new StreamReader(filenames[0]))
            {
                _5_11_root = JsonConvert.DeserializeObject<JsonRoot>(sr.ReadToEnd());
            }

            using (StreamReader sr = new StreamReader(filenames[1]))
            {
                _5_14_root = JsonConvert.DeserializeObject<JsonRoot>(sr.ReadToEnd());
            }

            // Calculate differences in build rates for all mages across both patches
            /*foreach (var kvp in _5_11_root.championStatistics["999"].totalItemsBuilt)
            {
                Console.Out.WriteLine("{0}\t{1}\t{2}\t{3}",
                    kvp.Key,
                    Utilities.ItemIdToName(int.Parse(kvp.Key)),
                    (double)kvp.Value / _5_11_root.championStatistics["999"].numberOfDataPoints,
                    (double)_5_14_root.championStatistics["999"].totalItemsBuilt[kvp.Key] / _5_14_root.championStatistics["999"].numberOfDataPoints);
            }*/

            // Calculate differences in build rates of runeglaive/magus for each champion for both patches
            string query = "3708";
            foreach (var key in _5_11_root.championStatistics.Keys)
            {
                Console.Out.WriteLine("{0}\t{1}\t{2}\t{3}",
                    key,
                    Utilities.ChampionIdToName(int.Parse(key)),
                    _5_11_root.championStatistics[key].totalItemsBuilt.ContainsKey(query) ? (double)_5_11_root.championStatistics[key].totalItemsBuilt[query] / _5_11_root.championStatistics[key].numberOfDataPoints : 0,
                    _5_14_root.championStatistics[key].totalItemsBuilt.ContainsKey(query) ? (double)_5_14_root.championStatistics[key].totalItemsBuilt[query] / _5_14_root.championStatistics[key].numberOfDataPoints : 0);
            }

            // Calculate similarity scores for all champions across both patches
            /*foreach (var kvp in _5_14_root.championStatistics)
            {
                Console.Out.Write("\t{0}", Utilities.ChampionIdToName(int.Parse(kvp.Key)));
            }

            Console.Out.WriteLine();
            foreach (var kvp in _5_14_root.championStatistics)
            {
                Console.Out.Write("{0}", Utilities.ChampionIdToName(int.Parse(kvp.Key)));

                foreach (var kvp2 in _5_14_root.championStatistics)
                {
                    Console.Out.Write("\t{0}",
                        Utilities.CalculateSimilarityScore(
                            kvp.Value.totalItemsBuilt,
                            kvp.Value.numberOfDataPoints,
                            kvp2.Value.totalItemsBuilt,
                            kvp2.Value.numberOfDataPoints
                    ));
                }

                Console.Out.WriteLine();
            }*/
        }
    }
}
