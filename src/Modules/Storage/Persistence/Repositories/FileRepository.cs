using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Astrum.SharedLib.Persistence.Repositories;
using Astrum.Storage.Aggregates;
using Astrum.Storage.Repositories;

namespace Astrum.Storage.Persistance.Repositories
{
    public class FileRepository : EFRepository<StorageFile, Guid, StorageDbContext>, IFileRepository
    {
        public FileRepository(StorageDbContext context, ISpecificationEvaluator? specificationEvaluator = null) 
            : base(context, specificationEvaluator) 
        { 
        }
    }
}
