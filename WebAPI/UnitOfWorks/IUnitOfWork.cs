using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Repository.Interface;

namespace WebAPI.UnitOfWorks
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; set; }
        IUserRepository Users { get; set; }

        IBrandRepository Brands { get; set; }

        ISubscriberRepository Subscribers { get; set; }

        IFeedbackRepository Feedbacks { get; set; }

        IOrderRepository Orders { get; set; }
        IOrderDetailRepository OrderDetails { get; set; }
        Task SaveAsync();
    }
}
