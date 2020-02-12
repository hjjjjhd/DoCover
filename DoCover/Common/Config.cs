using System;
using System.Collections.Generic;
using System.IO;
//using System.Linq;
using System.Web;

/// <summary>
/// 基础配置类
/// </summary>
namespace Com.Alipay
{
    public class Config
    {
        public static string alipay_public_key = @"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAgY6Jo8G1U9VY4zb0bAGdQi723q7FUKhC/1h3nesTcgH5b7MJKVC1Y+sj+J9C3DNVHVj0WhA5lgBIN948LqxshwbsX3vMsjafkM0v/Oar7rKwCn/qLQv+tx4lv0MsVVPH+5OJB7n+eKnsr2qO0dzHLJIzlpjS+5c2rR/RkaISuc1htlNdC1KtG1ga+8mvFxgTYW5SNqalfuAK71o+dA5hui5XaRDeeRsgGYFLiEi/Nm60PsR+LYqsZ1EETBtiC8funl74O9b6+DV/mMrqci7n3WjYQM8bI/gtkkyW+LacFEqJQ1OD4gGZnSCw1yFMHqmC3wAvzUxo0DJJcG82NUgIgwIDAQAB";


        //这里要配置没有经过的原始私钥

        //开发者私钥
        public static string merchant_private_key = @"MIIEpAIBAAKCAQEAsAqDEuqoH3edSEW+jfVeBIhlGR57dA5ZC9qbtN3BX0HL4U8l8WKRHykUgpTivcMdsRIYzluejyxBCBpU56y7GL1auPVD5Owcl1bb4FXkKGjSRiLeJxQy/930pkASoUG/SmDtbwwXRwn4PGrVlkv6LPi1aeE+GsUsNtrtbALYf1o8p+yAOZ28CJRu3oO9ehAjIRBGcvmNRvk9pl/sRJgvNhzRFdG0Obu933gsHl49ks95LiM5+tps1d/RLjZ5RQJQ2wUEaMVs6iqsrU8yZil7GSOW6m3Qq/2E11h1t7qw79ta50rdM3ieOW9/xnB8jSHROKumCp82YHzY4qeF4pJdqQIDAQABAoIBAD9IWBwJvMwrDKA5ainP5a9NdMJV4d0zdzE6sbSP7v6WQXtKH4Kpgy+nbdcPPH8oz9iif8ZWyyX+q5YFGTZ7MnrPPvi0OmbltdV1hO8dETqxi71otWFo8nhmSpck+016vBA5zcCYoRmJbPITGZrAzzsxYM2iCdhHvwAtLSIh17/XkMuyJzSR7gesLyptoye+5kP6DqrxdEDLfGYAPRanYd8zrT8wRG1ENuFuZ++DTOiNWCe1WwYZAnFS4zFNx+Ji7dPM1fmwpChi39tbaULKMRcvPFAK9ljH+nzi6JIz5P0f2sn6Pl+hvl/7srJytV6j/XStWkBOei+15DV0fkwx+U0CgYEA1GdWmBaREES7S/4YkWMNK+FBwpFd9yky+LOQu9QnKYO6XsGgFQzwJa5C4N5+WyHiUZpolD/ravlsvx7eeNhJFllbVtonrZJ8pG3HbvaOcdzosiiKuJ36SMKBo0Hbifuf8TDIuMbEHE3macVZwvReuA4j7jhZoYKeI+fsMxzNYJcCgYEA1CyEGRIu+C5x0iTbYe2s6kl8L07/ciViPhu20294Ik0w96Cv762IXOYCkN76FCU0/sBtekKLjX1reChvNcmW2yK3Z5J6WLh0fglm9kyRT9+MKxpeYRGE2G1+RzqXPJa1DThymX/4R+AEMbNxw99bVamiwRgUgPfno9Qdsu6fu78CgYEAsO84Wk8KHlQy9ZXAX3P0p+XoAq1XaimetT/XxC9xRArgeEixEngJoEnumiWdekranYGTtlMcx6rpJLgROPdqwrxC8zGdNeC+BbcRF+U5Oa59BNPy4uFueafVl+qnd+TtElzCB/JDsRRPaTKlmFo6gPX+hlXYjEsjcuOihd1rVBcCgYEArFcQYZDsh/ipyWjYrd2xs4hHD+JaCWymNQ6r3WhQq4QJv0pNPiC1f8fw053agdyLBFZnVoSQ+DekwLAPTSBWod68HKvVJxWEwg9/C5a7/aX2I9jCSpRBVM7zYIYN7E/59iggpeBVWoRyw7AfO1vEw3sL0U5u8Sbsh503Fm1PX98CgYAGW+s8b7fpYw24vRyGc9tACR/SBVXiBVvyARyNXAaZ42Ac6huCesAyuaH8xlBQYKDr1+IHwN2X4ZurpF8LoQlF7tLywjEJIqUTIJkUhAqNmPHrmMNvY79r5ou6Z3PjwtS3erOaVx7WAOSTIDSaDBSPZrAeCOfOxNYOPccfzD9W4w==";

        //开发者公钥
        public static string merchant_public_key = @"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAgY6Jo8G1U9VY4zb0bAGdQi723q7FUKhC/1h3nesTcgH5b7MJKVC1Y+sj+J9C3DNVHVj0WhA5lgBIN948LqxshwbsX3vMsjafkM0v/Oar7rKwCn/qLQv+tx4lv0MsVVPH+5OJB7n+eKnsr2qO0dzHLJIzlpjS+5c2rR/RkaISuc1htlNdC1KtG1ga+8mvFxgTYW5SNqalfuAK71o+dA5hui5XaRDeeRsgGYFLiEi/Nm60PsR+LYqsZ1EETBtiC8funl74O9b6+DV/mMrqci7n3WjYQM8bI/gtkkyW+LacFEqJQ1OD4gGZnSCw1yFMHqmC3wAvzUxo0DJJcG82NUgIgwIDAQAB";

        //应用ID
        public static string appId = "2019031663533960";

        //合作伙伴ID：partnerID
        public static string pid = "2088022450312973";


        //支付宝网关
        public static string serverUrl = "https://openapi.alipay.com/gateway.do";
        public static string mapiUrl = "https://mapi.alipay.com/gateway.do";
        public static string monitorUrl = "http://mcloudmonitor.com/gateway.do";

        //编码，无需修改
        public static string charset = "utf-8";
        //签名类型，支持RSA2（推荐！）、RSA
        //public static string sign_type = "RSA2";
        public static string sign_type = "RSA2";
        //版本号，无需修改
        public static string version = "1.0";


        /// <summary>
        /// 公钥文件类型转换成纯文本类型
        /// </summary>
        /// <returns>过滤后的字符串类型公钥</returns>
        public static string getMerchantPublicKeyStr()
        {
            StreamReader sr = new StreamReader(merchant_public_key);
            string pubkey = sr.ReadToEnd();
            sr.Close();
            if (pubkey != null)
            {
                pubkey = pubkey.Replace("-----BEGIN PUBLIC KEY-----", "");
                pubkey = pubkey.Replace("-----END PUBLIC KEY-----", "");
                pubkey = pubkey.Replace("\r", "");
                pubkey = pubkey.Replace("\n", "");
            }
            return pubkey;
        }

        /// <summary>
        /// 私钥文件类型转换成纯文本类型
        /// </summary>
        /// <returns>过滤后的字符串类型私钥</returns>
        public static string getMerchantPriveteKeyStr()
        {
            StreamReader sr = new StreamReader(merchant_private_key);
            string pubkey = sr.ReadToEnd();
            sr.Close();
            if (pubkey != null)
            {
                pubkey = pubkey.Replace("-----BEGIN PUBLIC KEY-----", "");
                pubkey = pubkey.Replace("-----END PUBLIC KEY-----", "");
                pubkey = pubkey.Replace("\r", "");
                pubkey = pubkey.Replace("\n", "");
            }
            return pubkey;
        }

    }
}