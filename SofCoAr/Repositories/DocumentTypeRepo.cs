
using SofCoAr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofCoAr.Repositories
{
    public class DocumentTypeRepo : BaseRepo<DocumentType>, IDocumentTypeRepo
    {
        public DocumentTypeRepo(SofcoContext context = null) : base(context)
        {
        }
    }
}
