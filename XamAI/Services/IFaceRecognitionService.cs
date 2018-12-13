using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using XamAI.Models;

namespace XamAI.Services
{
    public interface IFaceRecognitionService
    {
        Task<Face[]> DetectAsync(Stream imageStream, bool returnFaceId, bool returnFaceLandmarks, IEnumerable<FaceAttributeType> returnFaceAttributes);
    }
}
