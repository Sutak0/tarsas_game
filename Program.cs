using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        string[] osvenyek = File.ReadAllLines("osvenyek.txt");
        string[] dobasok = File.ReadAllLines("dobasok.txt").First().Split(' ');

        Console.WriteLine($"2. feladat: Ösvények száma: {osvenyek.Length}, dobások száma: {dobasok.Length}");

        int longestPathIndex = Array.IndexOf(osvenyek, osvenyek.OrderByDescending(s => s.Length).First()) + 1;
        Console.WriteLine($"3. feladat: Leghosszabb ösvény sorszáma: {longestPathIndex}, mezők száma: {osvenyek[longestPathIndex - 1].Length}");

        Console.Write("Ösvény sorszáma: ");
        int pathNumber = int.Parse(Console.ReadLine());
        Console.Write("Játékosok száma (2-5): ");
        int playersCount = int.Parse(Console.ReadLine());

        Console.WriteLine("5. feladat: Mező statisztika");
        var fieldStats = osvenyek[pathNumber - 1].GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());
        foreach (var stat in fieldStats)
            Console.WriteLine($"{stat.Key}: {stat.Value} db");

        using (StreamWriter writer = new StreamWriter("kulonleges.txt"))
        {
            foreach (var kvp in fieldStats.Where(kvp => kvp.Key != 'M'))
                writer.WriteLine($"{kvp.Key}\t{kvp.Value}");
        }

        var playerPositions = new int[playersCount];
        var maxRound = dobasok.Length / playersCount;
        int furthest = 0, winner = 0, winningRound = 0;
        string path = osvenyek[pathNumber - 1];
        int pathLength = path.Length;

        for (int round = 0; round < maxRound; round++)
        {
            for (int player = 0; player < playersCount; player++)
            {
                int roll = int.Parse(dobasok[round * playersCount + player]);
                playerPositions[player] += roll;
                if (playerPositions[player] > furthest)
                {
                    furthest = playerPositions[player];
                    winner = player + 1;
                    winningRound = round + 1;
                }
            }
        }

        Console.WriteLine($"7. feladat: Legmesszebb jutó játékos: {winner}, kör: {winningRound}");

        playerPositions = new int[playersCount];
        furthest = 0;
        winner = 0;
        winningRound = 0;

        for (int round = 0; round < maxRound; round++)
        {
            for (int player = 0; player < playersCount; player++)
            {
                int roll = int.Parse(dobasok[round * playersCount + player]);
                int newPosition = Math.Min(playerPositions[player] + roll, pathLength - 1);
                char fieldType = path[newPosition];


                playerPositions[player] = newPosition;

                if (newPosition > furthest)
                {
                    furthest = newPosition;
                    winner = player + 1;
                    winningRound = round + 1;
                }
            }
        }

        Console.WriteLine($"8. feladat: Nyertes: {winner}");
        Console.Write("A többi bábu pozíciója: ");
        foreach (var pos in playerPositions.Select((value, index) => new { value, index }).Where(p => p.index + 1 != winner))
        {
            Console.Write($"{pos.value} ");
        }
        Console.WriteLine();


    }
}

