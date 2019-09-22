﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DeepSleep.Auth
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiKeyInfo
    {
        /// <summary>Gets or sets the public key.</summary>
        /// <value>The public key.</value>
        public string PublicKey { get; set; }

        /// <summary>Gets or sets the private key.</summary>
        /// <value>The private key.</value>
        public string PrivateKey { get; set; }
    }
}
