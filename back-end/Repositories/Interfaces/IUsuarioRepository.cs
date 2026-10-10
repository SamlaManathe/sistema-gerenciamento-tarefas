using System.Threading.Tasks;
using back_end.Models;

namespace back_end.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        //Retorna A Usuaria criada
        Task<Usuario> AdicionarAsync(Usuario usuario);

        Task<Usuario?> ObterPorEmailAsync(string email);

        Task<Usuario?> ObterPorIdAsync(int id);

    }
    
}

