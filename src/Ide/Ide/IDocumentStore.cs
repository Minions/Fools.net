using System.Threading.Tasks;

namespace Ide
{
    public interface IDocumentStore
    {
        Task<IDocument> Open();
        Task Save(IDocument document);
    }
}