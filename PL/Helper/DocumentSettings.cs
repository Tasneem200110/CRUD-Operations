namespace PL.Helper
{
    public static class DocumentSettings
    {
        public static string UploadFile(IFormFile File, string FolderName)
        {
            //1- Get located folder path
            var folderpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", FolderName);

            //2- Get file name and make it unique
            var FileName = $"{Guid.NewGuid()}_{Path.GetFileName(File.FileName)}";

            //3- Get File path
            var filepath = Path.Combine(folderpath, FileName);

            //4- 
            using var fileStream = new FileStream(filepath, FileMode.Create);
            File.CopyTo(fileStream);
            return FileName;

        }

        public static int DeleteFile(string ImageUrl, string folderName)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", folderName);
            var filePath = Path.Combine(folderPath, ImageUrl);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return 0;
            }
            return -1;


        }
    }
}
