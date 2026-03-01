namespace Wemcy.RecipeApp.Backend.Services
{
    public class ImageStorageService
    {
        public async Task SaveImage(Guid id, Stream imageData)
        {
            using var filestream = File.OpenWrite(GetImagePath(id));
            await imageData.CopyToAsync(filestream);
        }

        public Stream LoadImage(Guid id)
        {
            var filestream = File.OpenRead(GetImagePath(id));
            return filestream;
        }

        private string GetImagePath(Guid id)
        {
            return Path.Combine("/storage/recipe_images", $"{id}");
        }
    }
}
