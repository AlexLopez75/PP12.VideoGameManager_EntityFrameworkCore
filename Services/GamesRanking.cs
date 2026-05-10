using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.AspNetCore.Hosting;
using VideoGameManager.Models;

namespace VideoGameManager.Services
{
    public class GamesRanking
    {
        public readonly string _path;

        public GamesRanking(IWebHostEnvironment env)
        {
            _path = Path.Combine(env.WebRootPath, "data", "games_ranking.xml");
        }

        public void GenerateRankingXml(IEnumerable<Game> games)
        {
            var rankedGames = games.OrderByDescending(g => g.Score).ToList();

            var doc = new XDocument(
                new XDeclaration("1.0", "utf-8", null),
                new XElement("VideoGameManager",
                    new XElement("Type", "VideoGame Ranking"),
                    new XElement("Games",
                        rankedGames.Select(g =>
                            new XElement("Game",
                                new XElement("Id", g.Id),
                                new XElement("Title", g.Title),
                                new XElement("Genre", g.Genre),
                                new XElement("Year", g.Year),
                                new XElement("Score", g.Score),
                                new XElement("Description", g.Description)
                            )
                        )
                    )
                )
            );
            Directory.CreateDirectory(Path.GetDirectoryName(_path));
            doc.Save(_path);
        }
    }
}
