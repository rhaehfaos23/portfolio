using System.Threading.Tasks;

namespace DearManager.Accessible
{
    interface ICanHttpMethodGet<T>
    {
        Task<T> HttpGetAsync();
    }

    interface ICanHttpMethodPost
    {
        Task HttpPostAsync(string data);
    }

    interface ICanHttpMethodPost<T>
    {
        Task<T> HttpPostAsync(string data);
    }

    interface ICanHttpMethodPut
    {
        Task HttpPutAsync(string data);
    }

    interface ICanHttpMethodDelete
    {
        Task HttpDeleteAsync(string data);
    }
}
