﻿using System;
using itmdevlabs_currency_exchange.exchange.accounting.domain;

namespace itmdevlabs_currency_exchange.exchange.accounting.application
{
    public class InvoicePosition : IPosiionAttributes
    {
        private string number;
        private decimal value;
        private string currency;

        public InvoicePosition(decimal productValue, string currency)
        {
            this.number = "randomnumber";
            this.value = productValue;
            this.currency = currency;
        }

        public string productNumber()
        {
            return number;
        }

        public decimal productValue()
        {
            return value;
        }

        public string valueCurrency()
        {
            return currency;
        }
    }
}
