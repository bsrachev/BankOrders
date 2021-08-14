﻿namespace BankOrders.Services.Users
{
    public interface IUserService
    {
        UserServiceModel GetUserInfo(string userId);

        bool IsOrderUserCreate(int orderOrTemplateId, string userId);

        bool IsTemplateUserCreate(int templateId, string userId);

        bool IsUserApprove(int orderId, string userId);

        bool IsUserPosting(int orderId, string userId);
    }
}
