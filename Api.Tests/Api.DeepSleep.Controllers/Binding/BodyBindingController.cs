﻿namespace Api.DeepSleep.Controllers.Binding
{
    using global::DeepSleep;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class BodyBindingController
    {
        /// <summary>Posts the length of for maximum request.</summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public MaxRequestLengthModel PostForMaxRequestLength([BodyBound] MaxRequestLengthModel model)
        {
            return model;
        }

        /// <summary>Puts the length of for maximum request.</summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public MaxRequestLengthModel PutForMaxRequestLength([BodyBound] MaxRequestLengthModel model)
        {
            return model;
        }

        /// <summary>Patches the length of for maximum request.</summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public MaxRequestLengthModel PatchForMaxRequestLength([BodyBound] MaxRequestLengthModel model)
        {
            return model;
        }

        /// <summary>Posts for bad request format.</summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public MaxRequestLengthModel PostForBadRequestFormat([BodyBound] MaxRequestLengthModel model)
        {
            return model;
        }

        /// <summary>Puts for bad request format.</summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public MaxRequestLengthModel PutForBadRequestFormat([BodyBound] MaxRequestLengthModel model)
        {
            return model;
        }

        /// <summary>Patches for bad request format.</summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public MaxRequestLengthModel PatchForBadRequestFormat([BodyBound] MaxRequestLengthModel model)
        {
            return model;
        }

        /// <summary>Simples the post.</summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public MaxRequestLengthModel SimplePost([BodyBound] MaxRequestLengthModel model)
        {
            return model;
        }

        /// <summary>Simples the put.</summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public MaxRequestLengthModel SimplePut([BodyBound] MaxRequestLengthModel model)
        {
            return model;
        }

        /// <summary>Simples the patch.</summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public MaxRequestLengthModel SimplePatch([BodyBound] MaxRequestLengthModel model)
        {
            return model;
        }

        /// <summary>Simples the patch.</summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public SimpleMultipartRs SimpleMultipart([BodyBound] MultipartHttpRequest model)
        {
            string fileData = null;
            var filesSection = model.Sections.FirstOrDefault(m => m.Name == "Files");

            if (filesSection != null)
            {
                var stream = model.Sections.FirstOrDefault(m => m.Name == "Files").GetStream();

                using (var reader = new StreamReader(stream, Encoding.UTF8, true, 1024, true))
                {
                    fileData = reader.ReadToEnd();
                }
            }

            return new SimpleMultipartRs
            {
                Value = model.Sections.FirstOrDefault(m => m.Name == "Value")?.Value,
                OtherValue = model.Sections.FirstOrDefault(m => m.Name == "OtherValue")?.Value,
                TextFileData = fileData
            };
        }

        /// <summary>Multiparts the custom.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public SimpleMultipartRs MultipartCustom([BodyBound] SimpleMultipartRq request)
        {
            string fileData = null;
            var filesSection = request.Files.FirstOrDefault(m => m.Name == "files");

            if (filesSection != null)
            {
                var stream = filesSection.GetStream();

                using (var reader = new StreamReader(stream, Encoding.UTF8, true, 1024, true))
                {
                    fileData = reader.ReadToEnd();
                }
            }


            return new SimpleMultipartRs
            {
                Value = request.Value,
                OtherValue = request.OtherValue,
                TextFileData = fileData
            };
        }
    }

    public class SimpleMultipartRs
    {
        public string Value { get; set; }

        public string OtherValue { get; set; }

        public string TextFileData { get; set; }
    }



    public class SimpleMultipartRq
    {
        public string Value { get; set; }

        public string OtherValue { get; set; }

        public IList<MultipartHttpRequestSection> Files { get; set; }
    }

    public class MaxRequestLengthModel
    {
        public string Value { get; set; }
    }
}