using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Lair
{
	public interface IDocumentStore
	{
		[NotNull]
		Task<IDocument> Open();

		Task Save(IDocument document);
	}
}
