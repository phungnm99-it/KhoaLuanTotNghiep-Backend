using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.MailKit;
using WebAPI.Models;
using WebAPI.ModelDTO;
using WebAPI.RepositoryService.Interface;
using WebAPI.UnitOfWorks;

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
            var checkEmailExist = await _unitOfWork.SubscriberRepository
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
            _unitOfWork.SubscriberRepository
                .AddSubscriber(new Subscriber { Email = email, Status = true });
            await _unitOfWork.SaveAsync();
            var sub = await _unitOfWork.SubscriberRepository.GetSubscriberByEmailAsync(email);

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
            var subsribers = await _unitOfWork.SubscriberRepository.GetAllSubscribersAsync();
            foreach (var item in subsribers)
            {
                list.Add(_mapper.Map<SubscriberDTO>(item));
            }
            return list;
        }

        public async Task<SubscriberDTO> GetSubscriberByIdAsync(int id)
        {
            var sub = await _unitOfWork.SubscriberRepository.GetSubscriberByIdAsync(id);
            if (sub == null) return null;
            return _mapper.Map<SubscriberDTO>(sub);
        }

        public async Task<bool> RemoveSubscriberAsync(int id)
        {
            var sub = await _unitOfWork.SubscriberRepository.GetSubscriberByIdAsync(id);
            if (sub == null || sub.Status == false) return false;
            sub.Status = false;
            _unitOfWork.SubscriberRepository.RemoveSubscriber(sub);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
