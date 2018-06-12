using System;

namespace DearManager.Model
{
    static class HttpUrl
    {
        public static string GetUsersUrl()
        {
            return Statics.baseUrl + "/users";
        }

        public static string GetGroupsUrl()
        {
            return Statics.baseUrl + "/groups";
        }

        public static string GetUsersInfoUrl(string userId)
        {
            return GetUsersUrl() + $"/{userId}";
        }

        public static string GetUsersFriendsUrl(uint userId)
        {
            return GetUsersInfoUrl(userId.ToString()) + "/friends";
        }

        public static string GetUsersGroupsUrl(uint userId)
        {
            return GetUsersInfoUrl(userId.ToString()) + "/groups";
        }

        public static string GetUsersRequestUrl(uint userId)
        {
            return GetUsersInfoUrl(userId.ToString()) + "/request";
        }

        public static string GetUsersImagesUrl(uint userId)
        {
            return GetUsersInfoUrl(userId.ToString()) + "/images";
        }

        public static string GetGroupInfoUrl(uint groupId)
        {
            return GetGroupsUrl() + $"/{groupId}";
        }

        public static string GetGroupBalanceUrl(uint groupId)
        {
            return GetGroupInfoUrl(groupId) + "/balance";
        }

        public static string GetGroupMemberUrl(uint groupId)
        {
            return GetGroupInfoUrl(groupId) + "/member";
        }

        public static string GetGroupReceiptsUrl(uint groupId, int page, int size)
        {
            UriBuilder uBuilder = new UriBuilder(GetGroupInfoUrl(groupId) + "/receipts")
            {
                Query = $"page={page}&size={size}"
            };

            return uBuilder.ToString();
        }
    }
}
