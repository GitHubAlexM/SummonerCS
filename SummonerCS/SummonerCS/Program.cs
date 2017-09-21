using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace SummonerCS
{
    class Program
    {
        //using regular expressions to span class one well defined element
        static Regex MedalSpanPattern =  new Regex (@"<span class=""tierRank"">(.+?)</span>");
        static Regex PointsSpanPattern =  new Regex (@"<span class=""LeaguePoints"">(.+?)</span>");
        static Regex WinsSpanPattern = new Regex(@"<span class=""wins"">(.+?)</span>");
        static Regex LossesSpanPattern = new Regex(@"<span class=""losses"">(.+?)</span>");
        static Regex WinRatioSpanPattern = new Regex(@"<span class=""winratio"">(.+?)</span>");

        static void Main(string[] args)
        {
            Console.Write("Enter Summoner Name: ");
            string summonerName = Convert.ToString(Console.ReadLine()).Trim();//set user input as summonerName
            args = new[]
            {
                "http://na.op.gg/summoner/userName="+summonerName//URL Address including the user input of "summonerName"
            };

            if (summonerName == "")//validation
            {
                Console.WriteLine("Invalid arguments: no input for summoner name");
                Environment.Exit(0);
            }

            using (var client = new WebClient())//get content of page
            {   //this will be the content we will be working with
                var content = SiteContent(client.DownloadString(args[0]));

                var medal = MedalSpanPattern.Matches(content).Cast<Match>().Single().Groups[1].Value.Trim();
                var points = PointsSpanPattern.Matches(content).Cast<Match>().Single().Groups[1].Value.Trim();
                var wins = WinsSpanPattern.Matches(content).Cast<Match>().Single().Groups[1].Value.Trim();
                var losses = LossesSpanPattern.Matches(content).Cast<Match>().Single().Groups[1].Value.Trim();
                var winRatio = WinRatioSpanPattern.Matches(content).Cast<Match>().Single().Groups[1].Value.Trim();
                Console.WriteLine("Summoner: {0}\nRank: {1}\nLP: {2}\n{3} {4}\n{5}", summonerName, medal, points, wins, losses, winRatio);//output

                //ridiculous amount of whitespace for the LP text so use .Value.Trim() to remove that shit
            }
        }
        static string SiteContent(string content)
        {
            return new string(content.Where(c => c != '\n').ToArray());
        }
    }
}
