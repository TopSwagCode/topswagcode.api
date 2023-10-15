namespace TopSwagCode.Api.Services;

public interface IHelloWorldService
{
}

[RegisterService<IHelloWorldService>(LifeTime.Scoped)]
public class HelloWorldService : IHelloWorldService
{

}