namespace BuildTray.Logic
{
    public class FailedTest
    {
        public string ClassName { get; set; }
        public string TestName { get; set; }
        public string Output { get; set; }
        public string FailedBy { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as FailedTest;
            if (other == null)
                return false;

            return other.ClassName == ClassName && other.TestName == TestName;
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}
