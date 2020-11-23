public class TestLib : Lib
{
    public override Context InjectLibrary(Context context)
    {
        context.Set("test", new StdLibFunc(Test));
        return context;
    }

    private Object Test(Object[] parameters)
    {
        return parameters[0];
    }
}