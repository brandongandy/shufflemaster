namespace ShuffleMaster.Model;

public class Song
{
    public string TrackName { get; set; } = string.Empty;
    public string ArtistName { get; set; } = string.Empty;
    public string Album { get; set; } = string.Empty;
    public string ISRC { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"{ArtistName} - {TrackName}";
    }
}