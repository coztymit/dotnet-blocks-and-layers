﻿using System;
using System.Collections.Generic;
using exchange_domain.exchange.accounting.domain.exceptions;
using exchange_domain.exchange.accounting.domain.policy;

namespace exchange_domain.exchange.accounting.domain
{

    public class Invoice
    {
        private Number number;
        private ApproveStatus approveStatus = ApproveStatus.DRAFT;
      
        private Money positionsValue = Money.ZERO_PLN;
        private List<Position> positions = new List<Position>();
        private List<Correction> correction;
        private Seller seller;
        private Buyer buyer;

        //TO należy zamienić na politykę
        private static Money positionValueLimit = new Money(10000);
        //TO należy zamienić na politykę
        private static int positionLimit = 10;

        
        internal Invoice(Seller seller, Buyer buyer)
        {
            this.seller = seller;
            this.buyer = buyer;
            this.number = new Number();

        }

        internal Invoice(Number number, Seller seller, Buyer buyer)
        {
            this.seller = seller;
            this.buyer = buyer;
            this.number = number;
        }

        public ApproveStatus Approve()
        {
            this.approveStatus = ApproveStatus.APPROVED;
            return ApproveStatus.APPROVED;
        }

       internal void AddPosition(Position position, IPositionLimitPolicy positionLimitPolicy)
        {
            //InvariantCheck(position);

            if(!positionLimitPolicy.lessOrEqualsLimit(this.positions.Count + 1))
            {

                throw new Exception();
            }

            if (approveStatus.Equals(ApproveStatus.DRAFT))
            {
                positions.Add(position);
                positionsValue = positionsValue.Add(position.PositionValue());
            }
        }

        internal void AddPositions(List<Position> positions)
        {
            //sprawdzenie invariant
          
            if (approveStatus.Equals(ApproveStatus.DRAFT))
            {
                positions.ForEach(pos =>
                {
                    this.positionsValue = positionsValue.Add(pos.PositionValue());
                    this.positions.Add(pos);
                }
                    );
            }
            else
            {
                throw new CannotAddPositionToApprovedInvoiceException();
            }
        }

       
        //invariant
        private bool LessOrEqualsPositionLimit(int newPositionCount)
        {
            if(this.positions.Count + newPositionCount > 10)
            {
                return false;
            }
            return true;
        }
        //invariant
        private bool ValueLessOrEqualsMoneyLimit(Money newPositionValue)
        {
            var oldAndNewPositionValue = positionsValue.Add(newPositionValue);
            if (oldAndNewPositionValue.lessThan(positionValueLimit)) {
                return false;
            }
            return true;
        }

        private void InvariantCheck(Position position)
        {
            if (!LessOrEqualsPositionLimit(1))
            {
                throw new Exception();
            }
            if (!ValueLessOrEqualsMoneyLimit(position.PositionValue()))
            {
                throw new Exception();
            }
        }

        public String InvoiceNumber()
        {
            return this.number.ToString();
        }

        public override string ToString()
        {
            return number.ToString() + " : " + approveStatus.ToString() + " : "
                 + positionsValue.ToString() + " : " + seller.ToString();
        }
    }

}