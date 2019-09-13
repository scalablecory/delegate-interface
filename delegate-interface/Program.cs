using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;

namespace delegate_interface
{
    public class Program : IFoo
    {
        const int InnerLoopCount = 10000;

        public Func<object, int, int, int> Func;
        public IFoo Foo;
        public int X;

        public static int AddStatic(object @this, int x, int y) => ((Program)@this).Add(x, y);
        public int Add(int x, int y) => x + y;

        [GlobalSetup]
        public void GlobalSetup()
        {
            Func = AddStatic;
            Foo = this;
            X = 5;
        }

        [Benchmark]
        public int BenchDelegate()
        {
            int x = X;
            Func<object, int, int, int> addFunc = Func;

            int res = 0;

            for (int i = 0; i < InnerLoopCount; ++i)
            {
                res = addFunc(this, res, x);
            }

            return res;
        }

        [Benchmark]
        public int BenchInterface()
        {
            int x = X;
            IFoo foo = Foo;

            int res = 0;

            for (int i = 0; i < InnerLoopCount; ++i)
            {
                res = foo.Add(res, x);
            }

            return res;
        }

        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Program>();
        }
    }

    public interface IFoo
    {
        int Add(int x, int y);
    }
}
