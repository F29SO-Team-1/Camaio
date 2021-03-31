using Login.Data.Interfaces;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Login.Service
{
    public class VisionService : IVision
    {
        public VisionService()
        {

        }

        public async Task<ImageAnalysis> AnalyzeImageUrl(ComputerVisionClient client, string imageUrl)
        {
            // Creating a list that defines the features to be extracted from the image. 

            List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
            {
                VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
                VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
                VisualFeatureTypes.Tags, VisualFeatureTypes.Adult,
                VisualFeatureTypes.Color, VisualFeatureTypes.Brands,
                VisualFeatureTypes.Objects
            };

            /*
             *      Analyzing the image...
             */
            // Analyze the URL image 
            ImageAnalysis results = await client.AnalyzeImageAsync(imageUrl, features);

            return results;
        }

        public void Tags(ImageAnalysis results)
        {
            // Image tags and their confidence score
            Console.WriteLine("Tags:");
            foreach (var tag in results.Tags)
            {
                Console.WriteLine($"{tag.Name} {tag.Confidence}");
            }
            Console.WriteLine();
        }

        public void Objects(ImageAnalysis results)
        {
            // Objects
            Console.WriteLine("Objects:");
            foreach (var obj in results.Objects)
            {
                Console.WriteLine($"{obj.ObjectProperty} with confidence {obj.Confidence} at location {obj.Rectangle.X}, " +
                  $"{obj.Rectangle.X + obj.Rectangle.W}, {obj.Rectangle.Y}, {obj.Rectangle.Y + obj.Rectangle.H}");
            }
            Console.WriteLine();
        }

        public void Faces(ImageAnalysis results)
        {
            // Faces
            Console.WriteLine("Faces:");
            foreach (var face in results.Faces)
            {
                Console.WriteLine($"A {face.Gender} of age {face.Age} at location {face.FaceRectangle.Left}, " +
                  $"{face.FaceRectangle.Left}, {face.FaceRectangle.Top + face.FaceRectangle.Width}, " +
                  $"{face.FaceRectangle.Top + face.FaceRectangle.Height}");
            }
            Console.WriteLine();
        }

        public bool Description(ImageAnalysis results)
        {
            List<string> list = new List<string>();
            //Description
            foreach (var tags in results.Description.Tags)
            {
                list.Add(tags);
            }
            foreach (var obj in results.Objects)
            {
                list.Add(obj.ObjectProperty);
            }

            return Check(list);
        }

        //true means that it has a human element
        //false means that it DOES NOT have a human element
        private bool Check(List<string> l)
        {
            //tags that have human elements
            IEnumerable<string> notAllowed = new List<string> { "human", "finger", "person" };

            //returns a int of how many notAllowed the image contains
            int num = l.Intersect(notAllowed).Count();

            if (num >= 1) return true; else return false;
        }

    }
}
