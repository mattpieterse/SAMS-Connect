namespace Connect.Data.Models;

public class FileAttachment
{
#region Schema

    public string FileName { get; set; }


    public string FilePath { get; set; }

#endregion

#region Serialization

    public FileAttachment(
        string localFilePath
    ) {
        if (string.IsNullOrWhiteSpace(localFilePath))
            throw new ArgumentNullException(nameof(localFilePath));

        FilePath = localFilePath;
        FileName = Path.GetFileName(
            localFilePath
        );
    }

#endregion
}
