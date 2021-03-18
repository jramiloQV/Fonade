﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace Fonade.FONADE.Utilidades
{
    public static class Utils
    {
        public static string l_requisitos_minimos = "^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?!.*cuenta)(?!.*usuario)(?!.*nombre)(?=.*[!\"#$%&'()*+,\\-./:;<=>?@[\\]^_`{|}~]).{8,20}$";
    }

    public class Encrypt
    {
        public static string GetSHA256(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

    }

    #region AntiguoCODE
    //public static class Utils
    //{
    //    public static string l_requisitos_minimos = "^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?!.*cuenta)(?!.*usuario)(?!.*nombre)(?=.*[!\"#$%&'()*+,\\-./:;<=>?@[\\]^_`{|}~]).{8,20}$";
    //}

    //public class Encrypt
    //{
    //    public static string GetSHA256(string str)
    //    {
    //        SHA256 sha256 = SHA256Managed.Create();
    //        ASCIIEncoding encoding = new ASCIIEncoding();
    //        byte[] stream = null;
    //        StringBuilder sb = new StringBuilder();
    //        stream = sha256.ComputeHash(encoding.GetBytes(str));
    //        for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
    //        return sb.ToString();
    //    }

    //}
    #endregion
}