namespace ReceiptCreator.Api.Repository;

public interface IFileService
{
    public Tuple<int, string> SaveDatabase(IFormFile sqlightDb,String userphone);
    public Tuple<int, string> GetDatabasePath(String userphone);
    
    
}