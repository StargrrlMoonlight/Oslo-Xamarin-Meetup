using System;
using System.Collections.Generic;
using System.Text;

namespace XamAI.Models
{
    public class Face
    {
        public Guid FaceId { get; set; }
        public FaceRectangle FaceRectangle { get; set; }
        public FaceLandmarks FaceLandmarks { get; set; }
        public FaceAttributes FaceAttributes { get; set; }
    }
}
