using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using ShuffleMaster.IO;
using ShuffleMaster.Model;
using ShuffleMaster.Sorting;

namespace ShuffleMaster;

internal class Program
{
    private static void Main(string[] args)
    {
        var songs = LoadSongs("allofit.csv");
        var condensedList = LoadSongs("allofit-condensed.csv");
        
        // Console.WriteLine("=============================");
        // Console.WriteLine("True Random Shuffle");
        // Console.WriteLine("=============================");
        //
        // var randomList = TrueRandomShuffle(songs);
        // WriteSongsToConsole(randomList);
        //
        // Console.WriteLine("=============================");
        // Console.WriteLine("Fisher-Yates Shuffle");
        // Console.WriteLine("=============================");
        // var fisherYatesV1 = FisherYatesShuffle(songs);
        // WriteSongsToConsole(fisherYatesV1);
        
        Console.WriteLine("=============================");
        Console.WriteLine("Improved Fisher-Yates Shuffle");
        Console.WriteLine("=============================");
        var fisherYatesV2 = ImprovedFisherYatesShuffle(songs);
        //WriteSongsToConsole(fisherYatesV2);

        // Console.WriteLine("==========================================");
        // Console.WriteLine("Improved Fisher-Yates Shuffle w/ Condensed");
        // Console.WriteLine("==========================================");
        // var fisherYatesV2Condensed = ImprovedFisherYatesShuffle(condensedList);
        // WriteSongsToConsole(fisherYatesV2Condensed);
        
        Console.WriteLine("==================");
        Console.WriteLine("Merge-List Shuffle");
        Console.WriteLine("==================");
        var mergedShuffleList = MergedShuffle(condensedList);
        WriteSongsToConsole(mergedShuffleList);

        // Console.WriteLine("===============================");
        // Console.WriteLine("Merge-List Shuffle w/ Full List");
        // Console.WriteLine("===============================");
        // var mergeShuffleFullList = MergedShuffle(songs);
        // WriteSongsToConsole(mergeShuffleFullList);

        // Console.WriteLine("===============================");
        // Console.WriteLine("Factory Settings");
        // Console.WriteLine("===============================");
        // var factory = new SortableListFactory<Song>(songs);
        //
        // var hasLargeGroupings = factory.HasLargeGroupings(songs.Take(100));
        // Console.WriteLine($"Has large groupings in first 100: {hasLargeGroupings}");
        //
        // var list = factory.GetNextChunk();
        // WriteSongsToConsole(list);
        //
        // list = factory.GetNextChunk();
        // WriteSongsToConsole(list);
    }

    private static List<Song> TrueRandomShuffle(IEnumerable<Song> songs)
    {
        var rsl = new RandomShuffleList<Song>(songs);
        return rsl.Sort().ToList();
    }

    private static List<Song> FisherYatesShuffle(IEnumerable<Song> songs)
    {
        var fyl = new FisherYatesList<Song>(songs);
        return fyl.Sort().ToList();
    }

    private static List<Song> ImprovedFisherYatesShuffle(IEnumerable<Song> songs)
    {
        var fyl = new FisherYatesImprovedList<Song>(songs);
        return fyl.Sort().ToList();
    }

    private static List<Song> MergedShuffle(IEnumerable<Song> songs)
    {
        var msl = new MergedShuffleList<Song>(songs);
        return msl.Sort().ToList();
    }

    private static List<Song> LoadSongs(string filename)
    {
        var path = filename;
        var sr = new SongReader(path);
        return sr.ReadSongs().ToList();
    }

    private static void WriteSongsToConsole(List<Song> songs, int linesToWrite = 100)
    {
        for (var i = 0; i < linesToWrite; i++)
        {
            Console.WriteLine($"[{i}]: {songs.ElementAt(i)}");
        }
    }

    private static void ShuffleAlphabet()
    {
        var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var foo = new FisherYatesList<char>(alphabet);

        var sortedList = new List<string>();

        var sb = new StringBuilder();
        var lev = new Fastenshtein.Levenshtein(alphabet);
        for (var i = 0; i < 10; i++)
        {
            sb.Clear();
            var newAlphabet = foo.Sort();
            sb.AppendJoin(null, newAlphabet);
            sortedList.Add(sb.ToString());
        }

        Console.WriteLine("Output:");
        Console.WriteLine(alphabet);
        foreach (var str in sortedList)
        {
            var distance = lev.DistanceFrom(str);
            Console.WriteLine($"{str} : {distance}");
        }
    }
}