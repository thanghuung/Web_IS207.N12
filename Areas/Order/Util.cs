using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using static System.Net.WebRequestMethods;

namespace WEB2.Areas.Order {

    public class Util {

        public static string Md5(string sInput) {
            HashAlgorithm algorithmType = default(HashAlgorithm);
            ASCIIEncoding enCoder = new ASCIIEncoding();
            byte[] valueByteArr = enCoder.GetBytes(sInput);
            byte[] hashArray = null;
            // Encrypt Input string
#pragma warning disable SYSLIB0021 // Type or member is obsolete
            algorithmType = new MD5CryptoServiceProvider();
#pragma warning restore SYSLIB0021 // Type or member is obsolete
            hashArray = algorithmType.ComputeHash(valueByteArr);
            //Convert byte hash to HEX
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashArray) {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }

        public static string Sha256(string data) {
            using (var sha256Hash = SHA256.Create()) {
                // ComputeHash - returns byte array
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(data));

                // Convert byte array to a string
                var builder = new StringBuilder();
                foreach (var t in bytes) {
                    builder.Append(t.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static string GetIpAddress() {
            //string ipAddress = "";
            //try {
            //    ipAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            //    if (string.IsNullOrEmpty(ipAddress) || (ipAddress.ToLower() == "unknown"))
            //        ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            //} catch (Exception ex) {
            //    ipAddress = "Invalid IP:" + ex.Message;
            //}

            //string ipList = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            //if (!string.IsNullOrEmpty(ipList)) {
            //    return ipList.Split(',')[0];
            //}

            //return Request.ServerVariables["REMOTE_ADDR"];

            var ipAddress = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString();

            return ipAddress;
        }
    }
}