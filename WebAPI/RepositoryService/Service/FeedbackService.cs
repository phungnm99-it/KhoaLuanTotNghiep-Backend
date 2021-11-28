using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.DataModel;
using WebAPI.MailKit;
using WebAPI.ModelDTO;
using WebAPI.Models;
using WebAPI.RepositoryService.Interface;
using WebAPI.UnitOfWorks;

namespace WebAPI.RepositoryService.Service
{
    public class FeedbackService : IFeedbackService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private IMailService _mailService;
        public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper, IMailService mailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mailService = mailService;
        }

        public async Task<FeedbackDTO> GetFeedbackByIdAsync(int id)
        {
            var feedback = await _unitOfWork.Feedbacks.GetFeedbackByIdAsync(id);
            return _mapper.Map<FeedbackDTO>(feedback);
        }

        public async Task<IEnumerable<FeedbackDTO>> GetAllFeedbacksAsync()
        {
            var feedbacks = await _unitOfWork.Feedbacks.GetAllFeedbacksAsync();
            List<FeedbackDTO> list = new List<FeedbackDTO>();
            foreach(var feedback in feedbacks)
            {
                list.Add(_mapper.Map<FeedbackDTO>(feedback));
            }
            return list;
        }

        public async Task<FeedbackDTO> CreateFeedbackAsync(FeedbackModel feedback)
        {
            Feedback model = new Feedback
            {
                FullName = feedback.FullName,
                Email = feedback.Email,
                Topic = feedback.Topic,
                Content = feedback.Content,
                FeedbackTime = DateTime.Now,
                IsReplied = null,
                ReplyContent = "",
                ReplyTime = DateTime.Now,
                RepliedBy = null
            };
            _unitOfWork.Feedbacks.CreateFeedback(model);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<FeedbackDTO>(model);
        }

        public async Task<FeedbackDTO> ReplyFeedbackAsync(ReplyFeedbackModel model, int adminId)
        {
            var feedback = await _unitOfWork.Feedbacks.GetFeedbackByIdAsync(model.Id);
            if (feedback == null || feedback.IsReplied.GetValueOrDefault() == true)
                return null;
            feedback.IsReplied = true;
            feedback.RepliedBy = adminId;
            feedback.ReplyContent = model.ReplyContent;
            feedback.ReplyTime = DateTime.Now;
            _unitOfWork.Feedbacks.UpdateFeedback(feedback);
            await _unitOfWork.SaveAsync();

            try
            {
                MailRequest request = new MailRequest();
                request.ToEmail = feedback.Email;
                request.Subject = "[PT Store] Phản hồi feedback";
                request.Body = model.ReplyContent;
                await _mailService.SendEmailAsync(request);
            }
            catch
            {
                return null;
            }
            
            return _mapper.Map<FeedbackDTO>(feedback);
        }
    }
}
