using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Threading.Tasks;

namespace Login.Data.Interfaces
{
    public interface IVision
    {
        Task<ImageAnalysis> AnalyzeImageUrl(ComputerVisionClient client, string imageUrl);
        void Tags(ImageAnalysis results);
        void Objects(ImageAnalysis results);
        void Faces(ImageAnalysis results);

        bool Description(ImageAnalysis results);
    }
}
