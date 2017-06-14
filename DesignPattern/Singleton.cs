namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            a model = b.instance;


            Console.WriteLine(model.aa);
        }
    }

    class a
    {
        protected static object lockobj = new object();
        protected static a _instance = null;
        public static a instance
        {
            get
            {
                lock (lockobj) //double check locking
                {
                    if (_instance == null)
                    {
                        _instance = new a();
                    }
                }

                return _instance;
            }
        }
        protected a() { }

        public virtual int aa { get => 1; }
    }

    class b : a
    {
        public new static b instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new b();
                }
                return _instance as b;
            }
        }
        protected b() { }

        public override int aa { get => 2; }
    }
}
