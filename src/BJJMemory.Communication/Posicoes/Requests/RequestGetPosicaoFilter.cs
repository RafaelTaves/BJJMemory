namespace BJJMemory.Communication.Posicoes.Requests;

public class RequestGetPosicaoFilter
{
    public string? Nome { get; set; }

    public Guid? CategoriaId { get; set; }

    public Guid? SubcategoriaId { get; set; }
}
