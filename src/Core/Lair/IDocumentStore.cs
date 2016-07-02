using System.Threading.Tasks;

namespace Lair
{
	public interface IDocumentStore
	{
		Task<IDocument> Open();

		Task Save(IDocument document);
	}
}
