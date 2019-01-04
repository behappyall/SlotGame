﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlotGame.Model
{
    public class SignCollection : IEnumerable<Sign>
    {   
        private List<Sign> _signs = new List<Sign>();
        public int Count => _signs.Count;

        public void Add(Sign sign)
        {
            this._signs.Add(sign);
        }

        public IEnumerator<Sign> GetEnumerator()
        {
            return this._signs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this._signs).GetEnumerator();
        }

        private bool CheckValidation()
        {
            return _signs.Sum(s => s.Probability) == 100;
        }


        public Sign GetRandomSign()
        {
            //if (!CheckValidation())
            //    throw new Exception("Sum of Probabilities must be 100");

            decimal rand = Convert.ToDecimal(ConcurrentRandomHelper.GenRandom()) * 100;

            decimal currentSum = 0;

            for (int i = 0; i < this._signs.Count; i++)
            {
                currentSum += this._signs[i].Probability;

                if (currentSum >= rand)
                    return this._signs[i];
            }

            throw new NotImplementedException("Out Random Exception");
        }

        

        public static SignCollection Default = new SignCollection
            {
                new Sign(SignName.HappyCharH, 13.0m),
                new Sign(SignName.HappyCharA, 13.0m),
                new Sign(SignName.HappyCharP, 13.0m),
                new Sign(SignName.HappyCharY, 13.0m),

                new Sign(SignName.HappyFace1, 19.0m),
                new Sign(SignName.HappyFace2, 19.0m),

                new Sign(SignName.HappyVip, 10.0m)
            };
    }
}
