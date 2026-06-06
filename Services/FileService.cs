namespace GLMS.API.Services
{
    public class FileService
    {
        public async Task<string> SavePdfAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new Exception("File is empty");

            var extension = Path.GetExtension(file.FileName).ToLower();

            if (extension != ".pdf")
                throw new Exception("Only PDF files can be uploaded");

            var folder = Path.Combine("wwwroot/uploads");

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var fileName = $"{Guid.NewGuid()}.pdf";
            var path = Path.Combine(folder, fileName);

            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            return fileName;
        }
    }
}