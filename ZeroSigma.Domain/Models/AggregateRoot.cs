﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroSigma.Domain.Models
{
    public abstract class AggregateRoot : Entity
    {
        protected AggregateRoot()
        {
        }
    }
}
