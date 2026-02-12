namespace StellarElysium.Domain.Dtos.Compartilhado;

public class PaginadoResponse<T>
{
    public int Total { get; set; }
    public int Skip { get; set; }
    public int Take { get; set; }
    public List<T> Itens { get; set; } = new();
}
