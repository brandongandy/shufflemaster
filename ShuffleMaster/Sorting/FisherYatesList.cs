using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using ShuffleMaster.Model;
using ShuffleMaster.Utilities;

namespace ShuffleMaster.Sorting;

public class FisherYatesList<T> : ISortableList<T>
{
    public IEnumerable<T> ItemList { get; set; }

    public FisherYatesList(IEnumerable<T> items)
    {
        ItemList = items;
    }

    public IEnumerable<T> Sort()
    {
        var shuffledItemList = ItemList.ToArray();
        for (var i = shuffledItemList.Length - 1; i >= 0; i--)
        {
            var j = RandomNumber.Next(0, i);
            (shuffledItemList[i], shuffledItemList[j]) = (shuffledItemList[j], shuffledItemList[i]);
        }
        return shuffledItemList;
    }
}