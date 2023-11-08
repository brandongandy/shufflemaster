using System;
using System.Collections.Generic;
using System.Linq;
using ShuffleMaster.Model;
using ShuffleMaster.Utilities;

namespace ShuffleMaster.Sorting;

public class FisherYatesImprovedList<T> : ISortableList<T> where T: Song
{
    private const int MaxRetryCount = 3;
    private const int MaxInvalidCount = 10;

    private const int ConsecutiveArtistMatchCount = 3;
    private const int ConsecutiveAlbumMatchCount = 4;
    
    private int invalidCount;
    private bool checkValidPick = true;
    public IEnumerable<T> ItemList { get; set; }

    public FisherYatesImprovedList(IEnumerable<T> items)
    {
        ItemList = items;
    }
    
    public IEnumerable<T> Sort()
    {
        var shuffledItemList = ItemList.ToArray();
        var lastPickedItems = new Queue<T>();
        for (var i = shuffledItemList.Length - 1; i >= 0; i--)
        {
            var j = RandomNumber.Next(0, i);

            var retryCount = 0;
            while (!IsValidPick(shuffledItemList[j], lastPickedItems) &&
                   retryCount < MaxRetryCount)
            {
                retryCount++;
                j = RandomNumber.Next(0, i);
            }

            if (retryCount >= MaxRetryCount)
            {
                // short-circuiting; we maxed out our attempts
                // so increment the counter and move on with life
                invalidCount++;

                if (invalidCount >= MaxInvalidCount)
                {
                    checkValidPick = false;
                }
            }
            
            // a winner has been chosen!
            // trim the stack so it doesn't get too long
            while (lastPickedItems.Count >= Math.Max(ConsecutiveAlbumMatchCount, ConsecutiveArtistMatchCount))
            {
                _ = lastPickedItems.TryDequeue(out _);
            }
            
            // then push our choice onto the stack
            lastPickedItems.Enqueue(shuffledItemList[j]);
            
            (shuffledItemList[i], shuffledItemList[j]) = (shuffledItemList[j], shuffledItemList[i]);
        }
        return shuffledItemList;
    }

    private bool IsValidPick(Song newSong, IEnumerable<Song> lastPickedSongs)
    {
        var pickedSongs = lastPickedSongs.ToList();
        if (!checkValidPick ||
            !pickedSongs.Any())
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