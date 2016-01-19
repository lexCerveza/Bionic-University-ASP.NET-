using GuitarShop.Models;

namespace GuitarShop.Providers
{
    public interface ISender
    {
        bool SendInfoToClient(Purchase purchase);
    }
}