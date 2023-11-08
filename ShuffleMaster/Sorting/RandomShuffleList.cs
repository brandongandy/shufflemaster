using System.Collections.Generic;
using System.Linq;
using ShuffleMaster.Utilities;

namespace ShuffleMaster.Sorting;

public class RandomShuffleList<T> : ISortableList<T>
{
    public IEnumerable<T> ItemList { get; set; }

    public RandomShuffleList(IEnumerable<T> items)
    {
        ItemList = items;
    }
    
    public IEnumerable<T> Sort()
    {
        var songList = ItemList.ToList();
        for (var i = 0; i < songList.Count - 1; i++)
        {
            var index = RandomNumber.Next(0, songList.Count - 1);
            yield return songList[index];
        }
    }
}