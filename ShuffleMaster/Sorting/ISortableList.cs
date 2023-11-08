using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using ShuffleMaster.Model;

namespace ShuffleMaster.Sorting;

public interface ISortableList<T>
{
    IEnumerable<T> ItemList { get; set; }
    
    /// <summary>
    /// Sorts the entire <see cref="ItemList"/> using a custom sorting / shuffling algorithm.
    /// </summary>
    /// <returns>A new IEnumerable, sorted or shuffled.</returns>
    IEnumerable<T> Sort();
}