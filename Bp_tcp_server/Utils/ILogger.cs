﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bp_tcp_server.Utils
{

    public interface IBpLogger
    {
        void Log(string message);
        void Error(string error);
        void Error(Exception ex);
    }
}
