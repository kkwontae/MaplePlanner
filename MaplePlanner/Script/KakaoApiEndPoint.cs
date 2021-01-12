using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaplePlanner
{
    class KakaoApiEndPoint
    {
        // API 키
        public const string KakaoRestApiKey = "69c174068e0bf2ef42f25ae3e6aa6406";
        public const string KakaoSendMessageKey = "44464";

        // 리다이렉트 url
        public const string KakaoRedirectUrl = "https://mapleplanner.synology.me/oauth";
        public const string KakaoLogOutRedirectUrl = "https://mapleplanner.synology.me/logout";

        // 로그인 url
        public const string KakaoLogInUrl = "https://kauth.kakao.com/oauth/authorize?client_id=" + KakaoRestApiKey + "&redirect_uri=" + KakaoRedirectUrl + "&response_type=code";

        // 로그아웃 url
        public const string KakaoLogOutUrl = "https://kauth.kakao.com/oauth/logout?client_id=" + KakaoRestApiKey + "&logout_redirect_uri=" + KakaoLogOutRedirectUrl;

        // 루트 url
        public const string KakaoHostOAuthUrl = "https://kauth.kakao.com";
        public const string KakaoHostApiUrl = "https://kapi.kakao.com";

        // 이벤트 url
        public const string KakaoOAuthUrl = "/oauth/token";
        public const string KakaoUnlinkUrl = "/v1/user/unlink";
        public const string KakaoTemplateMessageUrl = "/v2/api/talk/memo/send";
        public const string KakaoDefaultMessageUrl = "/v2/api/talk/memo/default/send";
        public const string KakaoUserDataUrl = "/v2/user/me";
    }
}
