﻿using Antonio.TechTest.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Antonio.TechTest.Application.Services
{
    public interface IOrderService
    {
        void CreateOrder(Order order);
    }
}