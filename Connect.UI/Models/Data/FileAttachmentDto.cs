using Connect.Data.Models;

namespace Connect.UI.Models.Data;

public class FileAttachmentDto
{
#region Schema

    public string FileName { get; set; } = string.Empty;


    public string FilePath { get; set; } = string.Empty;

#endregion

#region Mapper

    public FileAttachment ToDataObject => new(FilePath);


    public FileAttachmentDto ToUiObject => new() {
        FileName = FileName,
        FilePath = FilePath
    };

#endregion
}
