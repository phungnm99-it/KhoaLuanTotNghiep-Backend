using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.MailKit;
using WebAPI.Models;
using WebAPI.ModelDTO;
using WebAPI.RepositoryService.Interface;
using WebAPI.UnitOfWorks;
using WebAPI.DataModel;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.RepositoryService.Service
{
    public class SubscriberService : ISubscriberService
    {
        private IMapper _mapper;
        private IUnitOfWork _unitOfWork;
        private IMailService _mailService;
        public SubscriberService(IMapper mapper, IUnitOfWork unitOfWork, IMailService mailService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _mailService = mailService;
        }
        public async Task<SubscriberDTO> AddSubscriberAsync(string email)
        {
            var checkEmailExist = await _unitOfWork.Subscribers
                .GetSubscriberByEmailAsync(email);
            if(checkEmailExist != null)
            {
                if(checkEmailExist.Status == false)
                {
                    checkEmailExist.Status = true;
                    await _unitOfWork.SaveAsync();
                    return _mapper.Map<SubscriberDTO>(checkEmailExist);
                }
                else
                {
                    return _mapper.Map<SubscriberDTO>(checkEmailExist);
                }
            }
            _unitOfWork.Subscribers
                .AddSubscriber(new Subscriber { Email = email, Status = true });
            await _unitOfWork.SaveAsync();
            var sub = await _unitOfWork.Subscribers.GetSubscriberByEmailAsync(email);

            MailRequest request = new MailRequest();
            request.ToEmail = email;
            request.Subject = "[PT Store] Thông báo đăng ký nhận tin tức";
            request.Body = "Đăng ký nhận tin tức thành công!";
            await _mailService.SendEmailAsync(request);
            return _mapper.Map<SubscriberDTO>(sub);
        }

        public async Task<IEnumerable<SubscriberDTO>> GetAllSubscribersAsync()
        {
            List<SubscriberDTO> list = new List<SubscriberDTO>();
            var subsribers = await _unitOfWork.Subscribers.GetAllSubscribersAsync();
            foreach (var item in subsribers)
            {
                list.Add(_mapper.Map<SubscriberDTO>(item));
            }
            return list;
        }

        public async Task<SubscriberDTO> GetSubscriberByIdAsync(int id)
        {
            var sub = await _unitOfWork.Subscribers.GetSubscriberByIdAsync(id);
            if (sub == null) return null;
            return _mapper.Map<SubscriberDTO>(sub);
        }

        public async Task<bool> RemoveSubscriberAsync(int id)
        {
            var sub = await _unitOfWork.Subscribers.GetSubscriberByIdAsync(id);
            if (sub == null || sub.Status == false) return false;
            sub.Status = false;
            _unitOfWork.Subscribers.RemoveSubscriber(sub);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> SendNewsAsync(NewsModel model)
        {
            try
            {
                MailRequestForBCC request = new MailRequestForBCC();
                request.Body = model.Body;
                request.Subject = model.Subject;

                var emails = await _unitOfWork.Subscribers.FindByCondition(em => em.Status == true).ToListAsync();
                request.ToBcc = new List<string>();
                foreach (var em in emails)
                {
                    request.ToBcc.Add(em.Email);
                }

                await _mailService.SendNewsWithBccAsync(request);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
