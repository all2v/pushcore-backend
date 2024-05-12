using System.Runtime.Serialization;

namespace Astrum.CodeRev.Application.TrackerService.ViewModels.DTO;

[DataContract]
public class IndexDto
{
    [DataMember]
    public int LineNumber { get; set; }

    [DataMember]
    public int ColumnNumber { get; set; }
}