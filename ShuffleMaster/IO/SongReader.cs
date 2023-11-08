using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using ShuffleMaster.Model;

namespace ShuffleMaster.IO;

public class SongReader
{
    private readonly string csvFilePath;

    public SongReader(string csvFilePath)
    {
        this.csvFilePath = csvFilePath;
    }
    
    public IEnumerable<Song> ReadSongs()
    {
        using var reader = new StreamReader(csvFilePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        return csv.GetRecords<Song>().ToList();
    }
}