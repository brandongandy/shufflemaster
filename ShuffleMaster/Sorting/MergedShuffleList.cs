using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ShuffleMaster.Model;
using ShuffleMaster.Utilities;

namespace ShuffleMaster.Sorting;

public class MergedShuffleList<T> : ISortableList<T> where T: Song
{
    private const int ConsecutiveArtistMatchCount = 1;
    private const int ConsecutiveAlbumMatchCount = 1;
    private const int MaxRetryCount = 10;
    private const int SplitCount = 4;
    private const int ShuffleCount = 7;

    private readonly Song dummySong = new Song()
    {
        ArtistName = "--",
        Album = "--"
    };

    public IEnumerable<T> ItemList { get; set; }

    public MergedShuffleList(IEnumerable<T> items)
    {
        ItemList = items;
    }
    
    public IEnumerable<T> Sort()
    {
        var list = ItemList;
        
        for (var i = 0; i <= ShuffleCount - 1; i++)
        {
            var splitLists = SplitList(list.ToList(), SplitCount);
            list = MergeLists(splitLists);
        }

        return list;
    }

    private List<List<T>> SplitList(IEnumerable<T> itemList, int splitCount)
    {
        var items = itemList.ToArray();
        return items.Chunk(items.Length / splitCount).Select(songs => songs.ToList()).ToList();
    }

    private IEnumerable<T> MergeLists(List<List<T>> lists)
    {
        var enumerable = lists.ToList();
        var totalCount = enumerable.First().Count;
        var minCount = enumerable.Last().Count;

        var difference = totalCount - minCount;
        var lastList = enumerable.Last();

        lastList.AddRange(Enumerable.Repeat((T)dummySong, difference));
        
        // set result
        var resultList = new List<T>();
        var slice = new Song[enumerable.Count];

        for (var i = 0; i < totalCount - 1; i++)
        {
            for (var l = 0; l <= enumerable.Count - 1; l++)
            {
                slice[l] = enumerable[l][i];
            }

            for (var j = slice.Length - 1; j >= 0; j--)
            {
                var x = RandomNumber.Next(0, j);

                (slice[x], slice[j]) = (slice[j], slice[x]);
            }

            for (var j = 1; j <= slice.Length - 1; j++)
            {
                if (slice[j - 1] == dummySong)
                {
                    continue;
                }
                
                if (slice[j].ArtistName == slice[j - 1].ArtistName)
                {
                    (slice[j - 1], slice[slice.Length - 1]) = (slice[slice.Length - 1], slice[j - 1]);
                }
            }

            if (i > 0)
            {
                var retryCount = 0;
                while (!IsValidPick(slice[0], resultList.TakeLast(1)) &&
                       retryCount < MaxRetryCount)
                {
                    (slice[0], slice[enumerable.Count - 1]) = (slice[enumerable.Count - 1], slice[0]);
                    retryCount++;
                }
            }
            
            resultList.AddRange((IEnumerable<T>)slice.Where(s => s != dummySong).ToList());
        }
        
        return resultList;
    }

    private bool IsValidPick(Song newSong, Song oldSong)
    {
        if (newSong == dummySong)
        {
            return true;
        }

        return newSong.ArtistName != oldSong.ArtistName;
    }

    private bool IsValidPick(Song newSong, IEnumerable<T> lastPickedSongs)
    {
        var pickedSongs = lastPickedSongs.ToList();
        if (!pickedSongs.Any() ||
            newSong == dummySong)
        {
            return true;
        }

        var artistsMatch = pickedSongs.TakeLast(ConsecutiveArtistMatchCount)
            .Any(song => song.ArtistName == newSong.ArtistName);

        if (artistsMatch)
        {
            return false;
        }

        var albumsMatch = pickedSongs.TakeLast(ConsecutiveAlbumMatchCount)
            .Any(song => song.Album == newSong.Album);

        return !albumsMatch;
    }
}