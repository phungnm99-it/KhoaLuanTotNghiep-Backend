using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Repository.Interface;

namespace WebAPI.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private PTStoreContext RepositoryContext { get; set; }
        public IProductRepository Products { get; set; }
        public IUserRepository Users { get; set; }
        public IBrandRepository Brands { get; set; }
        public ISubscriberRepository Subscribers { get; set; }
        public IFeedbackRepository Feedbacks { get; set; }

        public IOrderRepository Orders { get; set; }
        public IOrderDetailRepository OrderDetails { get; set; }

        public UnitOfWork(PTStoreContext context,
            IProductRepository productRepository,
            IUserRepository userRepository,
            IBrandRepository brandRepository,
            ISubscriberRepository subscriberRepository,
            IFeedbackRepository feedbackRepository,
            IOrderRepository orderRepository,
            IOrderDetailRepository orderDetailRepository)
        {
            RepositoryContext = context;
            Products = productRepository;
            Users = userRepository;
            Brands = brandRepository;
            Subscribers = subscriberRepository;
            Feedbacks = feedbackRepository;
            Orders = orderRepository;
            OrderDetails = orderDetailRepository;
        }

        public async Task SaveAsync()
        {
            await RepositoryContext.SaveChangesAsync();
        }
    }
}
