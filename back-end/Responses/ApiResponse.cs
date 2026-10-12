namespace back_end.Responses;

public class ApiResponse<T>
{
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public T? Dados { get; set; }

    public ApiResponse(bool sucesso, string mensagem, T? dados = default)
    {
        Sucesso = sucesso;
        Mensagem = mensagem;
        Dados = dados;
    }
}