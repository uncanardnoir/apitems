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

namespace DataMiner
{
    /// <summary>
    /// This program downloads relevant match data using the Riot API. This is intended for one-off personal use and is not refactored for general use.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Maximum number of threads to use
        /// </summary>
        static private int NumThreads = 12;

        /// <summary>
        /// The match Ids to download
        /// </summary>
        static private List<Id>[] ids;

        /// <summary>
        /// For use in load-balancing across threads.
        /// </summary>
        static private int NextIndex = 0;

        /// <summary>
        /// The API GET request format
        /// </summary>
        static private string UrlFormat = "https://{0}.api.pvp.net/api/lol/{0}/v2.2/match/{1}?includeTimeline=true&api_key={2}";


        /// <summary>
        /// The API key read from an external file.
        /// </summary>
        static private string ApiKey;

        /// <summary>
        /// Represents a match ID to download.
        /// </summary>
        struct Id
        {
            /// <summary>
            /// The ID
            /// </summary>
            public string id;

            /// <summary>
            /// The patch, either 5.11 or 5.14
            /// </summary>
            public string patch;

            /// <summary>
            /// The mode, either RANKED or NORMAL
            /// </summary>
            public string mode;

            /// <summary>
            /// The region, one of ten possible values.
            /// </summary>
            public string region;
        }

        /// <summary>
        /// Returns the output file path of a given match
        /// </summary>
        /// <param name="patch">The patch</param>
        /// <param name="mode">The mode</param>
        /// <param name="region">The region</param>
        /// <param name="id">The id</param>
        /// <returns>The file path where this match should be saved</returns>
        static string GetOutputFile(string patch, string mode, string region, string id, bool directoryOnly = false)
        {
            if (directoryOnly)
            {
                return string.Format("E:\\LolData\\{0}\\{1}\\{2}", patch, mode, region);
            }
            else
            {
                return string.Format("E:\\LolData\\{0}\\{1}\\{2}\\{3}.json", patch, mode, region, id);
            }
        }

        /// <summary>
        /// Called on startup to load balance Ids across all the threads
        /// </summary>
        /// <param name="directory">The root directory</param>
        static void PopulateIds(string directory)
        {
            foreach (var f in Directory.EnumerateFiles(directory))
            {
                string[] sp = f.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                string input;

                using (StreamReader sr = new StreamReader(f))
                {
                    input = sr.ReadToEnd();
                }
                JavaScriptSerializer ser = new JavaScriptSerializer();
                Object[] o = (Object[])ser.DeserializeObject(input);

                for (int i = 0; i < o.Count(); i++)
                {
                    Id id = new Id
                    {
                        patch = sp[2],
                        mode = sp[3],
                        region = sp[4].Substring(0, sp[4].Length - 5),
                        id = o[i].ToString()
                    };

                    // Short-circuit if we already have the file
                    if (File.Exists(GetOutputFile(id.patch, id.mode, id.region, id.id)))
                    {
                        continue;
                    }

                    ids[NextIndex].Add(id);
                    NextIndex = (NextIndex + 1) % NumThreads;
                }
            }

            // Recursively search all children directories
            foreach (var d in Directory.EnumerateDirectories(directory))
            {
                PopulateIds(d);
            }
        }

        /// <summary>
        /// Worker thread to download the matches
        /// </summary>
        /// <param name="indexObj">The index of "ids" to use</param>
        static void WorkerThread(object indexObj)
        {
            int index = (int)indexObj;
            foreach (Id id in ids[index]) {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                string Url = string.Format(UrlFormat, id.region.ToLower(), id.id, ApiKey);

                Stream s = null;
                // On error, wait 200 ms (with exponential backoff)
                int timeout = 200;
                do
                {
                    try
                    {
                        WebRequest getRequest = WebRequest.Create(Url);
                        s = getRequest.GetResponse().GetResponseStream();
                    }
                    catch (Exception e)
                    {
                        Console.Out.WriteLine(e.Message);
                        if (timeout > 10000)
                        {
                            Console.Out.WriteLine("Max timeoutage occured.");
                            break;
                        }
                        Thread.Sleep(timeout);
                        timeout *= 2;
                    }
                } while (s == null);

                if (s == null)
                {
                    // Couldn't get the file
                    continue;
                }

                StreamReader objReader = new StreamReader(s);
                string response = objReader.ReadToEnd();

                Directory.CreateDirectory(GetOutputFile(id.patch, id.mode, id.region, null, true));
                string file = GetOutputFile(id.patch, id.mode, id.region, id.id);
                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.WriteLine(response);
                }

                Console.Out.WriteLine("{0} ms", stopwatch.ElapsedMilliseconds);
            }
        }
        
        /// <summary>
        /// Load balances the IDs to downloads, and spins up all worker threads.
        /// </summary>
        /// <param name="args">No arguments</param>
        static void Main(string[] args)
        {
            // Read our API key.
            using (StreamReader sr = new StreamReader(@"E:\apikey.txt")) {
                ApiKey = sr.ReadToEnd();
            }

            // Load balance
            ids = new List<Id>[NumThreads];
            for (int i = 0; i < NumThreads; i++)
            {
                ids[i] = new List<Id>();
            }

            PopulateIds(@"E:\input");

            // Spin up worker threads.
            for (int i = 0; i < NumThreads; i++) {
                Thread t = new Thread(new ParameterizedThreadStart(WorkerThread));
                t.Start((object)i);
            }
        }

    }
}