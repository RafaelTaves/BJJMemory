using BJJMemory.Communication.Categorias.Responses;
using BJJMemory.Domain.Entities;
using BJJMemory.Domain.Repositories.Categorias;
using BJJMemory.Domain.Services.LoggedUser;

namespace BJJMemory.Application.UseCases.Categorias.Get;

public class GetCategoria : IGetCategoria
{
    private readonly ILoggedUser _loggedUser;
    private readonly ICategoriaReadOnlyRepository _categoriaReadOnlyRepository;

    public GetCategoria(ILoggedUser loggedUser, ICategoriaReadOnlyRepository categoriaReadOnlyRepository)
    {
        _loggedUser = loggedUser;
        _categoriaReadOnlyRepository = categoriaReadOnlyRepository;
    }

    public async Task<IList<ResponseCategoriaTree>> Execute()
    {
        var user = await _loggedUser.Get();
        var categorias = await _categoriaReadOnlyRepository.GetAllByUsuarioId(user.Id);

        return BuildTree(categorias);
    }

    private static IList<ResponseCategoriaTree> BuildTree(IList<Categoria> categorias)
    {
        var groupedByParent = categorias
            .OrderBy(categoria => categoria.Nome)
            .ToLookup(categoria => categoria.ParentId);

        return BuildChildren(null);

        List<ResponseCategoriaTree> BuildChildren(Guid? parentId)
        {
            return groupedByParent[parentId]
                .Select(child => new ResponseCategoriaTree
                {
                    Id = child.Id,
                    Nome = child.Nome,
                    Subcategorias = BuildChildren(child.Id)
                })
                .ToList();
        }
    }
}
