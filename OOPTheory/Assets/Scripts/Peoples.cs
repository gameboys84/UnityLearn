
public class PeopleBase : IHumanBeing
{
    // ENCAPSULATION
    public string Country { get; protected set; }

    public virtual string IntroduceSelf()
    {
        return $"Country:{Country}\nHobby:{TellHobby()}\n{SayHi()}";
    }

    public virtual string SayHi()
    {
        throw new System.NotImplementedException();
    }

    public virtual string TellHobby()
    {
        return "I don't write my hobby.";
    }
}

public class Chinese : PeopleBase
{
    public Chinese()
    {
        Country = "中国";
    }

    // INHERITANCE
    public override string SayHi()
    {
        return "你好";
    }

    // INHERITANCE
    public override string TellHobby()
    {
        return "编程";
    }

    // POLYMORPHISM
    public string TellHobby(string text)
    {
        return $"我喜欢用{text}{TellHobby()}";
    }
}

public class English : PeopleBase
{
    public English()
    {
        Country = "English";
    }

    // INHERITANCE
    public override string SayHi()
    {
        return "hi";
    }

    // INHERITANCE
    public override string TellHobby()
    {
        return "Tell jokes";
    }
}

public class Russian : PeopleBase
{
    public Russian()
    {
        Country = "Россия";
    }

    // INHERITANCE
    public override string SayHi()
    {
        return "здравствй";
    }

    // INHERITANCE
    public override string TellHobby()
    {
        return "хакер";
    }
}
