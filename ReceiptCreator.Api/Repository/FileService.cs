namespace ReceiptCreator.Api.Repository;

public class FileService:IFileService
{
    private IWebHostEnvironment _environment;

    public FileService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }
    
    private readonly string _backupPath = "/app/files/backups";

    
    public Tuple<int, string> SaveDatabase(IFormFile sqlightDb, string userphone)
    {
        
        try
        {

                var contentPath = this._environment.ContentRootPath;
                var path = Path.Combine(_backupPath, userphone);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                
                var ext = Path.GetExtension(sqlightDb.FileName);
                var allowedExtensions = new string[] { ".csv" };
                if (!allowedExtensions.Contains(ext))
                {
                    string msg = string.Format("Only {0} extensions are allowed",
                        string.Join(",", allowedExtensions));
                    return new Tuple<int, string>(0, msg);
                }

                var newFileName = "receipt";
                var fileWithPath = Path.Combine(path, newFileName);
                if (System.IO.File.Exists(fileWithPath))
                {
                    System.IO.File.Delete(fileWithPath);
                }

                using (var stream = new FileStream(fileWithPath, FileMode.Create))
                {
                    sqlightDb.CopyTo(stream);
                }
                
                return new Tuple<int, string>(1, "backups/" + userphone + "/" + newFileName);
                
        }
        catch (Exception ex)
        {
            return new Tuple<int, string>(0, "Error has occured");
        }
    }

  

    public Tuple<int, string> GetDatabasePath(string userphone)
    {
        try
        {
            var contentPath = this._environment.ContentRootPath;
            var userFolderPath = Path.Combine(_backupPath, userphone);

            var dbFilePath = Path.Combine(userFolderPath, "receipt");
            return new Tuple<int, string>(1, dbFilePath);
        }
        catch (Exception e)
        {
            return new Tuple<int, string>(0, "Error has occured");

        }
        
    }
}