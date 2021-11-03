namespace Rhino.Mocks
{
	using Moq;

	public static class MockRepository
    {
        public static Mock<T> Get<T>(T obj) where T : class
        {
	        return Mock.Get(obj);
        }

        public static T GenerateStub<T>() where T : class
        {
            return GenerateMock<T>();
        }

        public static T GenerateStrictMock<T>() where T : class
        {
            return GenerateMock<T>(MockBehavior.Strict);
        }

        public static T GenerateMock<T>(params object[] args) where T : class
        {
            return new Mock<T>(args).Object;
        }

        public static T GenerateMock<T>(MockBehavior behavior = MockBehavior.Default) where T : class
        {
            return new Mock<T>(behavior).Object;
        }
    }
}
