namespace asciiadventure
{
    public class Twin<T1, T2>
    {
            public T1 t1 { get; private set; }
            public T2 t2 { get; private set; }
            public Twin(T1 t1, T2 t2)
            {
                this.t1 = t1;
                this.t2 = t2;
            }
            public void Deconstruct(out T1 one, out T2 two)
            {//The Tuple class, originally used in this project, caused an error when the program attemted to implicitly infer the type.
             //Upon googling the error message, I learned how to make a Deconstruct method, but do not have access to Tuples generically.
             //So, I created a whole new Tuple replacement class that has a "Deconstruct" method.
                one = t1;
                two = t2;
            }
        }
}