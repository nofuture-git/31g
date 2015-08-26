using System;

namespace NoFuture.Tests
{
    class Program
    {
        public static void Main(string[] args)
        {
            var noop = System.Console.ReadKey();
        }

    }
}
/*
namespace NoFuture.Tests.ScratchPad
{
    //we have a type like this that we are targeting to refactor
    public class RefactorMe
    {
        private string _instanceVar1;
        private string _instaceVar2;
        private int _int1;

        public int ThisNeedsToMove(string arg1, bool arg2)
        {
            if (arg2)
            {
                _instaceVar2 = arg1;
            }
            _instanceVar1 = IGetUsedAlot(arg1);

            ThisHasNoReturnType(_int1, _instanceVar1);

            //more code here
            return 0;
        }

        private string IGetUsedAlot(string something)
        {
            return something;
        }

        private void ThisHasNoReturnType(int something, string somethingelse)
        {
            
        }
        private void IdoNothing()
        {

        }

    }
    //the method ThisNeedsToMove is coupled within its class 
    public class IGotRefactored
    {
          private ImCgCode _ImCgCode = new ImCgCode();//Cg puts this here
        private string _instanceVar1;
        private string _instaceVar2;
        private int _int1;

        public int ThisNeedsToMove(string arg1, bool arg2)
        {
            //this has every dependency send in the fx call
            return _ImCgCode.ThisNeedsToMove(arg1, arg2, IGetUsedAlot, ref _instanceVar1, ref _instaceVar2, ThisHasNoReturnType, IdoNothing);

            //this is the code that was removed
            //if (arg2)
            //{
            //    _instaceVar2 = arg1;
            //}
            //_instanceVar1 = IGetUsedAlot(arg1);

            ////more code here
            //return 0;
        }

        private string IGetUsedAlot(string something)
        {
            return something;
        }

        private void ThisHasNoReturnType(int something, string somethingelse)
        {

        }

        private void IdoNothing()
        {
            
        }
    }

    public class ImCgCode
    {
        public int ThisNeedsToMove(string arg1, bool arg2, Func<string, string> IGetUsedAlot, ref string _instanceVar1, ref string _instaceVar2, Action<int, string> ThisHasNoReturnType, Action IdoNothing)
        {

            if (arg2)
            {
                _instaceVar2 = arg1;
            }
            _instanceVar1 = IGetUsedAlot(arg1);

            IdoNothing();

            //more code here
            return 0;
        }
    }
}
*/