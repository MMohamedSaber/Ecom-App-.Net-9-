
using Ecom.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace Ecom.infrastructure.Repositories
{
    public class ImageManagementService : IImageManagementService
    {

        private readonly IFileProvider _fileProvider;

        public ImageManagementService(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }

        public async Task<List<string>> AddImageAsync(IFormFileCollection files, string src)
        {
            var SaveImageSrc = new List<string>();
            var ImageDirectory = Path.Combine("wwwroot", "Images", src);
            if (File.Exists(ImageDirectory) is not true)
            {
                Directory.CreateDirectory(ImageDirectory);
            }

            foreach (var file in files)
            {
                if (file.Length > 0)
                {

                    var ImageName=file.FileName;
                    var ImageSrc = $"/Images/{src}/{ImageName}";
                    var root=Path.Combine(ImageDirectory, ImageName);

                    using(var stream=new FileStream(root,FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    SaveImageSrc.Add(ImageSrc);
                }
            }
            return SaveImageSrc;
        }
        public void DeleteImageAsync(string src)
        {
            var info=_fileProvider.GetFileInfo(src);
            var root = info.PhysicalPath;
            File.Delete(root);
        }
    }
}
