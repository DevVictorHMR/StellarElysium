namespace StellarElysium.Domain.Dtos.Desejos.Importacao;

public class ImportacaoDesejosResponse
{
    public Guid ContaGenshinId { get; set; }
    public int TotalRecebido { get; set; }
    public int Importados { get; set; }
    public int Ignorados { get; set; }
}
