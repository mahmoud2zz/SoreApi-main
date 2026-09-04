using System;
using CoffeeStoreApi.Models;

namespace CoffeeStoreApi.Repositories.Implementations
{
	public interface IDeliveryInformationRepositroy
	{
		Task CreateDeliveryInformation(DeliveryInformation deliveryInformation);

    }
}

