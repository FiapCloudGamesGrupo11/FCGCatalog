namespace FiapCloudGames.Application.Results
{
    public class ResultList<T> where T : class
    {
        public T Dados { get; private set; }
        public Pagination Paginacao { get; private set; }

        private ResultList() { }

        public static ResultList<T> Criar(T dados, Pagination pagination)
        {
            return new() { Dados = dados, Paginacao = pagination };
        }
    }
}
