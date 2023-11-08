using System.Collections.Generic;
using System.Linq;
using ShuffleMaster.Model;

namespace ShuffleMaster.Sorting;

public class SortableListFactory<T> where T: Song
{
    private readonly List<List<T>> chunkedShuffledLists;
    private int currentIndex;
    
    public SortableListFactory(IEnumerable<T> items)
    {
        chunkedShuffledLists = new List<List<T>>();
        
        ShuffleLongList(items);
    }

    public List<T> GetNextChunk()
    {
        if (currentIndex + 1 <= chunkedShuffledLists.Count - 1)
        {
            return chunkedShuffledLists[currentIndex++];
        }
        
        // if we're out of bounds, do a hard reset
        currentIndex = 0;
        ShuffleLongList(chunkedShuffledLists.SelectMany(i => i));

        return chunkedShuffledLists[currentIndex++];
    }

    public List<T> GetPreviousChunk()
    {
        if (currentIndex < 0)
        {
            currentIndex = 0;
        }

        return chunkedShuffledLists[currentIndex--];
    }
    
    public void ShuffleLongList(IEnumerable<T> itemList,
        int itemChunkSize = 100)
    {
        var items = itemList.ToList();
        if (items.Count <= itemChunkSize)
        {
            chunkedShuffledLists.Add(GetSortType(items).Sort().ToList());
            return;
        }

        items = new FisherYatesList<T>(items).Sort().ToList();
        
        // split into chunks
        var chunks = items.Chunk(itemChunkSize).ToArray();

        // shuffle the chunks
        var shuffledChunks = new FisherYatesList<T[]>(chunks).Sort();

        foreach (var chunk in shuffledChunks)
        {
            chunkedShuffledLists.Add(GetSortType(chunk).Sort().ToList());
        }
    }
    
    private ISortableList<T> GetSortType(IEnumerable<T> itemList)
    {
        var items = itemList.ToList();
        if (HasLargeGroupings(items))
        {
            return new MergedShuffleList<T>(items);
        }
        return new FisherYatesList<T>(items);
    }

    public bool HasLargeGroupings(IEnumerable<T> itemList)
    {
        var items = itemList.ToList();
        if (items.Count <= 10)
        {
            // item is essentially a single group (or album)
            // no point in calculating.
            return true;
        }

        var groups = items.GroupBy(s => s.ArtistName)
            .Select(s => s.ToList())
            .ToList();

        var biggestGroupItemCount = groups.Max(s => s.Count);

        var percentage = (double)biggestGroupItemCount / items.Count * 100;

        return percentage >= 15;
    }
}