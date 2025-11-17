using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Exceptions
{
    public abstract class NotFoundException(string message) : Exception(message)
    {
    }

    public sealed class ProductNotFoundException(int Id)
        : NotFoundException($"Product with Id {Id} was not found.")
    {
    }
    public sealed class BasketNotFoundException(string Id)
        : NotFoundException($"Basket with {Id} was not found.")
    {
    }
}
