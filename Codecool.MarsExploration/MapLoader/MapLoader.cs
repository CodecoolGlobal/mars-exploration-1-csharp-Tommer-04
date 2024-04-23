using Codecool.MarsExploration.MapElements.Model;
using Codecool.MarsExploration.MapExplorer.MapLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecool.MarsExploration.MapLoader
{
    public class MapLoader : IMapLoader
    {
        public Map Load(string mapFile)
        {
            var mapLines = File.ReadAllLines(mapFile);
            int mapLength = mapLines.GetLength(0);
            string[,] map =  new string[mapLength,mapLength];

            int currentLength = 0;
            int CurrentRow = 0;
            foreach (var line in mapLines)
            {
                
                foreach (var character in line)
                {
                    map[CurrentRow, currentLength] = character.ToString();
                    currentLength++;
                }
                currentLength = 0;
                CurrentRow++;

            }

            return new Map(map, true);
        }
    }
}
