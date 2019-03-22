﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace RummikubLib.Collections
{
    class SublistsEnumerator<T> : IEnumerator<List<T>>
    {
        readonly IReadOnlyList<T> list;

        readonly BigInteger maxSublistSeed;

        BigInteger sublistSeed;

        List<T> current;

        public SublistsEnumerator(IReadOnlyList<T> list)
        {
            this.list = list ?? throw new ArgumentNullException(nameof(list));
            maxSublistSeed = (new BigInteger(1) << list.Count) - 1;
            sublistSeed = -1;
        }

        public List<T> Current
        {
            get
            {
                if (current == null)
                {
                    throw new InvalidOperationException("There is no current element.");
                }

                return current;
            }
        }

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (sublistSeed >= maxSublistSeed)
            {
                return false;
            }

            ++sublistSeed;

            current = new List<T>();
            BigInteger bitMask = 1;
            foreach (var item in list)
            {
                if ((bitMask & sublistSeed) != 0)
                {
                    current.Add(item);
                }

                bitMask = bitMask << 1;
            }

            return true;
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public void Dispose()
        {
        }
    }
}