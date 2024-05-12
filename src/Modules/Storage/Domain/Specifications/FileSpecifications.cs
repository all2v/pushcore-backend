using Ardalis.Specification;
using Astrum.Storage.Aggregates;

namespace Astrum.Storage.Specifications;

//public class GetFileSpec : Specification<StorageFile>
//{
//}

//public class GetFileByIdSpec : GetFileSpec
//{
//    public GetFileByIdSpec(Guid id)
//    {
//        Query
//            .Where(x => x.Id == id);
//    }
//}

//public class GetFilesByUserIdSpec : GetFileSpec
//{
//    public GetFilesByUserIdSpec(Guid userId) 
//    {
//        Query
//            .Where(File => File.From == userId);
//    }
//}

//public class GetFilesByUserIdPagination : GetFilesByUserIdSpec
//{
//    public GetFilesByUserIdPagination(Guid userId, int startIndex, int count) : base(userId)
//    {
//        Query
//            .OrderByDescending(File => File.DateCreated).Skip(startIndex).Take(count);
//    }
//}